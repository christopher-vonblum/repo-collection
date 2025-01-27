using CVB.NET.Reflection.Caching.Aspect;
using CVB.NET.Reflection.Caching.Cached;
using CVB.NET.Reflection.Caching.Wrapper;

namespace CVB.NET.DataAccess.MetaData.Views
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Lookups;
    using Reflection.Caching;
    using Utils;

    [DebuggerDisplay("{ReflectedType.FullName}")]
    public class OrmTypeMetaDataInfoViewBase : ReflectionInfoViewWrapperBase<Type>
    {
        public OrmTypeMetaDataInfoViewBase BaseModel
        {
            get
            {
                if (HasBaseModel)
                {
                    return ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(InnerReflectionInfo.BaseType);
                }

                return null;
            }
        }

        public bool HasBaseModel => ModelBaseTypeUtils.InheritsFromModelRootBaseType(ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(InnerReflectionInfo.BaseType));

        [UseLookup(typeof (OrmLookups), nameof(OrmLookups.DeclaredForeignModelReferences))]
        public CachedPropertyInfo[] DeclaredForeignModelReferences { get; }

        [UseLookup(typeof (OrmLookups), nameof(OrmLookups.DeclaredPrimitiveValueProperties))]
        public CachedPropertyInfo[] DeclaredPrimitiveValueProperties { get; }

        [UseLookup(typeof (OrmLookups), nameof(OrmLookups.PrimaryKeyPropertyLookup))]
        public CachedPropertyInfo[] PrimaryKeyProperties { get; }

        [UseLookup(typeof (OrmLookups), nameof(OrmLookups.PrimitiveValuePropertiesLookup))]
        public CachedPropertyInfo[] PrimitiveValueProperties { get; }

        [UseLookup(typeof (OrmLookups), nameof(OrmLookups.ForeignModelReferencePropertyLookup))]
        public CachedPropertyInfo[] ForeignModelReferences { get; }

        [UseLookup(typeof (OrmLookups), nameof(OrmLookups.MandatoryPropertyLookup))]
        public CachedPropertyInfo[] MandatoryProperties { get; }

        public CachedPropertyInfo[] ColumnProperties
            => PrimaryKeyProperties.Concat(ForeignModelReferences).Concat(PrimitiveValueProperties).ToArray();

        public OrmTypeMetaDataInfoViewBase(Type reflectedType) : base(reflectedType)
        {
        }
    }
}