namespace CVB.NET.Reflection.Caching.Wrapper
{
    using System;
    using System.Reflection;
    using Base;
    using Interface;

    internal class LookupInfoWrapperBase : FunctionInfoWrapperBase<MethodInfo>
    {
        public object Value => value.Value;
        private DebuggableLazy<ICacheableWrapper> builtin;
        private DebuggableLazy<object> value;

        public LookupInfoWrapperBase(MethodInfo lookupMethod, object invocationTarget, ICustomAttributeProvider reflectionInfo, Func<object> customLookupData = null) : base(lookupMethod)
        {
            builtin = new DebuggableLazy<ICacheableWrapper>(() => ReflectionCache.Get(reflectionInfo));
            if (customLookupData != null)
            {
                value = new DebuggableLazy<object>(() => customLookupData() ?? InnerReflectionInfo.Invoke(invocationTarget, new[] {reflectionInfo}));
            }
            else
            {
                value = new DebuggableLazy<object>(() => InnerReflectionInfo.Invoke(invocationTarget, new[] {reflectionInfo}));
            }
        }

        public override string GetCacheKeyIdentifier()
        {
            return builtin.Value.GetPropertyCacheKey("[Lookup]:" + base.GetCacheKeyIdentifier());
        }
    }
}