using System;
using System.Collections.Generic;
using System.Linq;

namespace CampaniasLito.Classes
{
    public static class LinqExtenions
    {
        public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TFirstKey> firstKeySelector, Func<TSource, TSecondKey> secondKeySelector, Func<IEnumerable<TSource>, TValue> aggregate)
        {
            var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

            var l = source.ToLookup(firstKeySelector);
            foreach (var item in l)
            {
                var dict = new Dictionary<TSecondKey, TValue>();
                retVal.Add(item.Key, dict);

                var subdictAll = source.ToLookup(secondKeySelector);

                var subdict = item.ToLookup(secondKeySelector);
                foreach (var subitemAll in subdictAll)
                {
                    var subitem = subdict.SingleOrDefault(t => t.Key.ToString() == subitemAll.Key.ToString());
                    if (subitem != null)
                        dict.Add(subitemAll.Key, aggregate(subitem));
                    else
                        dict.Add(subitemAll.Key, default(TValue));
                }
            }

            return retVal;
        }
    }
}