namespace CVB.NET.Reflection.Caching.Cached
{
    using System.Reflection;
    using Aspects.MethodBehaviors;
    using PostSharp.Patterns.Contracts;
    using Wrapper;

    public class CachedConstructorInfo : FunctionInfoWrapperBase<ConstructorInfo>
    {
        internal CachedConstructorInfo([NotNull] ConstructorInfo constructorInfo)
            : base(constructorInfo)
        {
        }

        [TrierDoerAspect]
        public static implicit operator ConstructorInfo(CachedConstructorInfo cachedConstructorInfo)
            => cachedConstructorInfo.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedConstructorInfo(ConstructorInfo constructorInfo)
            => ReflectionCache.Get<CachedConstructorInfo>(constructorInfo);

        public override string GetCacheKeyIdentifier()
        {
            return "[Constructor]:" + GetFunctionCacheKey();
        }
    }
}