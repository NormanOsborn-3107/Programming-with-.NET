using System.Dynamic;
using System.Reflection;

namespace PRN232.LMS.Services.Helpers
{
    public static class DataShaper
    {
        public static ExpandoObject ShapeData<T>(this T entity, string? fieldsString)
        {
            var dataToReturn = new ExpandoObject();

            if (entity == null)
                return dataToReturn;

            var propertyInfoList = GetPropertyInfos<T>(fieldsString);

            foreach (var propertyInfo in propertyInfoList)
            {
                var propertyValue = propertyInfo.GetValue(entity);
                ((IDictionary<string, object?>)dataToReturn).Add(propertyInfo.Name, propertyValue);
            }

            return dataToReturn;
        }

        public static IEnumerable<ExpandoObject> ShapeData<T>(this IEnumerable<T> entities, string? fieldsString)
        {
            var propertyInfoList = GetPropertyInfos<T>(fieldsString);
            var expandoObjectList = new List<ExpandoObject>();

            foreach (var entity in entities)
            {
                var dataToReturn = new ExpandoObject();
                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(entity);
                    ((IDictionary<string, object?>)dataToReturn).Add(propertyInfo.Name, propertyValue);
                }
                expandoObjectList.Add(dataToReturn);
            }

            return expandoObjectList;
        }

        private static IEnumerable<PropertyInfo> GetPropertyInfos<T>(string? fieldsString)
        {
            var propertyInfoList = new List<PropertyInfo>();
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (string.IsNullOrWhiteSpace(fieldsString))
            {
                return propertyInfos;
            }

            var fieldsAfterSplit = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

                if (propertyInfo != null)
                {
                    propertyInfoList.Add(propertyInfo);
                }
            }

            return propertyInfoList;
        }
    }
}
