using System;
using System.Linq;

namespace Eventing.Core.Domain
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StreamCategoryAttribute : Attribute
    {
        public StreamCategoryAttribute(string categoryName)
        {
            this.CategoryName = categoryName;
        }

        public string CategoryName { get; }

        public static string GetFullStreamName<T>(string streamId) => GetFullStreamName(typeof(T), streamId);

        public static string GetFullStreamName(Type type, string streamName)
        {
            var category = GetCategory(type);

            return category is null ? streamName : $"{category}-{streamName}";
        }

        public static string GetCategory<T>()
        {
            return GetCategory(typeof(T));
        }

        public static string GetCategory(Type type)
        {
            var att = GetCustomAttributes(type)
                        .FirstOrDefault(a => a is StreamCategoryAttribute);
            return att is null ? null : ((StreamCategoryAttribute)att).CategoryName;
        }

        public static string GetCategoryProjectionStream<T>()
        {
            return GetCategoryProjectionStream(typeof(T));
        }

        public static string GetCategoryProjectionStream(Type type)
        {
            var category = GetCategory(type);
            return $"$ce-{category}";
        }
    }
}
