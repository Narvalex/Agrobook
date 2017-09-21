using System;

namespace Agrobook
{
    public static class ObjectExtensions
    {
        public static TResult Transform<TSource, TResult>(this TSource o, Func<TSource, TResult> map)
        {
            return map.Invoke(o);
        }
    }
}
