namespace CVB.NET.Reflection.Caching.Wrapper
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Base;
    using Cached;
    using Interface;

    public abstract class FunctionInfoWrapperBase<TMemberInfo> : MemberInfoWrapperBase<TMemberInfo>, IReflectionFunctionMember where TMemberInfo : MethodBase
    {
        public ParameterInfo[] ParameterInfos => parameterInfos.Value;
        public CachedParameterInfo[] CachedParameterInfos => cachedParameterInfos.Value;

        private DebuggableLazy<ParameterInfo[]> parameterInfos { get; }
        private DebuggableLazy<CachedParameterInfo[]> cachedParameterInfos { get; }

        protected FunctionInfoWrapperBase(TMemberInfo memberInfo) : base(memberInfo)
        {
            parameterInfos = new DebuggableLazy<ParameterInfo[]>(GetParameterInfos);
            cachedParameterInfos = new DebuggableLazy<CachedParameterInfo[]>(() => Enumerable.ToArray(ParameterInfos.Select(ReflectionCache.Get<CachedParameterInfo>)));
        }

        public override string GetCacheKeyIdentifier()
        {
            return "[MethodBase]:" + GetFunctionCacheKey();
        }

        ParameterInfo[] IReflectionFunctionMember.GetParameterInfos()
        {
            return GetParameterInfos();
        }

        protected string GetFunctionCacheKey()
        {
            return InnerReflectionInfo.MetadataToken + GetGenericIdentity();
        }

        private string GetGenericIdentity()
        {
            if (!InnerReflectionInfo.IsGenericMethod)
            {
                return string.Empty;
            }

            if (InnerReflectionInfo.IsGenericMethodDefinition)
            {
                return string.Join("|", InnerReflectionInfo.GetGenericArguments().Select(a => a.Name));
            }

            return string.Join("|", InnerReflectionInfo.GetGenericArguments().Select(CachedType.GetGenericTypeIdentityString));
        }

        protected virtual ParameterInfo[] GetParameterInfos()
        {
            return InnerReflectionInfo.GetParameters();
        }
    }
}