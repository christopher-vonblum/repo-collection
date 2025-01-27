namespace CVB.NET.Abstractions.Ioc.Provider
{
    using System.Collections.Generic;
    using Container;

    public interface IIocProvider : IReadOnlyIocProvider
    {
        new IIocContainer Container { get; }
    }
}