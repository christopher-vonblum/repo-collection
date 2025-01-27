namespace CVB.NET.Abstractions.Ioc.Provider
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Container;
    using Container.Model;
    using Model;
    using ServiceLocator;

    public class IocProvider : IocProviderServiceLocatorAdapter, IIocProvider
    {
        IReadOnlyIocContainer IReadOnlyIocProvider.Container => Container;

        public IIocContainer Container { get; }

        private readonly ConcurrentDictionary<IServiceInstanceKey, IServiceInstance> serviceInstances = new ConcurrentDictionary<IServiceInstanceKey, IServiceInstance>();

        public IocProvider(IIocContainer innerContainer)
        {
            Container = innerContainer;
            Container.OnRemove += ContainerOnRemove;
        }

        private void ContainerOnRemove(IServiceRegistrationKey serviceRegistrationKey)
        {
            var release = QueryServiceInstances().Where(i => i.Key.Key.Equals(serviceRegistrationKey)).Select(i => i.Key).ToList();

            foreach (IServiceInstanceKey instanceKey in release)
            {
                IServiceInstance instance;
                serviceInstances.TryRemove(instanceKey, out instance);
            }
        }

        public TService GetInstance<TService>(
            IReadOnlyDictionary<string, object> varyServiceBy = null, 
            IReadOnlyDictionary<string, object> varyInstanceBy = null) where TService : class
        {
            return (TService) GetInstance(typeof (TService), varyServiceBy, varyInstanceBy);
        }

        public object GetInstance(
            Type tService, 
            IReadOnlyDictionary<string, object> varyServiceBy = null, 
            IReadOnlyDictionary<string, object> varyInstanceBy = null)
        {
            IServiceRegistrationKey serviceRegistrationKey = ServiceRegistrationKey.Create(tService, varyServiceBy);

            IServiceInstanceKey serviceInstanceKey = ServiceInstanceKey.Create(serviceRegistrationKey, varyInstanceBy);

            return GetInstance(serviceInstanceKey);
        }

        public IServiceInstance GetInstance(IServiceInstanceKey serviceInstanceKey)
        {
            return serviceInstances
                .GetOrAdd(
                    serviceInstanceKey,
                    (key) => ServiceInstance.Create(key, Container.CreateInstance(key.Key.Service, key.Key.VaryBy)));
        }

        public IEnumerable<IServiceInstance> QueryServiceInstances()
        {
            return serviceInstances.Values;
        }
    }
}