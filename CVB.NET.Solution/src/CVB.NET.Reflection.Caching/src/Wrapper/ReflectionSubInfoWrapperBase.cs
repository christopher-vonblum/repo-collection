namespace CVB.NET.Reflection.Caching.Wrapper
{
    using System.Reflection;
    using Base;
    using Interface;

    public abstract class ReflectionSubInfoWrapperBase<TReflectionInfo, TParent> : ReflectionInfoWrapperBase<TReflectionInfo>, ISubInfo<TParent> where TReflectionInfo : ICustomAttributeProvider where TParent : ICacheableWrapper
    {
        private DebuggableLazy<TParent> declaringReflectionInfo;

        public ReflectionSubInfoWrapperBase(TReflectionInfo info) : base(info)
        {
            declaringReflectionInfo = new DebuggableLazy<TParent>(GetDeclaringReflectionInfo);
        }

        public TParent DeclaringReflectionInfo => declaringReflectionInfo.Value;

        ICacheableWrapper ISubInfo.DeclaringReflectionInfo => DeclaringReflectionInfo;

        public sealed override string GetCacheKey()
        {
            return $"key[{GetCacheKeyInternal(this)}]";
        }

        protected abstract TParent GetDeclaringReflectionInfo();

        private string GetCacheKeyInternal(ISubInfo subInfo)
        {
            if (subInfo.DeclaringReflectionInfo is ISubInfo)
            {
                ISubInfo parent = (ISubInfo) subInfo.DeclaringReflectionInfo;

                return GetCacheKeyInternal(parent) + "| |" + subInfo.GetCacheKeyIdentifier();
            }

            return subInfo.DeclaringReflectionInfo.GetCacheKeyIdentifier() + "| |" + subInfo.GetCacheKeyIdentifier();
        }
    }
}