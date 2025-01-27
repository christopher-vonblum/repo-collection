namespace CVB.NET.Reflection.Caching.Cached
{
    using System.Diagnostics;
    using System.Reflection;
    using Aspects.MethodBehaviors;
    using Base;
    using Interface;
    using PostSharp.Patterns.Contracts;
    using Wrapper;

    [DebuggerDisplay("{DebuggerDisplayString}")]
    public sealed class CachedMemberInfo : MemberInfoWrapperBase<MemberInfo>
    {
        private ICacheableWrapper WrappedCacheEntry => wrappedCacheEntry.Value;

        private string DebuggerDisplayString
            => InnerReflectionInfo.Name + ", " + InnerReflectionInfo.MemberType;

        private DebuggableLazy<ICacheableWrapper> wrappedCacheEntry;

        internal CachedMemberInfo([NotNull] MemberInfo memberInfo) : base(memberInfo)
        {
            wrappedCacheEntry = new DebuggableLazy<ICacheableWrapper>(() => ReflectionCache.Get(memberInfo));
        }

        public override string GetCacheKeyIdentifier()
        {
            return "[MemberWrapper]:" + WrappedCacheEntry.GetCacheKeyIdentifier();
        }

        [TrierDoerAspect]
        public static implicit operator CachedMemberInfo(MemberInfo memberInfo)
            => ReflectionCache.Get<CachedMemberInfo>(memberInfo);

        [TrierDoerAspect]
        public static implicit operator MemberInfo(CachedMemberInfo cachedMemberInfo)
            => cachedMemberInfo.InnerReflectionInfo;

        [TrierDoerAspect]
        public static explicit operator ConstructorInfo(CachedMemberInfo cachedMemberInfo)
            => (ConstructorInfo) cachedMemberInfo.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedMemberInfo(ConstructorInfo constructorInfo)
            => ReflectionCache.Get<CachedMemberInfo>(constructorInfo);

        [TrierDoerAspect]
        public static explicit operator PropertyInfo(CachedMemberInfo cachedMemberInfo)
            => (PropertyInfo) cachedMemberInfo.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedMemberInfo(PropertyInfo propertyInfo)
            => ReflectionCache.Get<CachedMemberInfo>(propertyInfo);

        [TrierDoerAspect]
        public static explicit operator MethodInfo(CachedMemberInfo cachedMemberInfo)
            => (MethodInfo) cachedMemberInfo.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedMemberInfo(MethodInfo methodInfo)
            => ReflectionCache.Get<CachedMemberInfo>(methodInfo);
    }
}