using System;
using System.Collections.Generic;
using System.Linq;

namespace DB.Models.Extensions
{
    public static class DbModelExtensions
    {
        public static T CopyValues<T>(this T target, 
            T source, 
            IEnumerable<string> excludedProps = null)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop =>
            {
                return prop.CanRead 
                       && prop.CanWrite 
                       && (excludedProps is null || !excludedProps.Contains(prop.Name));
            });

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value, null);
            }

            return target;
        }
    }
}