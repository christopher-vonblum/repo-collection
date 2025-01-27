namespace CVB.NET.DataAccess.MetaData.Lookups
{
    using System;
    using System.Linq;
    using DataAccess.MetaData.Attributes;
    using DataAccess.MetaData.Utils;
    using DataAccess.MetaData.Views;
    using Reflection.Caching;
    using Reflection.Caching.Cached;

    public static class OrmLookups
    {
        public static Func<CachedType, CachedPropertyInfo[]> DeclaredForeignModelReferences = type => type
            .Properties
            .Where(
                prop =>
                    ModelBaseTypeUtils.InheritsFromModelRootBaseType(
                        ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(
                            prop.InnerReflectionInfo.PropertyType))
                    && prop.InnerReflectionInfo.DeclaringType == type
                    && !prop.Attributes.OfType<NoColumnAttribute>().Any()).ToArray();

        public static Func<CachedType, CachedPropertyInfo[]> DeclaredPrimitiveValueProperties = type => type
            .Properties
            .Where(prop => !prop.Attributes.OfType<IdentifierAttribute>().Any()
                           && !prop.Attributes.OfType<NoColumnAttribute>().Any()
                           && prop.InnerReflectionInfo.DeclaringType == type)
            .ToArray();

        public static Func<CachedType, CachedPropertyInfo[]> ForeignModelReferencePropertyLookup = type => type
            .Properties
            .Where(prop => ModelBaseTypeUtils.InheritsFromModelRootBaseType(ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(prop.InnerReflectionInfo.PropertyType))
                           && !prop.Attributes.OfType<NoColumnAttribute>().Any())
            .ToArray();

        public static Func<CachedType, CachedPropertyInfo[]> MandatoryPropertyLookup = type => type
            .Properties
            .Where(prop => (prop.Attributes.OfType<IdentifierAttribute>().Any()
                            || prop.Attributes.OfType<ModelTypeDefinitionAttribute>().Any())
                           && !prop.Attributes.OfType<NoColumnAttribute>().Any()
                           && !prop.Attributes.OfType<AllowNullAttribute>().Any())
            .ToArray();

        public static Func<CachedType, CachedPropertyInfo[]> PrimaryKeyPropertyLookup = type => type
            .Properties
            .Where(prop => prop.Attributes.OfType<IdentifierAttribute>().SingleOrDefault() != null
                           && !prop.Attributes.OfType<NoColumnAttribute>().Any())
            .ToArray();

        public static Func<CachedType, CachedPropertyInfo[]> PrimitiveValuePropertiesLookup = type => type
            .Properties
            .Where(prop => !ModelBaseTypeUtils.InheritsFromModelRootBaseType(ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(prop.InnerReflectionInfo.PropertyType))
                           && !prop.Attributes.OfType<IdentifierAttribute>().Any()
                           && !prop.Attributes.OfType<NoColumnAttribute>().Any())
            .ToArray();
    }
}