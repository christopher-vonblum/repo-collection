using System;

using CVB.NET.Abstractions.Ioc.Registration.Information;

namespace CVB.NET.Abstractions.Ioc.Registration
{
    public interface IRegistration
    {
        Type ServiceType { get; set; }
        Type ImplementationType { get; set; }

        TInformationRole GetInformation<TInformationRole>() where TInformationRole : IRegistrationInformation;
        void RegisterInformation(IRegistrationInformation information);
    }

    public interface IRegistration<TService> : IRegistration
    {
    }
}