namespace CVB.NET.Abstractions.Ioc.Container.Registration
{
    using System;

    using CVB.NET.Abstractions.Ioc.Base;

    public interface IIocRegistrationContext : IIocMetaProvider, IDisposable
    {
        IRegistrationInstructionProxy RegisterService(Type tService);
        IRegistrationInstructionProxy<TService> RegisterService<TService>();
        IRegistrationInstructionProxy<TService> RegisterService<TService, TImplementation>();
        IRegistrationInstructionProxy RegisterServiceInstance(Type tService, object instance);
        IRegistrationInstructionProxy<TService> RegisterServiceInstance<TService>(TService instance) where TService : class;
    }
}