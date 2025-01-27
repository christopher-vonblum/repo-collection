namespace CVB.NET.Abstractions.Ioc.ServiceLocator
{
    using System;
    using System.Collections.Generic;
    using Provider;

    public class IocProviderServiceLocatorAdapter : INamedServiceLocator, IServiceLocator
    {
        protected IIocProvider InnerProvider { get; private set; }

        protected IocProviderServiceLocatorAdapter()
        {
            if (this is IIocProvider)
            {
                InnerProvider = this as IIocProvider;
            }
        }

        public IocProviderServiceLocatorAdapter(IIocProvider innerProvider)
        {
            InnerProvider = innerProvider;
        }

        public bool HasInstanceRegistration<TService>() where TService : class
        {
            return HasInstanceRegistration(typeof (TService));
        }

        public bool HasInstanceRegistration(Type serviceType)
        {
            return InnerProvider.Container.HasServiceRegistration(serviceType);
        }

        public TService GetInstance<TService>() where TService : class
        {
            return (TService)GetInstance(typeof (TService));
        }

        public object GetInstance(Type serviceType)
        {
            return InnerProvider.GetInstance(serviceType);
        }

        public void Register<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            Register(typeof(TService), typeof(TImplementation));
        }

        public void Register(Type tService, Type tImplementation)
        {
            InnerProvider.Container.Register(tService, tImplementation);
        }

        public void Remove<TService>() where TService : class
        {
            Remove(typeof(TService));
        }

        public void Remove(Type tService)
        {
            InnerProvider.Container.Remove(tService);
        }

        private Dictionary<string, object> MakeNamedVariance(string name)
        {
            return new Dictionary<string, object> {{"Name", name}};
        }

        public bool HasInstanceRegistration<TService>(string name) where TService : class
        {
            return HasInstanceRegistration(typeof (TService), name);
        }

        public bool HasInstanceRegistration(Type serviceType, string name)
        {
            return InnerProvider.Container.HasServiceRegistration(serviceType, MakeNamedVariance(name));
        }

        public TService Resolve<TService>(string name) where TService : class
        {
            return (TService) Resolve(typeof (TService), name);
        }

        public object Resolve(Type serviceType, string name)
        {
            return InnerProvider.GetInstance(serviceType, MakeNamedVariance(name));
        }

        public void Register<TService, TImplementation>(string name) where TService : class where TImplementation : class, TService
        {
            Register(typeof(TService), typeof(TImplementation), name);
        }

        public void Register(Type tService, Type tImplementation, string name)
        {
            InnerProvider.Container.Register(tService, tImplementation, MakeNamedVariance(name));
        }

        public void Remove<TService>(string name) where TService : class
        {
            Remove(typeof (TService), name);
        }

        public void Remove(Type tService, string name)
        {
            InnerProvider.Container.Remove(tService, MakeNamedVariance(name));
        }
    }
}