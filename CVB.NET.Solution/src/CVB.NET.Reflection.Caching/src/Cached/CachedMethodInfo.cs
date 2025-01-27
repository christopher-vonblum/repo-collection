namespace CVB.NET.Reflection.Caching.Cached
{
    using System.Reflection;
    using Aspects.MethodBehaviors;
    using PostSharp.Patterns.Contracts;
    using Wrapper;

    public sealed class CachedMethodInfo : FunctionInfoWrapperBase<MethodInfo>
    {
        internal CachedMethodInfo([NotNull] MethodInfo methodInfo) : base(methodInfo)
        {
        }

        [TrierDoerAspect]
        public static implicit operator MethodInfo(CachedMethodInfo cachedMethodInfo)
            => cachedMethodInfo.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedMethodInfo(MethodInfo methodinfo)
            => ReflectionCache.Get<CachedMethodInfo>(methodinfo);

        public override string GetCacheKeyIdentifier()
        {
            return "[Method]:" + GetFunctionCacheKey();
        }
    }
}