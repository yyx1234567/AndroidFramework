using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinqUtils
{
    public static class Linq
    {
        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            List<TResult> sources = new List<TResult>();
            foreach (var item in source)
            {
                sources.Add(selector.Invoke(item));
            }
            return sources;
        }

        public static TSource First<TSource>(this IEnumerable<TSource> source)
        {
            foreach (var item in source)
            {
                return item;
            }
            return default(TSource);
        }
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            List<TSource> sources = new List<TSource>();
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                    sources.Add(item);
            }
            return sources;
        }

        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            ICollection<TSource> collectionoft = source as ICollection<TSource>;
            if (collectionoft != null) return collectionoft.Count;
            ICollection collection = source as ICollection;
            if (collection != null) return collection.Count;
            int count = 0;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                checked
                {
                    while (e.MoveNext()) count++;
                }
            }
            return count;
        }

        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            int count = 0;
            foreach (TSource element in source)
            {
                checked
                {
                    if (predicate(element)) count++;
                }
            }
            return count;
        }


        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            return new List<TSource>(source);
        }

        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            int index = 0;
            var result = new TSource[source.Count()];
            foreach (var item in source)
            {
                result[index] = item;
                index++;
            }
            return result;
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            Dictionary<TKey, TElement> result = new Dictionary<TKey, TElement>();
            foreach (var item in source)
            {
                result.Add(keySelector.Invoke(item), elementSelector.Invoke(item));
            }
            return result;
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                if (list.Count > 0) return list[0];
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (e.MoveNext()) return e.Current;
                }
            }
            return default(TSource);
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (TSource element in source)
            {
                if (predicate(element)) return element;
            }
            return default(TSource);
        }
    }
}