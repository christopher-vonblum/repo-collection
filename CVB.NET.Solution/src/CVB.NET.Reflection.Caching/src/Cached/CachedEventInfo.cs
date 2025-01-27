namespace CVB.NET.Reflection.Caching.Cached
{
    using System.Reflection;
    using Aspects.MethodBehaviors;
    using Base;
    using Wrapper;

    public sealed class CachedEventInfo : MemberInfoWrapperBase<EventInfo>
    {
        public CachedMethodInfo AddMethod => addMethod.Value;
        public CachedMethodInfo RemoveMethod => removeMethod.Value;
        private DebuggableLazy<CachedMethodInfo> addMethod;

        private DebuggableLazy<CachedMethodInfo> removeMethod;

        internal CachedEventInfo(EventInfo eventInfo) : base(eventInfo)
        {
            addMethod = new DebuggableLazy<CachedMethodInfo>(() => eventInfo.AddMethod);
            removeMethod = new DebuggableLazy<CachedMethodInfo>(() => eventInfo.RemoveMethod);
        }

        [TrierDoerAspect]
        public static implicit operator EventInfo(CachedEventInfo cachedFieldInfo)
            => cachedFieldInfo.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedEventInfo(EventInfo eventInfo)
            => ReflectionCache.Get<CachedEventInfo>(eventInfo);

        public override string GetCacheKeyIdentifier()
        {
            return "[Event]:" + InnerReflectionInfo.MetadataToken;
        }
    }
}