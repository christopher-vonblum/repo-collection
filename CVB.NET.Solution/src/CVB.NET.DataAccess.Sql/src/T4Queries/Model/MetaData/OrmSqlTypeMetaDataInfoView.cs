namespace CVB.NET.DataAccess.Sql.T4Queries.Model.MetaData
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using DataAccess.MetaData.Utils;
    using DataAccess.MetaData.Views;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching;
    using Reflection.Caching.Aspect;
    using Reflection.Caching.Cached;
    using SubModels;
    using TreeModelRepository.Schema;

    public class OrmSqlTypeMetaDataInfoView : OrmTypeMetaDataInfoViewBase
    {
        [UseLookup(typeof (SqlLookups), nameof(SqlLookups.TableName))]
        public string TableName { get; }

        [UseLookup(typeof (SqlLookups), nameof(SqlLookups.TablePrimaryKeyDescriptions))]
        public IColumnDescription[] TablePrimaryKeyDescriptions { get; }

        [UseLookup(typeof (SqlLookups), nameof(SqlLookups.TableForeignKeyDescriptions))]
        public IColumnDescription[] TableForeignKeyDescriptions { get; }

        [UseLookup(typeof (SqlLookups), nameof(SqlLookups.PrimitiveColumnDescriptions))]
        public IColumnDescription[] PrimitiveColumnDescriptions { get; }

        [UseLookup(typeof (SqlLookups), nameof(SqlLookups.TableDescription))]
        public ITableDescription TableDescription { get; }

        public bool HasTypeReferencePropertyInfo => TypeReferencePropertyInfo != null;

        [UseLookup(typeof (SqlLookups), nameof(SqlLookups.TypeReferencePropertyInfo))]
        public CachedPropertyInfo TypeReferencePropertyInfo { get; }

        public OrmSqlTypeMetaDataInfoView(Type reflectedType) : base(reflectedType)
        {
        }

        public static object GetRootIdentityModel([NotNull] Type tModel)
        {
            OrmSqlTypeMetaDataInfoView ormType = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(tModel);

            CachedType identityModelType = ModelBaseTypeUtils.GetModelRootBaseType(ormType).Cast<CachedType>();

            CachedMethodInfo getIdentityMethodInfo =
                identityModelType
                    .Methods
                    .FirstOrDefault(method =>
                        method.CachedParameterInfos.Length == 0
                        && method.InnerReflectionInfo.Name.Equals(nameof(ModelBase.GetRootIdentity)));

            return getIdentityMethodInfo?.InnerReflectionInfo.Invoke(identityModelType.DefaultConstructor.InnerReflectionInfo.Invoke(null), null);
        }

        public static object GetTypeIdentityModel(Type tModel)
        {
            OrmSqlTypeMetaDataInfoView ormType = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(tModel);

            CachedType identityModelType = ormType.Cast<CachedType>();

            CachedMethodInfo getIdentityMethodInfo =
                identityModelType
                    .Methods
                    .FirstOrDefault(method =>
                        method.CachedParameterInfos.Length == 0
                        && method.InnerReflectionInfo.Name.Equals(nameof(ModelBase.GetTypeIdentity)));

            if (getIdentityMethodInfo == null)
            {
                return GetTypeIdentityModel(tModel.BaseType);
            }

            return getIdentityMethodInfo.InnerReflectionInfo.Invoke(((CachedType) tModel).DefaultConstructor.InnerReflectionInfo.Invoke(null), null);
        }
    }

    public class LookupTableDescriptionImplementation : ITableDescription
    {
        public OrmSqlTypeMetaDataInfoView OrmTypeMetaDataInfoView { get; set; }
        public string Schema { get; set; }
        public string Table { get; set; }
        public ImmutableList<IColumnDescription> Columns { get; set; }
        public IEnumerable<KeyValuePair<string, List<IColumnDescription>>> ForeignKeyDescriptionsGroupedByTable { get; set; }
    }
}