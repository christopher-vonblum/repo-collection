namespace CVB.NET.Reflection.Caching.Wrapper
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using Aspect;
    using Cached;
    using Interface;

    [ReflectionViewAspect]
    public abstract class ReflectionInfoViewWrapperBase<TMetaData> : ReflectionInfoWrapperBase<TMetaData>, IReflectionView<TMetaData>, ICachedReflectionView where TMetaData : ICustomAttributeProvider
    {
        private ConcurrentDictionary<string, object> cachedLookups = new ConcurrentDictionary<string, object>();

        protected ReflectionInfoViewWrapperBase(TMetaData reflected) : base(reflected)
        {
        }

        public object GetOrAddLookupValue(string proxyPropertyName, Func<object> getLookupValue)
        {
            return cachedLookups.GetOrAdd(proxyPropertyName, (name) => getLookupValue());
        }

        public override string GetCacheKeyIdentifier()
        {
            ICacheableWrapper cached = ReflectionCache.Get(InnerReflectionInfo);
            return cached.GetPropertyCacheKey("[View]:" + CachedType.GetGenericTypeIdentityString(this.GetType()));
        }
    }
}