namespace CVB.NET.Abstractions.Ioc.Container
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Activator;
    using Exception;
    using Model;

    public class IocContainer : IIocContainer
    {
        private SafeAction<IServiceRegistrationKey> onRegister = new SafeAction<IServiceRegistrationKey>();

        public event Action<IServiceRegistrationKey> OnRegister
        {
            add { onRegister += value; }
            remove { onRegister -= value; }
        }

        private SafeAction<IServiceRegistrationKey> onRemove = new SafeAction<IServiceRegistrationKey>();

        public event Action<IServiceRegistrationKey> OnRemove
        {
            add { onRemove += value; }
            remove { onRemove -= value; }
        }

        private readonly ConcurrentDictionary<IServiceRegistrationKey, IServiceRegistration> services = new ConcurrentDictionary<IServiceRegistrationKey, IServiceRegistration>();

        public bool HasServiceRegistration<TService>(
            IReadOnlyDictionary<string, object> varyServiceBy = null) where TService : class
        {
            return HasServiceRegistration(typeof (TService), varyServiceBy);
        }

        public bool HasServiceRegistration(
            Type tService,
            IReadOnlyDictionary<string, object> varyServiceBy = null)
        {
            return services.ContainsKey(ServiceRegistrationKey.Create(tService, varyServiceBy));
        }

        public TService CreateInstance<TService>(
            IReadOnlyDictionary<string, object> varyServiceBy = null) where TService : class
        {
            return (TService)CreateInstance(typeof (TService), varyServiceBy);
        }

        public object CreateInstance(
            Type tService, 
            IReadOnlyDictionary<string, object> varyServiceBy = null)
        {
            IServiceRegistrationKey serviceRegistrationKey = ServiceRegistrationKey.Create(tService, varyServiceBy);
            IServiceRegistration registration;

            if (!services.TryGetValue(serviceRegistrationKey, out registration))
            {
                throw new RegistrationNotFoundException();
            }

            return registration.Activator.CreateInstance(this, registration);
        }

        public void Register<TService, TImplementation>(
            IReadOnlyDictionary<string, object> varyServiceBy = null, 
            IReadOnlyDictionary<string, object> customConstructorArguments = null, 
            ConstructorInjectionMode injectionMode = ConstructorInjectionMode.UseCustomArgsOverResolving, 
            IServiceActivator customActivator = null) where TService : class where TImplementation : TService
        {
            Register(typeof(TService), typeof(TImplementation), varyServiceBy, customConstructorArguments, injectionMode, customActivator);
        }

        public void Register(
            Type tService, 
            Type tImplementation, 
            IReadOnlyDictionary<string, object> varyServiceBy = null,
            IReadOnlyDictionary<string, object> customConstructorArguments = null,
            ConstructorInjectionMode injectionMode = ConstructorInjectionMode.UseCustomArgsOverResolving,
            IServiceActivator customActivator = null)
        {
            IServiceRegistrationKey serviceRegistrationKey = ServiceRegistrationKey.Create(tService, varyServiceBy);

            if (HasServiceRegistration(tService, varyServiceBy))
            {
                throw new SimilarServiceAlreadyRegisteredException();
            }

            services.TryAdd(
                serviceRegistrationKey,
                new ServiceRegistration
                {
                    RegistrationKey = serviceRegistrationKey,
                    ComponentType = tImplementation,
                    Activator = customActivator
                });
        }

        public void Remove<TService>(
            IReadOnlyDictionary<string, object> varyServiceBy)
        {
            Remove(typeof(TService), varyServiceBy);
        }

        public void Remove(
            Type tService, 
            IReadOnlyDictionary<string, object> varyServiceBy)
        {
            IServiceRegistrationKey serviceRegistrationKey = ServiceRegistrationKey.Create(tService, varyServiceBy);
            IServiceRegistration registration;

            if(!services.TryRemove(serviceRegistrationKey, out registration))
            {
                throw new RegistrationNotFoundException();
            }
            
            onRemove.Invoke(serviceRegistrationKey);
        }

        public IEnumerable<IServiceRegistration> QueryServiceRegistrations()
        {
            return services.Select(c => c.Value);
        }
    }
}