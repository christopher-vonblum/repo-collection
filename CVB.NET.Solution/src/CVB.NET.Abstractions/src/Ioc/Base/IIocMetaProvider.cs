namespace CVB.NET.Abstractions.Ioc.Base
{
    using System;
    using System.Collections.Generic;

    public interface IIocMetaProvider
    {
        bool HasServiceRegistration(Type tService);
        bool HasServiceRegistration(Type tService, IReadOnlyDictionary<string, string> keys);
        bool HasServiceRegistration(Type tService, string name);
        bool HasServiceRegistration<TService>() where TService : class;
        bool HasServiceRegistration<TService>(IReadOnlyDictionary<string, string> keys) where TService : class;
        bool HasServiceRegistration<TService>(string name) where TService : class;

        Type ResolveImplementationType(Type tService);
        Type ResolveImplementationType(Type tService, IReadOnlyDictionary<string, string> keys);
        Type ResolveImplementationType(Type tService, string name);
        Type ResolveImplementationType<TService>();
        Type ResolveImplementationType<TService>(IReadOnlyDictionary<string, string> keys);
        Type ResolveImplementationType<TService>(string name);
    }
}