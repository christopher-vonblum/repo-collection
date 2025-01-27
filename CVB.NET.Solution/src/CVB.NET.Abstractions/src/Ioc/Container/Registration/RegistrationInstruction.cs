namespace CVB.NET.Abstractions.Ioc.Container.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Reflection.Caching.Cached;
    public class RegistrationInstruction : IRegistrationInstruction
    {
        public virtual CachedType Service { get; protected set; }

        private CachedType implementationType;

        public virtual CachedType Implementation
        {
            get => this.implementationType;
            set => this.implementationType = value;
        }

        public virtual IReadOnlyDictionary<string, string> EnvironmentModifiers { get; protected set; }
        public virtual IReadOnlyDictionary<string, string> ServiceModifiers { get; protected set; }
        public virtual string ServiceKey { get; protected set; }

        protected RegistrationInstruction()
        {
        }
        
        public RegistrationInstruction(IRegistrationInstructionProxy registrationInstructionProxy)
        {
            Service = registrationInstructionProxy.Service;
            EnvironmentModifiers = registrationInstructionProxy.EnvironmentModifiers;
            ServiceModifiers = registrationInstructionProxy.ServiceModifiers;
            ServiceKey = registrationInstructionProxy.ServiceKey;
        }
    }
}