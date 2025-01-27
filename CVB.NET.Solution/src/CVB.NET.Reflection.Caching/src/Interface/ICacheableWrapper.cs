namespace CVB.NET.Reflection.Caching.Interface
{
    public interface ICacheableWrapper : IReflectable
    {
        string GetCacheKeyIdentifier();
        string GetCacheKey();
        string GetPropertyCacheKey(string property);
    }
}