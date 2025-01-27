namespace CVB.NET.Abstractions.Ioc.Container.Model
{
    using System;
    using Container.Activator;

    public interface IServiceRegistration
    {
        IServiceRegistrationKey RegistrationKey { get; }
        IServiceActivator Activator { get; }
        Type ComponentType { get; }
    }
}