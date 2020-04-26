using System;
using System.Diagnostics.CodeAnalysis;

namespace GDNET.Client.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class LevelObjectAttribute : Attribute
    {
        public string Id;

        public LevelObjectAttribute([NotNull] string objId = "1")
        {
            Id = objId;
        }
    }
}