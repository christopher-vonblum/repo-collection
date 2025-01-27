namespace CVB.NET.Reflection.Caching.Cached
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using Aspects.MethodBehaviors;
    using PostSharp.Patterns.Contracts;
    using Wrapper;

    [DebuggerDisplay("{ParameterInfo.ParameterType} {ParameterInfo.Name}")]
    public sealed class CachedParameterInfo : ReflectionSubInfoWrapperBase<ParameterInfo, CachedMethodBase>
    {
        internal CachedParameterInfo([NotNull] ParameterInfo parameterInfo) : base(parameterInfo)
        {
        }

        protected override Attribute[] GetAttributes() => InnerReflectionInfo.GetCustomAttributes().ToArray();

        [TrierDoerAspect]
        public static implicit operator ParameterInfo(CachedParameterInfo cachedPropertyInfo)
            => cachedPropertyInfo.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedParameterInfo(ParameterInfo propertyInfo)
            => ReflectionCache.Get<CachedParameterInfo>(propertyInfo);

        public override string GetCacheKeyIdentifier()
        {
            return "[Parameter]:" + InnerReflectionInfo.MetadataToken;
        }

        protected override CachedMethodBase GetDeclaringReflectionInfo()
        {
            return (MethodBase) InnerReflectionInfo.Member;
        }
    }
}