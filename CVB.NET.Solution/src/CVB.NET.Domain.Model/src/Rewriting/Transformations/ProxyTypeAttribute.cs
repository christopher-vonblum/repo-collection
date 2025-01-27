namespace CVB.NET.Domain.Model.Rewriting.Transformations
{
    using System;

    public class ProxyTypeAttribute : Attribute
    {
        public Type ProxyType { get; }

        public ProxyTypeAttribute(string proxyType)
        {
            ProxyType = Type.GetType(proxyType);
        }
    }
}