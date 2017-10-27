using System;
using System.Collections.Generic;

namespace Eventing
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action.Invoke(item);
        }
    }
}
