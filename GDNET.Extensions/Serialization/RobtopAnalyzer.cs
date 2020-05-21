using System;
using System.Collections.Generic;
using System.Reflection;
using GDNET.Extensions.Attributes;

namespace GDNET.Extensions.Serialization
{
    /// <summary>
    /// This is an analyzer. It splits GD responses into readable forms, or classes.
    /// Unfortunately, Madoka made this so it's bulky as hell.
    /// </summary>
    public static class RobtopAnalyzer
    {
        public static T DeserializeObject<T>(string value, char charToSplit = ':')
            where T : new()
        {
            var result = new Dictionary<int, object>();

            if (value == null)
                return new T();

            var seperated = value.Replace('~', ' ').Split(charToSplit);

            for (var i = 0; i < seperated.Length - 1;) // we want to skip by 2 in order to skip the value.
            {
                int.TryParse(seperated[i], out var key);
                i++;
                var val = seperated[i];

                result.Add(key, val);

                i++;
            }

            // Now let's try to set the value.
            var t = new T();

            // Get all properties
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(false);

                foreach (var attr in attrs)
                {
                    var webAttr = attr as GdProperty;

                    if (webAttr != null)
                    {
                        result.TryGetValue(webAttr.Key, out var toSet);
                        var type = prop.PropertyType;
                        object changedType;

                        try
                        {
                            if (type.GetTypeInfo().IsEnum && toSet != null)
                                changedType = Enum.ToObject(type, int.Parse(toSet.ToString() ?? string.Empty));
                            else if (int.TryParse(toSet?.ToString(), out var toNumber))
                                changedType = Convert.ChangeType(toNumber, type);
                            else if (toSet != null)
                                changedType = Convert.ChangeType(toSet, type);
                            else
                                changedType = null;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);

                            throw;
                        }

                        // Finally set them
                        prop.SetValue(t, changedType);
                    }
                }
            }

            return t;
        }

        public static List<T> DeserializeObjectList<T>(string value, char charToSplit = ':', char charToAddToList = '|')
            where T : new()
        {
            var list = new List<Dictionary<int, object>>();

            var listValues = value.Split(charToAddToList);

            listValues.ForEach(v =>
            {
                var dictionary = new Dictionary<int, object>();

                var seperated = v.Split(charToSplit);

                for (var i = 0; i < seperated.Length - 1;) // we want to skip by 2 in order to skip the value.
                {
                    int.TryParse(seperated[i], out var key);
                    i++;
                    var val = seperated[i];

                    try
                    {
                        dictionary.Add(key, val);
                    }
                    catch
                    {
                        //...
                    }

                    i++;
                }

                list.Add(dictionary);
            });

            // Now let's try to set the value.
            var listType = new List<T>();

            // Get all properties
            list.ForEach(dictionary =>
            {
                var type = new T();

                var props = type.GetType().GetProperties();

                foreach (var prop in props)
                {
                    var attrs = prop.GetCustomAttributes(false);

                    foreach (var attr in attrs)
                    {
                        var webAttr = attr as GdProperty;

                        if (webAttr != null)
                        {
                            dictionary.TryGetValue(webAttr.Key, out var toSet);
                            var propType = prop.PropertyType;
                            object changedType;

                            try
                            {
                                if (toSet != null)
                                    changedType = Convert.ChangeType(toSet, propType);
                                else
                                    changedType = null;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);

                                throw;
                            }

                            // Finally set them
                            prop.SetValue(type, changedType);
                        }
                    }
                }

                listType.Add(type);
            });

            return listType;
        }
    }
}