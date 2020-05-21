using System;
using System.ComponentModel;
using System.Linq;

namespace GDNET.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum genericEnum)
        {
            var genericEnumType = genericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(genericEnum.ToString());

            if (memberInfo.Length <= 0) return genericEnum.ToString();

            var attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attribs.Length > 0 ? ((DescriptionAttribute)attribs.ElementAt(0)).Description : genericEnum.ToString();
        }
    }
}