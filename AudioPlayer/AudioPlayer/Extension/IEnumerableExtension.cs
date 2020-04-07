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
    }
}
