namespace CVB.NET.DataAccess.MetaData.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Attributes;
    using Reflection.Caching;
    using Reflection.Caching.Cached;
    using Views;

    public static class ModelBaseTypeUtils
    {
        public static bool InheritsFromModelRootBaseType(OrmTypeMetaDataInfoViewBase ormType)
        {
            return GetModelRootBaseType(ormType) != null;
        }

        public static TOrmTypeMetaDataView[] GetAllDependencyTypes<TOrmTypeMetaDataView>(TOrmTypeMetaDataView ormType) where TOrmTypeMetaDataView : OrmTypeMetaDataInfoViewBase
        {
            List<TOrmTypeMetaDataView> dependencyTypes = new List<TOrmTypeMetaDataView>();

            TOrmTypeMetaDataView[] baseTypes = GetModelBaseDependencyTypes(ormType);

            TOrmTypeMetaDataView[] foreignTypes = GetModelForeignDependencyTypes(ormType);

            foreach (TOrmTypeMetaDataView foreignOrmType in foreignTypes)
            {
                if (!dependencyTypes.Contains(foreignOrmType))
                {
                    dependencyTypes.Add(foreignOrmType);
                }
            }

            foreach (TOrmTypeMetaDataView baseOrmType in baseTypes)
            {
                TOrmTypeMetaDataView[] baseForeignTypes = GetModelForeignDependencyTypes(baseOrmType);

                if (!dependencyTypes.Contains(baseOrmType))
                {
                    dependencyTypes.Add(baseOrmType);
                }

                foreach (TOrmTypeMetaDataView baseForeignOrmType in baseForeignTypes)
                {
                    if (!dependencyTypes.Contains(baseForeignOrmType))
                    {
                        dependencyTypes.Add(baseForeignOrmType);
                    }
                }
            }

            return dependencyTypes.ToArray();
        }

        public static TOrmTypeMetaDataView[] GetModelBaseDependencyTypes<TOrmTypeMetaDataView>(TOrmTypeMetaDataView ormType) where TOrmTypeMetaDataView : OrmTypeMetaDataInfoViewBase
        {
            Type baseType = ormType.InnerReflectionInfo;

            if (ReflectionCache.Get<CachedType>(ormType.InnerReflectionInfo).Attributes.OfType<ModelBaseAttribute>().Any())
            {
                return new TOrmTypeMetaDataView[0];
            }

            List<TOrmTypeMetaDataView> baseTypes = new List<TOrmTypeMetaDataView>();

            while (baseType != typeof (object) && baseType != null)
            {
                baseType = baseType.BaseType;

                baseTypes.Add(ReflectionCache.Get<TOrmTypeMetaDataView>(baseType));

                if (ReflectionCache.Get<CachedType>(baseType).Attributes.OfType<ModelBaseAttribute>().Any())
                {
                    break;
                }
            }

            return baseTypes.ToArray();
        }

        public static TOrmTypeMetaDataView[] GetModelForeignDependencyTypes<TOrmTypeMetaDataView>(TOrmTypeMetaDataView ormType) where TOrmTypeMetaDataView : OrmTypeMetaDataInfoViewBase
        {
            List<TOrmTypeMetaDataView> foreignTypes = new List<TOrmTypeMetaDataView>();

            foreach (CachedPropertyInfo foreignPropertyInfo in ormType.ForeignModelReferences)
            {
                TOrmTypeMetaDataView foreignPropertyOrmTypeView =
                    ReflectionCache.Get<TOrmTypeMetaDataView>(
                        foreignPropertyInfo.InnerReflectionInfo.PropertyType);

                if (InheritsFromModelRootBaseType(foreignPropertyOrmTypeView) && !foreignTypes.Contains(ormType))
                {
                    foreignTypes.Add(foreignPropertyOrmTypeView);
                }
            }

            return foreignTypes.ToArray();
        }

        public static TOrmTypeMetaDataView GetModelRootBaseType<TOrmTypeMetaDataView>(TOrmTypeMetaDataView ormType) where TOrmTypeMetaDataView : OrmTypeMetaDataInfoViewBase
        {
            Type baseType = ormType.InnerReflectionInfo;


            if (ReflectionCache.Get<CachedType>(ormType.InnerReflectionInfo).Attributes.OfType<ModelBaseAttribute>().Any())
            {
                return ormType;
            }

            while (baseType != typeof (object))
            {
                if ((baseType = baseType.BaseType) == null)
                {
                    break;
                }

                if (ReflectionCache.Get<CachedType>(baseType).Attributes.OfType<ModelBaseAttribute>().Any())
                {
                    return ReflectionCache.Get<TOrmTypeMetaDataView>(baseType);
                }
            }

            return null;
        }

        public static int GetModelTypeLevel(OrmTypeMetaDataInfoViewBase ormType)
        {
            return GetModelBaseDependencyTypes(ormType).Length;
        }
    }
}