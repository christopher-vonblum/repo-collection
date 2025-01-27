namespace CVB.NET.Abstractions.Ioc.Container
{
    using System;
    using System.Collections.Generic;
    using Activator;
    using Model;

    public interface IIocContainer : IReadOnlyIocContainer
    {
        event Action<IServiceRegistrationKey> OnRegister;
        event Action<IServiceRegistrationKey> OnRemove;

        void Register<TService, TImplementation>(
            IReadOnlyDictionary<string, object> varyServiceBy = null, 
            IReadOnlyDictionary<string, object> customConstructorArguments = null, 
            ConstructorInjectionMode injectionMode = ConstructorInjectionMode.UseCustomArgsOverResolving,
            IServiceActivator customActivator = null) 
                where TService : class
                where TImplementation : TService;

        void Register(
            Type tService,
            Type tImplementation,
            IReadOnlyDictionary<string, object> varyServiceBy = null,
            IReadOnlyDictionary<string, object> customConstructorArguments = null,
            ConstructorInjectionMode injectionMode = ConstructorInjectionMode.UseCustomArgsOverResolving,
            IServiceActivator customActivator = null);

        void Remove<TService>(
            IReadOnlyDictionary<string, object> varyServiceBy = null);

        void Remove(
            Type tService,
            IReadOnlyDictionary<string, object> varyServiceBy = null);
    }
}