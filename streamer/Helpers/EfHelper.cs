using System.Collections.Generic;
using System.Linq;

namespace streamer.Helpers
{
    /// <summary>
    /// EfHelper
    /// </summary>
    public static class EfHelper
    {
        /// <summary>
        /// Sort
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ascending"></param>
        /// <param name="sortingProperty"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> Sort<TSource>(this IEnumerable<TSource> source, bool ascending, string sortingProperty)
        {
            if (ascending)
                return source.OrderBy(item => item.GetReflectedPropertyValue(sortingProperty));
            else
                return source.OrderByDescending(item => item.GetReflectedPropertyValue(sortingProperty));
        }

        private static object GetReflectedPropertyValue(this object subject, string field)
        {
            return subject.GetType().GetProperty(field).GetValue(subject, null);
        }
    }
}
