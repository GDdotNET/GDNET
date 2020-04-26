using System;
using System.Diagnostics.CodeAnalysis;

namespace GDNET.Client.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GdXmlAttribute : Attribute
    {
        public string Key;

        public GdXmlAttribute([NotNull] string gdId = "k1")
        {
            Key = gdId;
        }
    }
}