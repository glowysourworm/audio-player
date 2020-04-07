using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioPlayer.Extension
{
    public static class IListExtension
    {
        /// <summary>
        /// Clears and re-creates list using IList.Add from the provided collection.
        /// </summary>
        public static void ReCreate<T>(this IList<T> list,
                                       IEnumerable<T> collection)
        {
            list.Clear();

            foreach (var item in collection)
                list.Add(item);
        }

        /// <summary>
        /// Clears and re-creates list using IList.Add from the provided collection. Option to sort the collection.
        /// </summary>
        public static void ReCreate<T, TSelector>(this IList<T> list,
                                                  IEnumerable<T> collection,
                                                  Func<T, TSelector> sortSelector)
        {
            list.Clear();

            // With Sorting
            foreach (var item in collection.OrderBy(x => sortSelector(x)))
                list.Add(item);
        }
    }
}
