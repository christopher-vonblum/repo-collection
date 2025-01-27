namespace CVB.NET.Reflection.Caching.Interface
{
    using System;

    internal interface ICachedReflectionView : IReflectionView
    {
        object GetOrAddLookupValue(string proxyPropertyName, Func<object> getLookupValue);
    }
}