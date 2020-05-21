using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GDNET.Client.Attributes;

namespace GDNET.Client.Objects
{
    /// <summary>
    /// A normal, everyday object in level data.
    /// </summary>
    public class Object
    {
        /// <summary>
        /// The id of the object, or what GD parses it as.
        /// </summary>
        [LevelObjectAttribute]
        public int Id { get; set; }

        /// <summary>
        /// The x-position of the object.
        /// </summary>
        [LevelObjectAttribute("2")]
        public float X { get; set; }

        /// <summary>
        /// The y-position of the object.
        /// </summary>
        [LevelObjectAttribute("3")]
        public float Y { get; set; }

        #region Static Methods
        /// <summary>
        /// A method to parse an object string to a proper object.
        /// </summary>
        /// <param name="objString">The object string.</param>
        /// <returns>An object.</returns>
        public static Object Parse(string objString)
        {
            if (string.IsNullOrEmpty(objString))
                return null;

            var result = new Dictionary<string, object>();

            var separated = objString.Split(',');

            for (var i = 0; i < separated.Length - 1;) // we want to manually increment the index count in the loop.
            {
                var key = separated[i];
                var val = separated[++i];

                result.Add(key, val);
                i++;
            }

            var obj = new Object();

            // Let's see if there's any inherited type with the same ID.
            var derivedTypes = new List<Type>();

            foreach (var domain in AppDomain.CurrentDomain.GetAssemblies())
            {
                var types = domain.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(Object)) && !t.IsAbstract);

                derivedTypes.AddRange(types);
            }

            foreach (var type in derivedTypes)
            {
                var attrs = type.GetCustomAttributes(false);

                foreach (var attr in attrs)
                    if (attr is LevelObjectAttribute objAttr)
                    {
                        result.TryGetValue("1", out var idObj);

                        if (idObj != null && objAttr.Id == idObj.ToString())
                            obj = (Object)Activator.CreateInstance(type);
                    }
            }

            var props = obj?.GetType().GetProperties();

            Debug.Assert(props != null, nameof(props) + " != null");

            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(false);

                foreach (var attr in attrs)
                    if (attr is LevelObjectAttribute objAttr)
                    {
                        result.TryGetValue(objAttr.Id, out var toSet);
                        var type = prop.PropertyType;
                        object changedType;

                        try
                        {
                            changedType = toSet != null ? Convert.ChangeType(toSet, type) : null;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);

                            throw;
                        }

                        prop.SetValue(obj, changedType);
                    }
            }

            return obj;
        }
        #endregion
    }
}