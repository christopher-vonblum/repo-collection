using Unity;
using Unity.Lifetime;

namespace CVB.NET.Abstractions.Adapters.Ioc
{
    using System;
    using System.Linq;
    using Abstractions.Ioc.Container;

    using CVB.NET.Abstractions.Ioc.Container.Base;
    using CVB.NET.Abstractions.Ioc.Container.Registration.Extension;
    using CVB.NET.Abstractions.Ioc.Injection;
    using CVB.NET.Abstractions.Ioc.Injection.Lambda;
    
    
    public class UnityContainerAdapter : IocContainerAdapterBase
    {
        private readonly IUnityContainer unityContainer;

        public UnityContainerAdapter(IUnityContainer unityContainer) : base(new DependencyInjectionHelper(new DependencyInjectionLambdaGenerator()))
        {
            this.unityContainer = unityContainer;
        }

        private LifetimeManager GetLifetimeManagerForBasicLifestyle(BasicLifestyle lifestyle)
        {
            switch (lifestyle)
            {
                case BasicLifestyle.Singleton:
                    return new ContainerControlledLifetimeManager();
                case BasicLifestyle.Transient:
                    return new TransientLifetimeManager();
            }

            throw new InvalidOperationException();
        }

        public override void RegisterServiceInternal(Type tService, Type tImplementation, string serviceKey = null)
        {
            unityContainer.RegisterType(tService, tImplementation, serviceKey, new ContainerControlledLifetimeManager());
        }

        public override void RegisterServiceInstanceInternal(Type tService, object instance, string serviceKey = null)
        {
            unityContainer.RegisterInstance(tService, serviceKey, instance, new ContainerControlledLifetimeManager());
        }

        public override Type ResolveImplementationTypeInternal(Type tService, string serviceKey = null)
        {
            return unityContainer.Registrations.Single(reg => reg.Name == serviceKey && reg.RegisteredType == tService).MappedToType;
        }

        public override bool HasServiceRegistrationInternal(Type tService, string serviceKey = null)
        {
            return unityContainer.Registrations.FirstOrDefault(r => r.RegisteredType == tService && r.Name == serviceKey) != null;
        }

        public override object ResolveServiceInternal(Type tService, string serviceKey = null)
        {
            return unityContainer.Resolve(tService, serviceKey);
        }

    }
}