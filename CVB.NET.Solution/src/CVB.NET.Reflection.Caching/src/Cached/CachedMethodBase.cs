namespace CVB.NET.Reflection.Caching.Cached
{
    using System.Reflection;
    using Aspects.MethodBehaviors;
    using Base;
    using Interface;
    using PostSharp.Patterns.Contracts;
    using Wrapper;

    public sealed class CachedMethodBase : FunctionInfoWrapperBase<MethodBase>
    {
        private IReflectionFunctionMember WrappedCacheEntry => wrappedCacheEntry.Value;

        private DebuggableLazy<IReflectionFunctionMember> wrappedCacheEntry;

        public CachedMethodBase([NotNull] MethodBase memberInfo) : base(memberInfo)
        {
            wrappedCacheEntry = new DebuggableLazy<IReflectionFunctionMember>(() => (IReflectionFunctionMember) ReflectionCache.Get(memberInfo));
        }

        public override string GetCacheKeyIdentifier()
        {
            return "[MethodBaseWrapper]:" + WrappedCacheEntry.GetCacheKeyIdentifier();
        }

        protected override ParameterInfo[] GetParameterInfos()
        {
            return WrappedCacheEntry.GetParameterInfos();
        }

        [TrierDoerAspect]
        public static implicit operator MethodBase(CachedMethodBase methodBase)
            => methodBase.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedMethodBase(MethodBase methodBase)
            => ReflectionCache.Get<CachedMethodBase>(methodBase);
    }
}