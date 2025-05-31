using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioPlayer.Extension
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Forces execution of the current linq stack
        /// </summary>
        public static IEnumerable<T> Actualize<T>(this IEnumerable<T> collection)
        {
            return collection.ToList();
        }

        ///// <summary>
        ///// Returns filtered collection using the provided distinctness selector
        ///// </summary>
        //public static IEnumerable<T> DistinctBy<T, TDistinct>(this IEnumerable<T> collection,
        //                                                           Func<T, TDistinct> distinctSelector) where TDistinct : IComparable
        //{
        //    return collection.GroupBy(item => distinctSelector(item))
        //                     .Select(group => group.First());
        //}
    }
}
