namespace CVB.NET.Reflection.Caching.Cached
{
    using System.Reflection;
    using Aspects.MethodBehaviors;
    using Wrapper;

    public sealed class CachedFieldInfo : MemberInfoWrapperBase<FieldInfo>
    {
        internal CachedFieldInfo(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        [TrierDoerAspect]
        public static implicit operator FieldInfo(CachedFieldInfo cachedFieldInfo)
            => cachedFieldInfo.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedFieldInfo(FieldInfo fieldInfo)
            => ReflectionCache.Get<CachedFieldInfo>(fieldInfo);

        public override string GetCacheKeyIdentifier()
        {
            return "[Field]:" + InnerReflectionInfo.MetadataToken;
        }
    }
}