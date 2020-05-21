using System;
using System.Diagnostics.CodeAnalysis;

namespace GDNET.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GdProperty : Attribute
    {
        public readonly int Key;

        public GdProperty([NotNull] int gdId = 0)
        {
            Key = gdId;
        }
    }
}