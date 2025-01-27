namespace CVB.NET.Abstractions.Ioc.ServiceLocator
{
    using System;

    public interface IReadOnlyNamedServiceLocator
    {
        bool HasInstanceRegistration<TService>(string name)
            where TService : class;

        bool HasInstanceRegistration(Type serviceType, string name);

        TService Resolve<TService>(string name)
            where TService : class;

        object Resolve(Type serviceType, string name);
    }
}