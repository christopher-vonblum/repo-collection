namespace CVB.NET.Abstractions.Ioc.Container.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base;

    using Reflection.Caching.Cached;

    public class RegistrationInstructionProxy : RegistrationInstruction, IRegistrationInstructionProxy
    {
        public override CachedType Implementation
        {
            get => GetImplementation?.Invoke();
            set
            {
                this.GetImplementation = () => value;
            }
        }

        public override string ServiceKey => GetInstanceKey();
        public override IReadOnlyDictionary<string, string> ServiceModifiers => GetServiceIdentifier();

        private List<IRegistrationExtension> extensions = new List<IRegistrationExtension>();
        public virtual IRegistrationExtension[] Extensions => extensions.ToArray();
        
        public Func<CachedType> GetImplementation { get; set; }

        private Action<RegistrationInstructionProxy> RegisterInstruction;

        protected RegistrationInstructionProxy()
        {
        }

        public RegistrationInstructionProxy(IIocRegistrationContext context, Type tService, IReadOnlyDictionary<string, string> registrationEnvironment, Action<RegistrationInstructionProxy> registerInstruction)
        {
            Service = tService;
            EnvironmentModifiers = registrationEnvironment;
            RegisterInstruction = registerInstruction;
        }
        
        public virtual void AddExtensions(params IRegistrationExtension[] extensions)
        {
            this.extensions.AddRange(extensions);
        }
        public string GetInstanceKey()
        {
            var key = GetInstanceIdentifier(GetServiceIdentifier());

            return Service.InnerReflectionInfo.AssemblyQualifiedName + "=>" + key;
        }

        private string GetInstanceIdentifier(IReadOnlyDictionary<string, string> keys)
        {
            return string.Join("#-#", keys.OrderBy(e => e.Key == "name").ThenBy(e => e.Key).Select(kv => kv.Key + "|" + kv.Value ?? string.Empty));
        }
        private IReadOnlyDictionary<string, string> GetServiceIdentifier()
        {
            return EnvironmentModifiers.Concat(Extensions.SelectMany(e => e.InstanceModifiers)).ToDictionary(k => k.Key, v => v.Value);
        }

        public void Dispose()
        {
            if (Implementation == null)
            {
                Implementation = Service;
            }

            RegisterInstruction(this);
        }
    }

    public class RegistrationInstructionProxy<TService> : RegistrationInstructionProxy, IRegistrationInstructionProxy<TService>
    {
        public override IReadOnlyDictionary<string, string> EnvironmentModifiers => this.Proxy.EnvironmentModifiers;

        public override CachedType Service => this.Proxy.Service;
        public override CachedType Implementation
        {
            get => this.Proxy.GetImplementation();
            set => this.Proxy.GetImplementation = () => value;
        }

        public override IRegistrationExtension[] Extensions => this.Proxy.Extensions;

        public override IReadOnlyDictionary<string, string> ServiceModifiers => this.Proxy.ServiceModifiers;

        private IRegistrationInstructionProxy Proxy;
        public RegistrationInstructionProxy(IRegistrationInstructionProxy proxy)
        {
            this.Proxy = proxy;
        }

        public override void AddExtensions(params IRegistrationExtension[] extensions)
        {
            this.Proxy.AddExtensions(extensions);
        }
    }
}