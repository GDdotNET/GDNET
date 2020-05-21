using System;

namespace GDNET.Extensions
{
    public static class ArrayExtensions
    {
        public static string FormatEnum<T>(this T[] array, string separator)
            where T : Enum
        {
            var res = "";

            foreach (var item in array) res += $"{Convert.ToInt32(item)},";

            return res.Remove(res.Length - 1, 1);
        }
    }
}