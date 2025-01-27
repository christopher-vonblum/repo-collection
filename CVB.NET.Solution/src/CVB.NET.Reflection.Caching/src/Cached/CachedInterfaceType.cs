namespace CVB.NET.Reflection.Caching.Cached
{
    using System;
    using System.Collections.Concurrent;
    using Aspect;
    using Aspects.MethodBehaviors;
    using Interface;
    using Lookup;
    using Wrapper;

    public interface ICachedInterface
    {
        
    }

    public sealed class CachedInterfaceType : ReflectionSubInfoWrapperBase<Type, CachedAssembly>, IReflectionMemberAccessor, ICachedReflectionView
    {
        [UseLookup(typeof(CachedInterfaceLookups), nameof(CachedInterfaceLookups.GetBaseInterfaces))]
        public CachedInterfaceType[] BaseInterfaceTypes { get; }

        [UseLookup(typeof (CachedTypeLookups), nameof(CachedTypeLookups.GetGenericTypeDefinition))]
        public Type GenericTypeDefinition { get; set; }

        private readonly ConcurrentDictionary<string, object> cachedLookups = new ConcurrentDictionary<string, object>();

        public CachedInterfaceType(Type info) : base(info)
        {
        }

        public object GetOrAddLookupValue(string proxyPropertyName, Func<object> getLookupValue)
        {
            return cachedLookups.GetOrAdd(proxyPropertyName, (name) => getLookupValue());
        }

        [UseLookup(typeof (CachedInterfaceLookups), nameof(CachedInterfaceLookups.GetEvents))]
        public CachedEventInfo[] Events { get; }

        [UseLookup(typeof (CachedInterfaceLookups), nameof(CachedInterfaceLookups.GetMethods))]
        public CachedMethodInfo[] Methods { get; }

        [UseLookup(typeof (CachedInterfaceLookups), nameof(CachedInterfaceLookups.GetProperties))]
        public CachedPropertyInfo[] Properties { get; }

        public override string GetCacheKeyIdentifier()
        {
            return "[Interface]:" + CachedType.GetGenericTypeIdentityString(InnerReflectionInfo);
        }

        protected override CachedAssembly GetDeclaringReflectionInfo()
        {
            return InnerReflectionInfo.Assembly;
        }

        [TrierDoerAspect]
        public static implicit operator Type(CachedInterfaceType cachedType) => cachedType.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedInterfaceType(Type reflectedType) => ReflectionCache.Get<CachedInterfaceType>(reflectedType);
    }
}