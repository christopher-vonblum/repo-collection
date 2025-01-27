namespace CVB.NET.Abstractions.Adapters.Ioc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions.Ioc.Container;
    using Abstractions.Ioc.Container.Registration;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    /*public class WindsorContainerAdapter : IocContainerAdapterBase
    {
        private readonly IWindsorContainer container;

        public WindsorContainerAdapter(IWindsorContainer container) : base(new DependencyInjectionHelper(new DependencyInjectionLambdaGenerator()))
        {
            this.container = container;
        }
        
        public override object Resolve(Type tService)
        {
            return container.Resolve(tService);
        }

        public override object Resolve(Type tService, string dependencyName)
        {
            return container.Resolve(dependencyName, tService);
        }
        
        protected override IRegistrationInstructionProxy RegisterService(Type tService, Type tImplementation, string name, BasicLifestyle lifestyle)
        {
            var proxy = new RegistrationInstructionProxy(this, tService, tImplementation, this.RegisterInstruction);
            proxy.AddExtensions(new Named(name));
            return proxy;
        }

        public override void RegisterInstance(Type tService, object instance)
        {
            container.Register(Component.For(tService).Instance(instance));
        }

        public override void RegisterInstance(Type tService, string name, object instance)
        {
            container.Register(Component.For(tService).Named(name).Instance(instance));
        }
        
        public override bool HasRegistration(Type tService)
        {
            return container.Kernel.HasComponent(tService);
        }

        public override bool HasRegistration(Type tService, string name)
        {
            return container.Kernel.HasComponent(name) && container.Kernel.GetHandler(name).ComponentModel.Implementation == tService;
        }
        
        private void RegisterInstruction(IEnumerable<IRegistrationExtension> extensions, IRegistrationInstruction reg)
        {
            var basicReg = Component.For(reg.Service).ImplementedBy(reg.Implementation);

            Named name = extensions.OfType<Named>().SingleOrDefault();

            if (name != null)
            {
                basicReg.Named(name.Name);
            }

            if (lifestyle == BasicLifestyle.Singleton)
            {
                basicReg.LifestyleSingleton();
            }
            else if (lifestyle == BasicLifestyle.Transient)
            {
                basicReg.LifestyleTransient();
            }

            container.Register(basicReg.LifestyleTransient());
        }

        public override Type ResolveType(Type tService)
        {
            return container.Kernel.GetHandler(tService).ComponentModel.Implementation;
        }

        public override Type ResolveType(Type tService, string name)
        {
            return container.Kernel.GetHandler(name).ComponentModel.Implementation;
        }
    }*/
}
