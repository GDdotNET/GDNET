using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDNET.Extensions
{
    public static class ArrayExtensions
    {
        public static string FormatEnum<T>(this T[] array, string separator)
            where T : Enum
        {
            string res = "";

            foreach (T item in array)
            {
                res += $"{Convert.ToInt32(@item)},";
            }

            return res.Remove(res.Length - 1, 1);
        }
    }
}
