namespace CVB.NET.Abstractions.Ioc.Provider.Model
{
    using System.Collections.Generic;
    using Container.Model;

    public interface IServiceInstanceKey
    {
        IServiceRegistrationKey Key { get; }
        IReadOnlyDictionary<string, object> VaryBy { get; }
    }
}