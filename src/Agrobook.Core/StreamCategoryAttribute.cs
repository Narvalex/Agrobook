using System;
using System.Linq;

namespace Agrobook.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StreamCategoryAttribute : Attribute
    {
        public StreamCategoryAttribute(string categoryName)
        {
            this.CategoryName = categoryName;
        }

        public string CategoryName { get; }

        public static string GetFullStreamName<T>(string streamName) => GetFullStreamName(typeof(T), streamName);

        public static string GetFullStreamName(Type type, string streamName)
        {
            var att = GetCustomAttributes(type)
                        .FirstOrDefault(a => a is StreamCategoryAttribute);

            return att is null ? streamName
                    : $"{((StreamCategoryAttribute)att).CategoryName}-{streamName}";
        }
    }
}
