using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlogDemo.Infrastructure.Services
{
    public class TypeHelperService : ITypeHelperService
    {
        /// <summary>
        /// Determine if the field is legal and exists in the T class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <returns>True if fields are legal otherwise false</returns>
        public bool TypeHasProperties<T>(string fields)
        {
            if(string.IsNullOrEmpty(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                if (string.IsNullOrEmpty(propertyName))
                {
                    continue;
                }

                var propertyInfo = typeof(T)
                    .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
