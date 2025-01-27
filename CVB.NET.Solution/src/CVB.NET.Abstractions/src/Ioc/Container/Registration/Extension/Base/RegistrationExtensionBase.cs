namespace CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base
{
    using System;
    using System.Collections.Generic;

    public class RegistrationExtensionBase : IRegistrationExtension
    {
        public virtual Func<object> Constructor => null;
        public IReadOnlyDictionary<string, string> InstanceModifiers => this.GetInstanceModifiers();

        public virtual IReadOnlyDictionary<string, string> GetInstanceModifiers()
        {
            return new Dictionary<string, string>();
        }

        public virtual void PreConstruction(IRegistrationInstruction instruction)
        {
        }

        public virtual void InitializeInstance(IRegistrationInstruction instruction, object instance)
        {
        }

        public virtual void Resolve(IRegistrationInstruction instruction, object instance)
        {
        }
    }
}