namespace CVB.NET.Reflection.Caching.Interface
{
    using Cached;

    public interface IReflectionMemberAccessor
    {
        CachedEventInfo[] Events { get; }
        CachedMethodInfo[] Methods { get; }
        CachedPropertyInfo[] Properties { get; }
    }
}