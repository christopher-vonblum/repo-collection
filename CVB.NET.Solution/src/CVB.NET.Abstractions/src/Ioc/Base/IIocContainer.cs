namespace CVB.NET.Abstractions.Ioc.Base
{
    using System;
    using System.Collections.Generic;

    using CVB.NET.Abstractions.Ioc.Container.Registration;

    public interface IIocContainer : IIocInjectionProvider, IIocMetaProvider
    {
        void Register(Action<RegistrationContext> register);
        
        object ResolveService(Type tService);
        object ResolveService(Type tService, IReadOnlyDictionary<string, string> keys);
        object ResolveService(Type tService, string name);
        TService ResolveService<TService>() where TService : class;
        TService ResolveService<TService>(IReadOnlyDictionary<string, string> keys) where TService : class;
        TService ResolveService<TService>(string name) where TService : class;
    }
}
