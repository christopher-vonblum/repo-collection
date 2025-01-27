using System;

namespace CVB.NET.Abstractions.Ioc.Registration.Information
{
    public abstract class RegistrationInformationBase<TInformationRole> : IRegistrationInformation
    {
        public Type ExtensionRoleInterface => typeof(TInformationRole);
    }
}