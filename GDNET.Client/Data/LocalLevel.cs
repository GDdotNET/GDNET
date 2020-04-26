using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GDNET.Client.Attributes;
using GDNET.Client.IO.Saves;
using Base64 = GDNET.Client.Encryption.Base64;

namespace GDNET.Client.Data
{
    /// <summary>
    /// A local level, on the host computer; still in its XML form. Refer to <see cref="Data.Level"/> for the parseable form of this.
    /// </summary>
    public class LocalLevel
    {
        [GdXml("k2")]
        public string Name { get; set; }

        [GdXml("k5")]
        public string Author { get; set; }

        [GdXml("k4")]
        public string LevelString { get; set; }

        [GdXml("k3")]
        public string Description { get; set; }

        public static LocalLevel Load(string levelString)
        {
            var lvl = new LocalLevel();
            var props = typeof(LocalLevel).GetProperties();

            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(false);

                foreach (var attr in attrs)
                {
                    if (attr is GdXmlAttribute gdattr)
                        prop.SetValue(lvl, LocalLevelManager.GetPair(levelString, gdattr.Key, prop.PropertyType).Value);
                }
            }

            return lvl;
        }

        public override string ToString()
        {
            return LevelString;
        } 
    }
}
