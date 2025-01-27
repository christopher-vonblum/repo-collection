namespace CVB.NET.DataAccess.Sql.T4Queries.Queries.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DataAccess.Exception;
    using MetaData.Utils;
    using MetaData.Views;
    using Model.MetaData;
    using Model.SubModels;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching;
    using Reflection.Caching.Cached;
    using Sql.Utils;

    public static class SqlQueryUtils
    {
        public static string GetSeperatedColumnNames([NotNull] IEnumerable<INamedColumn> columns, string seperator, bool useBrackets)
        {
            if (useBrackets)
            {
                return string.Join(seperator, columns.Select(col => "[" + col.Name + "]"));
            }

            return string.Join(seperator, columns.Select(col => col.Name));
        }

        public static string GetSeperatedReferenceColumnNames([NotNull] IEnumerable<IColumnDescription> columns, string seperator, bool useBrackets)
        {
            if (useBrackets)
            {
                return string.Join(seperator, columns.Select(col => "[" + col.ReferenceColumn + "]"));
            }

            return string.Join(seperator, columns.Select(col => col.ReferenceColumn));
        }

        public static string GetCommaSeperatedValues([NotNull] IEnumerable<ColumnValue> columns)
        {
            return string.Join(", ", columns.Select(col => "'" + col.Value + "'"));
        }

        public static string MakeSqlCausalityChain(string schema, IList<ColumnValue> causalities, LogicalOperator logicalOperator)
        {
            string @operator = null;

            switch (logicalOperator)
            {
                case LogicalOperator.And:
                    @operator = "AND ";
                    break;
                case LogicalOperator.Or:
                    @operator = "OR ";
                    break;
                case LogicalOperator.Enumeration:
                    @operator = ", ";
                    break;
                default:
                    throw new NotSupportedException(logicalOperator.ToString());
            }

            return string.Join(@operator, causalities.Select(cau => GetCausality(schema, cau)));
        }

        private static string GetCausality(string schema, ColumnValue columnValue)
        {
            StringBuilder causalityString = new StringBuilder();

            causalityString.Append("[");

            causalityString.Append(schema);

            causalityString.Append("].[");

            causalityString.Append(columnValue.Name);

            causalityString.Append("]");

            causalityString.Append(" = ");

            causalityString.Append("'");

            causalityString.Append(columnValue.Value);

            causalityString.Append("' ");

            return causalityString.ToString();
        }

        public static string GetJoinPrimaryKeys(List<ColumnValue> keys, string tableName)
        {
            return string.Join(tableName, keys.Select(key => "[" + tableName + "].[" + key.Name + "]"));
        }

        public static List<ColumnValue> GetForeignKeyValues(IDictionary<CachedPropertyInfo, object> properties, OrmSqlTypeMetaDataInfoView type)
        {
            List<ColumnValue> foreignKeys = new List<ColumnValue>();

            foreach (IColumnDescription foreignKeyDescription in type.TableForeignKeyDescriptions)
            {
                KeyValuePair<CachedPropertyInfo, object> propertyPair = properties.SingleOrDefault(prop =>
                    prop.Key.InnerReflectionInfo.Name.Equals(foreignKeyDescription.PropertyInfo.InnerReflectionInfo.Name)
                    && prop.Key.InnerReflectionInfo.DeclaringType == foreignKeyDescription.DeclaringType.InnerReflectionInfo
                    && ModelBaseTypeUtils.InheritsFromModelRootBaseType(ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(prop.Key.InnerReflectionInfo.PropertyType)));

                if (propertyPair.Value == null)
                {
                    continue;
                }

                OrmTypeMetaDataInfoViewBase propertyTypeInfoView = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(propertyPair.Key.InnerReflectionInfo.PropertyType);

                foreignKeys.AddRange(
                    propertyTypeInfoView
                        .PrimaryKeyProperties
                        .Select(primaryKey =>
                            new ColumnValue(
                                foreignKeyDescription,
                                primaryKey.InnerReflectionInfo.GetValue(propertyPair.Value))));
            }

            return foreignKeys;
        }

        public static List<ColumnValue> GetPrimaryKeyValues(IDictionary<CachedPropertyInfo, object> properties, OrmSqlTypeMetaDataInfoView type)
        {
            List<ColumnValue> primarykeys = new List<ColumnValue>();

            foreach (IColumnDescription primaryKeyDescription in type.TablePrimaryKeyDescriptions)
            {
                KeyValuePair<CachedPropertyInfo, object> propertyPair = properties.SingleOrDefault(prop =>
                    prop.Key.InnerReflectionInfo.Name.Equals(primaryKeyDescription.PropertyInfo.InnerReflectionInfo.Name));

                if (propertyPair.Value == null)
                {
                    throw new PrimaryKeyPropertiesAreRequiredException(primaryKeyDescription.PropertyInfo.InnerReflectionInfo.Name);
                }

                primarykeys.Add(
                    new ColumnValue(
                        primaryKeyDescription,
                        propertyPair.Value));
            }

            return primarykeys;
        }

        public static List<ColumnValue> GetPrimitiveColumnValues(IDictionary<CachedPropertyInfo, object> properties, OrmSqlTypeMetaDataInfoView type)
        {
            List<ColumnValue> primitiveValues = new List<ColumnValue>();

            foreach (CachedPropertyInfo property in type.DeclaredPrimitiveValueProperties.Where(prop => SqlTypeMapping.HasSqlType(prop.InnerReflectionInfo.PropertyType)))
            {
                KeyValuePair<CachedPropertyInfo, object> propertyPair = properties.SingleOrDefault(prop =>
                    prop.Key.InnerReflectionInfo.Name.Equals(property.InnerReflectionInfo.Name)
                    && prop.Key.InnerReflectionInfo.DeclaringType == property.DeclaringReflectionInfo);

                if (propertyPair.Value == null)
                {
                    continue;
                }

                primitiveValues.Add(
                    new ColumnValue(
                        new ColumnDescription(
                            type,
                            property,
                            SqlTypeMapping.GetSqlType(property.InnerReflectionInfo.PropertyType),
                            property.InnerReflectionInfo.Name),
                        propertyPair.Value));
            }

            return primitiveValues;
        }
    }
}