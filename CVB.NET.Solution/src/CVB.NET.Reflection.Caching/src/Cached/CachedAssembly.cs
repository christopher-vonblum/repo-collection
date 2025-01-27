namespace CVB.NET.Reflection.Caching.Cached
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Aspects.MethodBehaviors;
    using Base;
    using Wrapper;

    public sealed class CachedAssembly : ReflectionInfoWrapperBase<Assembly>
    {
        public Type[] Types => types.Value;

        public CachedType[] CachedTypes => cachedTypes.Value;

        private DebuggableLazy<CachedType[]> cachedTypes;

        private DebuggableLazy<Type[]> types;

        public CachedAssembly(Assembly assembly) : base(assembly)
        {
            types = new DebuggableLazy<Type[]>(() => InnerReflectionInfo.GetTypes());

            cachedTypes = new DebuggableLazy<CachedType[]>(() => Enumerable.ToArray(Types.Select(ReflectionCache.Get<CachedType>)));
        }

        public override string GetCacheKeyIdentifier()
        {
            return "[Assembly]:" + InnerReflectionInfo.FullName;
        }

        [TrierDoerAspect]
        public static implicit operator Assembly(CachedAssembly cachedAssembly) => cachedAssembly.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedAssembly(Assembly assembly) => ReflectionCache.Get<CachedAssembly>(assembly);
    }
}