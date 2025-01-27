namespace CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base
{
    using System;
    using System.Collections.Generic;

    public interface IRegistrationExtension
    {
        Func<object> Constructor { get; }

        IReadOnlyDictionary<string, string> InstanceModifiers { get; }
        void PreConstruction(IRegistrationInstruction instruction);
        void InitializeInstance(IRegistrationInstruction instruction, object instance);
        void Resolve(IRegistrationInstruction instruction, object instance);
    }
}