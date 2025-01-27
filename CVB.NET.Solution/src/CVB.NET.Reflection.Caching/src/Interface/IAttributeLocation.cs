namespace CVB.NET.Reflection.Caching.Interface
{
    using System;

    public interface IAttributeLocation
    {
        Attribute[] Attributes { get; }
        Attribute[] InheritedAttributes { get; }
    }
}