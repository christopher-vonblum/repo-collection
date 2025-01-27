using System;

using CVB.NET.Abstractions.Ioc.Registration.Information.Instance;

namespace CVB.NET.Abstractions.Ioc.Registration
{
    public static class Service
    {
        public static IRegistration For(Type tService)
        {
            IRegistration reg = new Registration();
            reg.ServiceType = tService;
            reg.ImplementationType = tService;
            return reg;
        }
        public static IRegistration For(Type tService, Type tImplementation)
        {
            IRegistration reg = new Registration();
            reg.ServiceType = tService;
            reg.ImplementationType = tImplementation;
            return reg;
        }
        public static IRegistration For(Type tService, object instance)
        {
            IRegistration reg = new Registration();
            reg.ServiceType = tService;
            reg.ImplementationType = instance.GetType();
            reg.RegisterInformation(new InstanceInformation(instance));
            return reg;
        }
        public static IRegistration<TService> For<TService>()
        {
            IRegistration<TService> reg = new Registration<TService>();
            reg.ServiceType = typeof(TService);
            reg.ImplementationType = typeof(TService);
            return reg;
        }

        public static IRegistration<TService> For<TService>(TService instance)
        {
            IRegistration<TService> reg = new Registration<TService>();
            reg.ServiceType = typeof(TService);
            reg.ImplementationType = instance.GetType();
            reg.Instance(instance);
            return reg;
        }

        public static IRegistration<TService> For<TService, TImplementation>() where TImplementation : TService
        {
            IRegistration<TService> reg = new Registration<TService>();
            reg.ServiceType = typeof(TService);
            reg.ImplementationType = typeof(TImplementation);
            return reg;
        }
    }
}