using System;

namespace CVB.NET.Abstractions.Ioc
{
    public interface IDependencyService : IDependencyInstaller, IDependencyResolver
    {
        bool HasRegistration<TService>() where TService : class;
        bool HasRegistration<TService>(string name) where TService : class;
        bool HasRegistration(Type tService);
        bool HasRegistration(Type tService, string name);

        void Teardown();
    }
}