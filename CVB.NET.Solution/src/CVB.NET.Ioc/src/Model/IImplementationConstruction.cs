namespace CVB.NET.Ioc.Model
{
    using Reflection.Caching.Cached;

    public interface IImplementationConstruction
    {
        InstanceLifeStyle InstanceLifeStyle { get; set; }
        CachedType Type { get; }
        object CreateInstance();
    }
}