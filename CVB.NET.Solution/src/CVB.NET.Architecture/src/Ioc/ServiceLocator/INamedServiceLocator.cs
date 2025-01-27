namespace CVB.NET.Abstractions.Ioc.ServiceLocator
{
    using System;

    public interface INamedServiceLocator : IReadOnlyNamedServiceLocator
    {
        void Register<TService, TImplementation>(string name)
            where TImplementation : class, TService
            where TService : class;

        void Register(
            Type tService,
            Type tImplementation,
            string name);

        void Remove<TService>(string name)
            where TService : class;

        void Remove(Type tService, string name);
    }
}