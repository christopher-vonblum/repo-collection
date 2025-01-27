namespace CVB.NET.Abstractions.Ioc.ServiceLocator
{
    using System;

    public interface IServiceLocator : IReadOnlyServiceLocator
    {
        void Register<TService, TImplementation>()
            where TImplementation : class, TService
            where TService : class;

        void Register(
            Type tService,
            Type tImplementation);

        void Remove<TService>()
            where TService : class;

        void Remove(Type tService);
    }
}