using System;

using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Base;
using CVB.NET.Abstractions.Ioc.Registration;
using CVB.NET.Abstractions.Ioc.Registration.Information.Instance;
using CVB.NET.Abstractions.Ioc.Registration.Information.Lifestyle;
using CVB.NET.Abstractions.Ioc.Registration.Information.Name;
using Unity;
using Unity.Lifetime;


namespace CVB.NET.Abstractions.Adapters.Ioc
{
    public class UnityDependencyService : IDependencyService, IDependencyInjectionProvider
    {
        private readonly IUnityContainer unityContainer;
        public UnityDependencyService(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void Register(IRegistration registration)
        {
            IRegistrationLifestyleInformation lifestyleInformation = registration.GetInformation<IRegistrationLifestyleInformation>();

            LifetimeManager lifetimeManager = null;

            InstanceInformation instanceInformation = registration.GetInformation<InstanceInformation>();

            if (lifestyleInformation is SingletonLifestyle)
            {
                lifetimeManager = new ContainerControlledLifetimeManager();
            }
            else if (lifestyleInformation is TransientLifestyle)
            {
                lifetimeManager = new TransientLifetimeManager();
            }

            NameInformation nameInformation = registration.GetInformation<NameInformation>();

            string name = nameInformation?.Name;

            if (instanceInformation == null)
            {
                if (lifetimeManager == null)
                {
                    if (name == null) this.unityContainer.RegisterType(registration.ServiceType, registration.ImplementationType);
                    else this.unityContainer.RegisterType(registration.ServiceType, registration.ImplementationType, name);
                }
                else
                {
                    if (name == null) this.unityContainer.RegisterType(registration.ServiceType, registration.ImplementationType, lifetimeManager);
                    else this.unityContainer.RegisterType(registration.ServiceType, registration.ImplementationType, name, lifetimeManager);
                }
                
            }
            else
            {
                if (lifetimeManager == null)
                {
                    if (name == null) this.unityContainer.RegisterInstance(registration.ServiceType, instanceInformation.Instance);
                    else this.unityContainer.RegisterInstance(registration.ServiceType, name, instanceInformation.Instance);
                }
                else
                {
                    if (name == null) this.unityContainer.RegisterInstance(registration.ServiceType, instanceInformation.Instance, lifetimeManager);
                    else this.unityContainer.RegisterInstance(registration.ServiceType, name, instanceInformation.Instance, lifetimeManager);
                }
            }
        }

        public void Install<TDependencySetup>()
            where TDependencySetup : IDependencySetup, new()
        {
            new TDependencySetup().InstallInto(this);
        }

        public void Install(IDependencySetup setup)
        {
            setup.InstallInto(this);
        }

        public TService Resolve<TService>()
            where TService : class
        {
            return this.unityContainer.Resolve<TService>();
        }

        public TService Resolve<TService>(string name)
            where TService : class
        {
            return this.unityContainer.Resolve<TService>(name);
        }

        public object Resolve(Type tService)
        {
            return this.unityContainer.Resolve(tService);
        }

        public object Resolve(Type tService, string name)
        {
            if (name == null)
            {
                return Resolve(tService);
            }

            return this.unityContainer.Resolve(tService, name);
        }

        public bool HasRegistration<TService>()
            where TService : class
        {
            return this.unityContainer.IsRegistered<TService>();
        }

        public bool HasRegistration<TService>(string name)
            where TService : class
        {
            return this.unityContainer.IsRegistered<TService>(name);
        }

        public bool HasRegistration(Type tService)
        {
            return this.unityContainer.IsRegistered(tService);
        }

        public bool HasRegistration(Type tService, string name)
        {
            if (name == null)
            {
                return HasRegistration(tService);
            }

            return this.unityContainer.IsRegistered(tService, name);
        }

        public void Teardown()
        {
            this.unityContainer.Dispose();
        }

        public void InjectDependencies(object target)
        {
            this.unityContainer.BuildUp(target.GetType(), target);
        }
    }
}