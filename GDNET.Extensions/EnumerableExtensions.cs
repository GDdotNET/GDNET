using System;
using System.Collections.Generic;

namespace GDNET.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Performs an action on all the items in an IEnumerable collection.
        /// </summary>
        /// <typeparam name="T">The type of the items stored in the collection.</typeparam>
        /// <param name="collection">The collection to iterate on.</param>
        /// <param name="action">The action to be performed.</param>
        /// owned by ppy
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection == null) return;

            foreach (var item in collection)
                action(item);
        }
    }
}