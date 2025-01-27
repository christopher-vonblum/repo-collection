namespace CVB.NET.Abstractions.Ioc.ServiceLocator
{
    using System;

    public interface IReadOnlyServiceLocator
    {
        bool HasInstanceRegistration<TService>()
            where TService : class;

        bool HasInstanceRegistration(Type serviceType);

        TService GetInstance<TService>()
            where TService : class;

        object GetInstance(Type serviceType);
    }
}
