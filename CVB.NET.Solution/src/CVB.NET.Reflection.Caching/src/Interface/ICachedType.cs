namespace CVB.NET.Reflection.Caching.Interface
{
    using Cached;

    public interface ICachedType : IReflectionView, IAttributeLocation, ISubInfo
    {
        CachedType[] GenericTypeArguments { get; }
        CachedType GenericTypeDefinition { get; }
        bool HasDefaultConstructor { get; }
        CachedConstructorInfo DefaultConstructor { get; }
        CachedConstructorInfo[] Constructors { get; }
        CachedPropertyInfo[] Properties { get; }
        CachedFieldInfo[] Fields { get; }
        CachedMethodInfo[] Methods { get; }
        CachedInterfaceType[] Interfaces { get; }
        CachedEventInfo[] Events { get; }
        CachedMethodInfo[] StaticMethods { get; }
    }
}