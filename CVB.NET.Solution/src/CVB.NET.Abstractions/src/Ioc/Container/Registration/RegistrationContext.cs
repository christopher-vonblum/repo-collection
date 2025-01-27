using CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base;

namespace CVB.NET.Abstractions.Ioc.Container.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CVB.NET.Abstractions.Ioc.Base;
    using CVB.NET.Abstractions.Ioc.Container.Base;
    using CVB.NET.Abstractions.Ioc.Container.Context;

    public class RegistrationContext : IocMetaProviderBase, IIocRegistrationContext
    {
        protected DictionaryExecutionContext<string, string> RegistrationEnvironment { get; } = new DictionaryExecutionContext<string, string>();
        private List<IRegistrationInstructionProxy> registrations = new List<IRegistrationInstructionProxy>();

        private IIocContainerAdapter adapter;

        public RegistrationContext(IIocContainerAdapter adapter)
        {
            this.adapter = adapter;
        }

        private IRegistrationInstructionProxy TrackServiceRegistration(Type tService, IReadOnlyDictionary<string, string> keys = null)
        {
            return this.TrackRegistration(tService, this.MakeServiceRegistrationInstruction, keys);
        }

        private IRegistrationInstructionProxy TrackServiceInstanceRegistration(Type tService, object instance, IReadOnlyDictionary<string, string> keys = null)
        {
            return this.TrackRegistration(tService, t => this.MakeServiceInstanceRegistrationInstruction(t, instance), keys);
        }

        private IRegistrationInstructionProxy TrackRegistration(Type tService, Func<Type, IRegistrationInstructionProxy> makeInstruction, IReadOnlyDictionary<string, string> keys = null)
        {
            if (keys == null)
            {
                var inst = makeInstruction(tService);

                this.registrations.Add(inst);

                return inst;
            }

            using (this.RegistrationEnvironment.EnvironmentOverride(keys))
            {
                var inst = makeInstruction(tService);

                this.registrations.Add(inst);

                return inst;
            }
        }

        private IRegistrationInstructionProxy MakeServiceRegistrationInstruction(Type tService)
        {
            return new RegistrationInstructionProxy(
                this,
                tService,
                this.RegistrationEnvironment.CurrentEnvironment,
                (r) =>
                    {
                        using (this.RegistrationEnvironment.EnvironmentOverride(r.ServiceModifiers))
                        {
                            this.adapter.RegisterServiceInternal(tService, r.Implementation, r.GetInstanceKey());
                        }
                    });
        }

        private IRegistrationInstructionProxy MakeServiceInstanceRegistrationInstruction(Type tService, object instance)
        {
            var proxy = new RegistrationInstructionProxy(
                this,
                tService,
                this.RegistrationEnvironment.CurrentEnvironment,
                (r) =>
                    {
                        using (this.RegistrationEnvironment.EnvironmentOverride(r.ServiceModifiers))
                        {
                            this.adapter.RegisterServiceInstanceInternal(tService, instance, r.GetInstanceKey());
                        }
                    });

            proxy.Implementation = instance.GetType();

            return proxy;
        }

        public void Dispose()
        {
            this.registrations.ForEach(reg => reg.Dispose());
        }

        public IRegistrationInstructionProxy RegisterService(Type tService)
        {
            return this.TrackServiceRegistration(tService);
        }

        public IRegistrationInstructionProxy RegisterServiceInstance(Type tService, object instance)
        {
            return this.TrackServiceInstanceRegistration(tService, instance);
        }

        public IRegistrationInstructionProxy<TService> RegisterService<TService>()
        {
            return this.RegisterService(typeof(TService)).Cast<TService>();
        }

        public IRegistrationInstructionProxy<TService> RegisterService<TService, TImplementation>()
        {
            var registration = this.TrackServiceRegistration(typeof(TService));
            registration.GetImplementation = () => typeof(TImplementation);
            return registration.Cast<TService>();
        }

        public IRegistrationInstructionProxy<TService> RegisterServiceInstance<TService>(TService instance)
            where TService : class
        {
            return this.RegisterServiceInstance(typeof(TService), instance).Cast<TService>();
        }

        public override bool HasServiceRegistration(Type tService, IReadOnlyDictionary<string, string> keys)
        {
            using (this.RegistrationEnvironment.EnvironmentOverride(keys))
            {
                string instanceKey = GetInstanceKey(tService, this.RegistrationEnvironment.CurrentEnvironment);
                return this.registrations.FirstOrDefault(reg => reg.Service == tService && reg.ServiceKey == instanceKey) != null || this.adapter.HasServiceRegistrationInternal(tService, instanceKey);
            }
        }

        public override Type ResolveImplementationType(Type tService, IReadOnlyDictionary<string, string> keys)
        {
            using (this.RegistrationEnvironment.EnvironmentOverride(keys))
            {
                string instanceKey = GetInstanceKey(tService, this.RegistrationEnvironment.CurrentEnvironment);
                var registration = this.registrations.FirstOrDefault(reg => reg.Service == tService && reg.ServiceKey == instanceKey);
                if(registration == null) return this.adapter.ResolveImplementationTypeInternal(tService, instanceKey);
                return registration.GetImplementation();
            }
        }

        public string GetInstanceKey(Type tService, IReadOnlyDictionary<string, string> keys)
        {
            var key = GetInstanceIdentifier(GetServiceIdentifier(this.registrations.Where(r => r.Service == tService).SelectMany(r => r.Extensions).Aggregate(new Dictionary<Type, IRegistrationExtension>(), (list, extension) => { list[extension.GetType()] = extension;
                                                                                                                                                                                                                             return list;
                                                                                                                                                                                                                         } ).Select(kv => kv.Value).ToArray()));

            return tService.AssemblyQualifiedName + "=>" + key;
        }

        private string GetInstanceIdentifier(IReadOnlyDictionary<string, string> keys)
        {
            return string.Join("#-#", keys.OrderBy(e => e.Key == "name").ThenBy(e => e.Key).Select(kv => kv.Key + "|" + kv.Value ?? string.Empty));
        }
        private IReadOnlyDictionary<string, string> GetServiceIdentifier(IRegistrationExtension[] extensions)
        {
            return RegistrationEnvironment.CurrentEnvironment.Concat(extensions.SelectMany(e => e.InstanceModifiers)).ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
