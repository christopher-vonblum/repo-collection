namespace CVB.NET.Domain.Model.Rewriting
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Reflection.Caching.Cached;

    public static class ProxyPropertiesLookup
    {
        public static Func<Type, CachedPropertyInfo[]> GetPublicStaticProperties = type => type
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Select(prop => (CachedPropertyInfo) prop)
            .ToArray();
    }
}