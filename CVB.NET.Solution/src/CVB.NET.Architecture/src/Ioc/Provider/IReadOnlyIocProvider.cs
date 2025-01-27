namespace CVB.NET.Abstractions.Ioc.Provider
{
    using System;
    using System.Collections.Generic;
    using Container;
    using Model;
    using ServiceLocator;

    public interface IReadOnlyIocProvider
    {
        IReadOnlyIocContainer Container { get; }
        TService GetInstance<TService>(IReadOnlyDictionary<string, object> varyServiceBy = null, IReadOnlyDictionary<string, object> varyInstanceBy = null) where TService : class;
        object GetInstance(Type tService, IReadOnlyDictionary<string, object> varyServiceBy = null, IReadOnlyDictionary<string, object> varyInstanceBy = null);
        IServiceInstance GetInstance(IServiceInstanceKey serviceInstanceKey);
        IEnumerable<IServiceInstance> QueryServiceInstances();
    }
}