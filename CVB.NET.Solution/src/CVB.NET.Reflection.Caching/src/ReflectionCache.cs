namespace CVB.NET.Reflection.Caching
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Caching;
    using Builtin;
    using Cached;
    using Exceptions.Reflection;
    using Interface;
    using PostSharp.Patterns.Contracts;
    using Wrapper;

    public static class ReflectionCache
    {
        /// <summary>
        /// MemoryCache that stores meta data lookups
        /// </summary>
        private static MemoryCache Cache { get; }

        /// <summary>
        /// Cached reflected ConstructorInfos for cached meta data classes
        /// </summary>
        private static ConcurrentDictionary<string, ConstructorInfo> CachedMetaDataCtors { get; } = new ConcurrentDictionary<string, ConstructorInfo>();

        /// <summary>
        /// Static ctor
        /// </summary>
        static ReflectionCache()
        {
            Cache = new MemoryCache("reflectionCache");
        }

        public static object GetLookup([NotNull] Delegate lookupMethod, [NotNull] ICustomAttributeProvider reflectionInfo)
        {
            return GetOrAddLookupWrapper(lookupMethod, reflectionInfo).Value;
        }

        public static TValue GetLookup<TValue>([NotNull] Delegate lookupMethod, [NotNull] ICustomAttributeProvider reflectionInfo)
        {
            return (TValue) GetOrAddLookupWrapper(lookupMethod, reflectionInfo).Value;
        }

        public static TValue GetLookup<TValue, TReflectionInfo>([NotNull] Func<TReflectionInfo, TValue> lookupMethod, [NotNull] ICustomAttributeProvider reflectionInfo)
        {
            return (TValue) GetOrAddLookupWrapper(lookupMethod, reflectionInfo).Value;
        }

        public static ICacheableWrapper Get([NotNull] ICustomAttributeProvider reflectionInfo)
        {
            if (reflectionInfo is Type && ((Type) reflectionInfo).IsEnum)
            {
                return Get(typeof (CachedEnum), reflectionInfo);
            }

            if (reflectionInfo is Type && ((Type) reflectionInfo).IsInterface)
            {
                return Get(typeof (CachedInterfaceType), reflectionInfo);
            }

            return Get(IntegratedWrapperMap.IntegratedWrappers[reflectionInfo.GetType().BaseType], reflectionInfo);
        }

        public static TCachedReflectionInfo Get<TCachedReflectionInfo>([NotNull] ICustomAttributeProvider reflectionInfo) where TCachedReflectionInfo : ICacheableWrapper
        {
            return (TCachedReflectionInfo) Get(typeof (TCachedReflectionInfo), reflectionInfo);
        }

        public static ICacheableWrapper Get([NotNull] Type tCachedReflectionInfo, [NotNull] ICustomAttributeProvider reflectionInfo)
        {
            ConstructorInfo constructor = CachedMetaDataCtors.GetOrAdd(CachedType.GetGenericTypeIdentityString(tCachedReflectionInfo), (ctor) => GetConstructor(tCachedReflectionInfo, reflectionInfo.GetType()));

            if (constructor == null)
            {
                throw new NoCompatibleConstructorFoundException(tCachedReflectionInfo, new[] {nameof(reflectionInfo)});
            }

            ICacheableWrapper cachedReflectionInfo = (ICacheableWrapper) constructor.Invoke(new object[] {reflectionInfo});

            return GetOrAddReflectionInfo(cachedReflectionInfo);
        }

        internal static LookupInfoWrapperBase GetOrAddLookupWrapper(Delegate lookupMethod, ICustomAttributeProvider reflectionInfo, string cacheModifier = null, Func<object> customData = null)
        {
            return
                (LookupInfoWrapperBase)
                    GetOrAddReflectionInfo(new LookupInfoWrapperBase(lookupMethod.Method, lookupMethod.Target, reflectionInfo, customData), cacheModifier);
        }

        private static ConstructorInfo GetConstructor(Type tCachedReflectionInfo, Type reflectionInfoType)
        {
            ConstructorInfo[] ctors = tCachedReflectionInfo.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            ConstructorInfo selected =
                ctors.OrderBy(ctor => ctor.IsPublic).FirstOrDefault(ctor =>
                                                                    {
                                                                        var parameters = ctor.GetParameters();

                                                                        if (parameters.Length != 1)
                                                                        {
                                                                            return false;
                                                                        }

                                                                        var param0 = parameters[0];

                                                                        return reflectionInfoType == param0.ParameterType
                                                                            || reflectionInfoType.IsSubclassOf(param0.ParameterType);
                                                                    });

            if (selected == null)
            {
                throw new NoCompatibleConstructorFoundException(tCachedReflectionInfo, new[] {"reflectionInfo"});
            }

            return selected;
        }

        private static ICacheableWrapper GetOrAddReflectionInfo(ICacheableWrapper reflectionInfo, string cacheModifier = null)
        {
            string cacheKey = reflectionInfo.GetCacheKey();

            cacheKey += cacheModifier ?? string.Empty;

            ICacheableWrapper cachedEntry = (ICacheableWrapper) Cache[cacheKey];

            if (cachedEntry != null)
            {
                return cachedEntry;
            }

            return (ICacheableWrapper) (Cache[cacheKey] = reflectionInfo);
        }
    }
}