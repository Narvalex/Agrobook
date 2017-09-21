using System;

namespace Agrobook
{
    public static class ObjectExtensions
    {
        public static TResult Transform<TSource, TResult>(this TSource o, Func<TSource, TResult> transform)
        {
            return transform.Invoke(o);
        }
    }
}
