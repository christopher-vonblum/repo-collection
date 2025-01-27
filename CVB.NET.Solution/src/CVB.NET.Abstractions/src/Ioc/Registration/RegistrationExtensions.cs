using CVB.NET.Abstractions.Ioc.Registration.Information.Instance;
using CVB.NET.Abstractions.Ioc.Registration.Information.Lifestyle;
using CVB.NET.Abstractions.Ioc.Registration.Information.Name;

namespace CVB.NET.Abstractions.Ioc.Registration
{
    public static class RegistrationExtensions
    {
        public static IRegistration Transient(this IRegistration registration)
        {
            registration.RegisterInformation(new TransientLifestyle());
            return registration;
        }
        public static IRegistration Singleton(this IRegistration registration)
        {
            registration.RegisterInformation(new SingletonLifestyle());
            return registration;
        }
        public static IRegistration<TService> Instance<TService>(this IRegistration<TService> registration, TService instance)
        {
            registration.RegisterInformation(new InstanceInformation(instance));
            return registration;
        }
        public static IRegistration Named(this IRegistration registration, string name)
        {
            registration.RegisterInformation(new NameInformation(name));
            return registration;
        }
    }
}