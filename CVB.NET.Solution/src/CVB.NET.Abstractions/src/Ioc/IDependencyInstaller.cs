using System;

using CVB.NET.Abstractions.Ioc.Registration;

namespace CVB.NET.Abstractions.Ioc
{
    public interface IDependencyInstaller
    {
        void Register(IRegistration registration);

        void Install<TDependencySetup>() where TDependencySetup : IDependencySetup, new();

        void Install(IDependencySetup setup);
    }
}