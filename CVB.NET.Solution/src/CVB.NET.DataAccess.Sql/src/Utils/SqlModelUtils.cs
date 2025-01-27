namespace CVB.NET.DataAccess.Sql.Utils
{
    using System.Linq;
    using Attributes;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching.Cached;

    public static class SqlModelUtils
    {
        public static string GetForeignColumnName([NotNull] CachedPropertyInfo foreignPropertyInfo, [NotNull] CachedPropertyInfo foreignPrimaryKeyPropertyInfo)
        {
            string foreignPropertyinfoName, foreignPropertyInfoPrimaryKeyName;

            foreignPropertyinfoName = GetCustomColumnName(foreignPropertyInfo) ?? foreignPropertyInfo.InnerReflectionInfo.Name;

            foreignPropertyInfoPrimaryKeyName = GetCustomColumnName(foreignPrimaryKeyPropertyInfo) ?? foreignPrimaryKeyPropertyInfo.InnerReflectionInfo.Name;

            return $"{foreignPropertyinfoName}_{foreignPropertyInfoPrimaryKeyName}";
        }

        public static string GetCustomColumnName([NotNull] CachedPropertyInfo propertyInfo)
        {
            CustomColumnNameAttribute columnNameAttribute;

            if ((columnNameAttribute = propertyInfo.Attributes.OfType<CustomColumnNameAttribute>().FirstOrDefault()) != null)
            {
                return columnNameAttribute.ColummnName;
            }

            return null;
        }
    }
}