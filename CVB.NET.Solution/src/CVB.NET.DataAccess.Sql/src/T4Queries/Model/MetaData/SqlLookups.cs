namespace CVB.NET.DataAccess.Sql.T4Queries.Model.MetaData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.MetaData.Attributes;
    using DataAccess.MetaData.Views;
    using Reflection.Caching;
    using Reflection.Caching.Cached;
    using SubModels;
    using Utils;

    public static class SqlLookups
    {
        public static Func<Type, string> TableName => type => type.GUID.ToString("B");

        public static Func<Type, IColumnDescription[]> TablePrimaryKeyDescriptions => type =>
                                                                                      {
                                                                                          List<IColumnDescription> primaryKeys = new List<IColumnDescription>();

                                                                                          OrmSqlTypeMetaDataInfoView infoView = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(type);

                                                                                          foreach (CachedPropertyInfo primaryKeyProperty in infoView.PrimaryKeyProperties)
                                                                                          {
                                                                                              LookupColumnDescriptionImplementation primaryKeyDescription =
                                                                                                  new LookupColumnDescriptionImplementation
                                                                                                  {
                                                                                                      Type = SqlTypeMapping.GetSqlType(primaryKeyProperty.InnerReflectionInfo.PropertyType),
                                                                                                      Name = primaryKeyProperty.InnerReflectionInfo.Name,
                                                                                                      IsPrimaryKey = true
                                                                                                  };

                                                                                              primaryKeyDescription.DeclaringType = infoView;

                                                                                              // primary key from base table is also a foreign key to the base table
                                                                                              if (primaryKeyProperty.InnerReflectionInfo.DeclaringType != infoView.InnerReflectionInfo)
                                                                                              {
                                                                                                  primaryKeyDescription.ReferenceTable = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(primaryKeyProperty.InnerReflectionInfo).TableName;
                                                                                                  primaryKeyDescription.ReferenceColumn = primaryKeyProperty.InnerReflectionInfo.Name;
                                                                                              }
                                                                                              else
                                                                                              {
                                                                                                  primaryKeyDescription.ReferenceColumn = null;
                                                                                              }

                                                                                              primaryKeyDescription.PropertyInfo = primaryKeyProperty;

                                                                                              primaryKeys.Add(primaryKeyDescription);
                                                                                          }

                                                                                          return primaryKeys.ToArray();
                                                                                      };

        public static Func<Type, IColumnDescription[]> TableForeignKeyDescriptions => type =>
                                                                                      {
                                                                                          List<IColumnDescription> foreignKeys = new List<IColumnDescription>();

                                                                                          OrmSqlTypeMetaDataInfoView infoView = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(type);

                                                                                          foreach (CachedPropertyInfo foreignModelProperty in infoView.DeclaredForeignModelReferences)
                                                                                          {
                                                                                              OrmSqlTypeMetaDataInfoView ormForeignReferenceType =
                                                                                                  ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(
                                                                                                      foreignModelProperty.InnerReflectionInfo.PropertyType);

                                                                                              string foreignModelTableName = ormForeignReferenceType.TableName;

                                                                                              foreach (CachedPropertyInfo primaryKeyProperty in ormForeignReferenceType.PrimaryKeyProperties)
                                                                                              {
                                                                                                  LookupColumnDescriptionImplementation
                                                                                                      foreignKeyReferenceDescription =
                                                                                                          new LookupColumnDescriptionImplementation
                                                                                                          {
                                                                                                              Type = SqlTypeMapping.GetSqlType(primaryKeyProperty.InnerReflectionInfo.PropertyType),
                                                                                                              Name = SqlModelUtils.GetForeignColumnName(foreignModelProperty, primaryKeyProperty),
                                                                                                              DeclaringType = foreignModelProperty.DeclaringReflectionInfo.Cast<OrmSqlTypeMetaDataInfoView>(),
                                                                                                              PropertyInfo = foreignModelProperty,
                                                                                                              ReferenceTable = foreignModelTableName,
                                                                                                              ReferenceColumn = SqlModelUtils.GetCustomColumnName(primaryKeyProperty)
                                                                                                          };

                                                                                                  if (foreignModelProperty.Attributes.OfType<AllowNullAttribute>().Any())
                                                                                                  {
                                                                                                      foreignKeyReferenceDescription.AllowNull = true;
                                                                                                  }

                                                                                                  if (foreignModelProperty.InnerReflectionInfo != primaryKeyProperty.InnerReflectionInfo
                                                                                                      && foreignModelProperty.InnerReflectionInfo.PropertyType.IsSubclassOf(infoView.InnerReflectionInfo))
                                                                                                  {
                                                                                                      foreignKeyReferenceDescription.ReferenceTable =
                                                                                                          infoView.TableName;
                                                                                                  }

                                                                                                  foreignKeys.Add(foreignKeyReferenceDescription);
                                                                                              }
                                                                                          }

                                                                                          return foreignKeys.ToArray();
                                                                                      };

        public static Func<Type, IColumnDescription[]> PrimitiveColumnDescriptions => type =>
                                                                                      {
                                                                                          OrmTypeMetaDataInfoViewBase ormTypeInfoView = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(type);
                                                                                          List<IColumnDescription> primitiveColumnDescriptions = new List<IColumnDescription>();

                                                                                          foreach (CachedPropertyInfo primitiveValueProperty in ormTypeInfoView.DeclaredPrimitiveValueProperties)
                                                                                          {
                                                                                              if (!SqlTypeMapping.HasSqlType(primitiveValueProperty.InnerReflectionInfo.PropertyType))
                                                                                              {
                                                                                                  continue;
                                                                                              }

                                                                                              LookupColumnDescriptionImplementation primitiveValueColumnDescription
                                                                                                  =
                                                                                                  new LookupColumnDescriptionImplementation
                                                                                                  {
                                                                                                      Type = SqlTypeMapping.GetSqlType(primitiveValueProperty.InnerReflectionInfo.PropertyType),
                                                                                                      Name = primitiveValueProperty.InnerReflectionInfo.Name
                                                                                                  };

                                                                                              primitiveValueColumnDescription.PropertyInfo = primitiveValueProperty;

                                                                                              if (primitiveValueProperty.Attributes.OfType<AllowNullAttribute>().Any())
                                                                                              {
                                                                                                  primitiveValueColumnDescription.AllowNull = true;
                                                                                              }

                                                                                              primitiveColumnDescriptions.Add(primitiveValueColumnDescription);
                                                                                          }

                                                                                          return primitiveColumnDescriptions.ToArray();
                                                                                      };

        public static Func<Type, ITableDescription> TableDescription => type =>
                                                                        {
                                                                            LookupTableDescriptionImplementation tableDescription = new LookupTableDescriptionImplementation();

                                                                            tableDescription.OrmTypeMetaDataInfoView = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(type);

                                                                            tableDescription.Schema = "dbo";

                                                                            tableDescription.Table = tableDescription.OrmTypeMetaDataInfoView.TableName;

                                                                            tableDescription.Columns.AddRange(tableDescription.OrmTypeMetaDataInfoView.TablePrimaryKeyDescriptions);

                                                                            tableDescription.Columns.AddRange(tableDescription.OrmTypeMetaDataInfoView.TableForeignKeyDescriptions);

                                                                            tableDescription.Columns.AddRange(tableDescription.OrmTypeMetaDataInfoView.PrimitiveColumnDescriptions);

                                                                            tableDescription.ForeignKeyDescriptionsGroupedByTable = tableDescription.OrmTypeMetaDataInfoView.TableForeignKeyDescriptions.Concat(tableDescription.OrmTypeMetaDataInfoView.TablePrimaryKeyDescriptions)
                                                                                .Where(refTable => !string.IsNullOrWhiteSpace(refTable.ReferenceTable) && !string.IsNullOrWhiteSpace(refTable.ReferenceColumn))
                                                                                .GroupBy(col => col.ReferenceTable)
                                                                                .ToDictionary(k => k.Key, v => v.ToList());

                                                                            return tableDescription;
                                                                        };

        public static Func<Type, CachedPropertyInfo> TypeReferencePropertyInfo => type => ReflectionCache.Get<CachedType>(type).Properties.FirstOrDefault(re => re.Attributes.OfType<ModelTypeDefinitionAttribute>().Any());

        internal class LookupColumnDescriptionImplementation : IColumnDescription
        {
            public string Name { get; set; }
            public CachedPropertyInfo PropertyInfo { get; set; }
            public OrmSqlTypeMetaDataInfoView DeclaringType { get; set; }
            public string Type { get; set; }
            public bool IsPrimaryKey { get; set; }
            public bool IsForeignKey { get; set; }
            public string ReferenceTable { get; set; }
            public string ReferenceColumn { get; set; }
            public bool AllowNull { get; set; }
        }
    }
}