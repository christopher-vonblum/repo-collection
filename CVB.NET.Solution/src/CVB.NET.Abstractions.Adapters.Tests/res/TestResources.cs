namespace CVB.NET.Abstractions.Adapters.Tests.res
{
    using System;
    using System.Linq;
    using Reflection.Caching.Cached;

    public static class TestResources
    {
        public static readonly Func<CachedParameterInfo, string> GetDependencyNameForParameter = p => p.Attributes.OfType<NamedDependencyAttribute>().Any() ? p.Attributes.OfType<NamedDependencyAttribute>().FirstOrDefault()?.Name ?? p.InnerReflectionInfo.Name : null;
        public static readonly Func<CachedPropertyInfo, string> GetDependencyNameForProperty = p => p.Attributes.OfType<NamedDependencyAttribute>().Any() ? p.Attributes.OfType<NamedDependencyAttribute>().FirstOrDefault()?.Name ?? char.ToLowerInvariant(p.InnerReflectionInfo.Name.First()) + p.InnerReflectionInfo.Name.Substring(1) : null;
    }
}