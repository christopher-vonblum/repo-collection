namespace CVB.NET.Reflection.Caching.Cached
{
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Aspects.MethodBehaviors;
    using Base;
    using PostSharp.Patterns.Contracts;
    using Wrapper;

    public sealed class CachedPropertyInfo : MemberInfoWrapperBase<PropertyInfo>
    {
        public bool HasImplementedGetter => hasImplementedSetter.Value;
        public bool HasImplementedSetter => hasImplementedGetter.Value;
        public CachedMethodInfo GetMethod => getMethod.Value;
        public CachedMethodInfo SetMethod => setMethod.Value;

        private DebuggableLazy<CachedMethodInfo> getMethod;

        private DebuggableLazy<bool> hasImplementedGetter;
        private DebuggableLazy<bool> hasImplementedSetter;

        private DebuggableLazy<CachedMethodInfo> setMethod;

        public CachedPropertyInfo([NotNull] PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            getMethod = new DebuggableLazy<CachedMethodInfo>(()
                => propertyInfo.GetMethod);

            setMethod = new DebuggableLazy<CachedMethodInfo>(()
                => propertyInfo.SetMethod);

            hasImplementedGetter = new DebuggableLazy<bool>(() => !getMethod.Value.Attributes.OfType<CompilerGeneratedAttribute>().Any());

            hasImplementedSetter = new DebuggableLazy<bool>(() => !setMethod.Value.Attributes.OfType<CompilerGeneratedAttribute>().Any());
        }

        [TrierDoerAspect]
        public static implicit operator PropertyInfo(CachedPropertyInfo cachedPropertyInfo)
            => cachedPropertyInfo.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedPropertyInfo(PropertyInfo propertyInfo)
            => ReflectionCache.Get<CachedPropertyInfo>(propertyInfo);

        public override string GetCacheKeyIdentifier()
        {
            return "[Property]:" + InnerReflectionInfo.Name;
        }
    }
}