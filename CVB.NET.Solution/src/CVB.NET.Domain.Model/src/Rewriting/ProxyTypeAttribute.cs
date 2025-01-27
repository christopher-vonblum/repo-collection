namespace CVB.NET.Domain.Model.Rewriting
{
    using System;

    public class ProxyTypeAttribute : Attribute
    {
        public Type ProxyClass { get; }

        public ProxyTypeAttribute(string fullName)
        {
            ProxyClass = Type.GetType(fullName);
        }
    }
}