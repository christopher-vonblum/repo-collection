using System.Linq;
using CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base;

namespace CVB.NET.Abstractions.Ioc.Container.Base
{
    using System;
    using System.Collections.Generic;

    using CVB.NET.Abstractions.Ioc.Base;
    using CVB.NET.Abstractions.Ioc.Container.Context;
    using CVB.NET.Abstractions.Ioc.Container.Registration;
    using CVB.NET.Abstractions.Ioc.Injection;
    using CVB.NET.Abstractions.Ioc.Injection.Lambda;

    public abstract class IocContainerAdapterBase : InjectionProviderBase, IIocContainer, IIocContainerAdapter
    {
        protected DictionaryExecutionContext<string, string> ResolverDomainEnvironment { get; } = new DictionaryExecutionContext<string, string>();

        protected DictionaryExecutionContext<string, string> ResolverComponentEnvironment { get; } = new DictionaryExecutionContext<string, string>();

        public IocContainerAdapterBase(IDependencyInjectionHelper injectionHelper) : base(injectionHelper, null)
        {
        }

        public IocContainerAdapterBase() : base(new DependencyInjectionHelper(new DependencyInjectionLambdaGenerator()), null)
        {
        }
        
        public void Register(Action<RegistrationContext> register)
        {
            using (var registrationContext = new RegistrationContext(this))
            {
                register(registrationContext);
            }
        }

        public override bool HasServiceRegistration(Type tService, IReadOnlyDictionary<string, string> keys)
        {
            using (this.ResolverDomainEnvironment.EnvironmentOverride(keys))
            {
                return this.HasServiceRegistrationInternal(tService, GetInstanceKey(tService, this.ResolverDomainEnvironment.CurrentEnvironment));
            }
        }
        public override Type ResolveImplementationType(Type tService, IReadOnlyDictionary<string, string> keys)
        {
            using (this.ResolverDomainEnvironment.EnvironmentOverride(keys))
            {
                return this.ResolveImplementationTypeInternal(tService, GetInstanceKey(tService, this.ResolverDomainEnvironment.CurrentEnvironment));
            }
        }
        public string GetInstanceKey(Type tService, IReadOnlyDictionary<string, string> keys)
        {
            var serviceKey = this.GetServiceIdentifier(keys);

            var instanceKey = serviceKey;

            return tService.AssemblyQualifiedName + "=>" + serviceKey;
        }

        private string GetServiceIdentifier(IReadOnlyDictionary<string, string> keys)
        {
            return string.Join("#-#", keys.OrderBy(e => e.Key == "name").ThenBy(e => e.Key).Select(kv => kv.Key + "|" + kv.Value ?? string.Empty));
        }
        private IReadOnlyDictionary<string, string> GetInstanceIdentifier(IRegistrationExtension[] extensions)
        {
            return ResolverComponentEnvironment.CurrentEnvironment.Concat(extensions.SelectMany(e => e.InstanceModifiers)).ToDictionary(k => k.Key, v => v.Value);
        }

        public object ResolveService(Type tService)
        {
            return this.ResolveService(tService, (string)null);
        }

        public object ResolveService(Type tService, string name)
        {
            return this.ResolveService(tService, new Dictionary<string, string> { { nameof(name), name } });
        }

        public object ResolveService(Type tService, IReadOnlyDictionary<string, string> keys)
        {
            using (this.ResolverDomainEnvironment.EnvironmentOverride(keys))
            {
                return this.ResolveServiceInternal(tService, GetInstanceKey(tService, this.ResolverDomainEnvironment.CurrentEnvironment));
            }
        }
        
        public TService ResolveService<TService>()
            where TService : class
        {
            return (TService)this.ResolveService(typeof(TService));
        }

        public TService ResolveService<TService>(string name)
            where TService : class
        {
            return (TService)this.ResolveService(typeof(TService), name);
        }

        public TService ResolveService<TService>(IReadOnlyDictionary<string, string> keys)
            where TService : class
        {
            return (TService)this.ResolveService(typeof(TService), keys);
        }

        public abstract void RegisterServiceInternal(Type tService, Type tImplementation, string serviceKey = null);
        public abstract void RegisterServiceInstanceInternal(Type tService, object instance, string serviceKey = null);
        public abstract Type ResolveImplementationTypeInternal(Type tService, string serviceKey = null);
        public abstract bool HasServiceRegistrationInternal(Type tService, string serviceKey = null);
        public abstract object ResolveServiceInternal(Type tService, string serviceKey = null);
    }
}
