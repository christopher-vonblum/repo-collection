namespace CVB.NET.Reflection.Caching.Wrapper
{
    using System.Reflection;
    using Base;
    using Interface;

    public abstract class ReflectionInfoWrapperBase<TReflectionInfo> : CachedAttributeLocationBase, ICacheableWrapper, IReflectable<TReflectionInfo> where TReflectionInfo : ICustomAttributeProvider
    {
        private string CalculatedCacheKey;

        protected ReflectionInfoWrapperBase(TReflectionInfo info) : base(info)
        {
        }

        string ICacheableWrapper.GetCacheKey()
        {
            return CalculatedCacheKey ?? (CalculatedCacheKey = GetCacheKey());
        }

        public abstract string GetCacheKeyIdentifier();

        public string GetPropertyCacheKey(string property)
        {
            return GetCacheKey() + "->" + property;
        }

        public virtual TReflectionInfo InnerReflectionInfo => (TReflectionInfo) ((IReflectable) this).InnerReflectionInfo;

        public virtual string GetCacheKey()
        {
            return $"key[{GetCacheKeyIdentifier()}]";
        }

        public TWrapper Cast<TWrapper>() where TWrapper : ReflectionInfoWrapperBase<TReflectionInfo>
        {
            return (TWrapper) ReflectionCache.Get(typeof (TWrapper), InnerReflectionInfo);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, this))
            {
                return false;
            }
            if (object.ReferenceEquals(obj, this))
            {
                return true;
            }

            ICacheableWrapper cacheableWrapperObj = obj as ICacheableWrapper;
            if (cacheableWrapperObj != null)
            {
                return cacheableWrapperObj.GetCacheKey().Equals(this.GetCacheKey());
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.GetCacheKey().GetHashCode();
        }
    }
}