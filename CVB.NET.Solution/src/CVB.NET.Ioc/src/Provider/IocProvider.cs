namespace CVB.NET.Ioc.Provider
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using Container;
    using Model;

    public class IocProvider : IocProviderBase
    {
        private ConcurrentDictionary<InterfaceDefinition, IocSingletonInstanceContainer> DomainSingletonInstanceCache { get; }
            = new ConcurrentDictionary<InterfaceDefinition, IocSingletonInstanceContainer>();

        private ThreadLocal<ConcurrentDictionary<InterfaceDefinition, IocSingletonInstanceContainer>> ThreadSingletonInstanceCache { get; }
            = new ThreadLocal<ConcurrentDictionary<InterfaceDefinition, IocSingletonInstanceContainer>>(() =>
                new ConcurrentDictionary<InterfaceDefinition, IocSingletonInstanceContainer>());

        public IocProvider() : base(new IocContainer())
        {
        }

        public IocProvider(IIocContainer iocContainer) : base(iocContainer)
        {
        }

        public override object GetNamedSingletonInstance(Type tInterface, string singletonKey)
        {
            IImplementationConstruction implementationConstruction =
                IocContainer.GetNamedSingletonImplementationConstruction(tInterface, singletonKey);

            switch (implementationConstruction.InstanceLifeStyle)
            {
                case InstanceLifeStyle.Default:
                case InstanceLifeStyle.InstancePerDomain:
                    return DomainSingletonInstanceCache.GetOrAdd(
                        new InterfaceDefinition(tInterface) {SingletonKey = singletonKey},
                        createInstance =>
                            new IocSingletonInstanceContainer(CreateInstance(implementationConstruction)))
                        .ContextualInstance;

                case InstanceLifeStyle.InstancePerThread:
                    return ThreadSingletonInstanceCache.Value.GetOrAdd(
                        new InterfaceDefinition(tInterface) {SingletonKey = singletonKey},
                        createInstance =>
                            new IocSingletonInstanceContainer(CreateInstance(implementationConstruction)))
                        .ContextualInstance;
                default:
                    throw new NotImplementedException($"InstanceLifeStyle.{implementationConstruction.InstanceLifeStyle}");
            }
        }

        public override void PushNamedContextSingletonInstance(Type tInterface, string singletonKey, object instance)
        {
            DomainSingletonInstanceCache[new InterfaceDefinition(tInterface) {SingletonKey = singletonKey}].ThreadContextStack.Push(instance);

            IImplementationConstruction implementationConstruction =
                IocContainer.GetNamedSingletonImplementationConstruction(tInterface, singletonKey);

            switch (implementationConstruction.InstanceLifeStyle)
            {
                case InstanceLifeStyle.Default:
                case InstanceLifeStyle.InstancePerDomain:
                    DomainSingletonInstanceCache[new InterfaceDefinition(tInterface) {SingletonKey = singletonKey}].ThreadContextStack.Push(instance);
                    break;

                case InstanceLifeStyle.InstancePerThread:
                    ThreadSingletonInstanceCache.Value[new InterfaceDefinition(tInterface) {SingletonKey = singletonKey}].ThreadContextStack.Push(instance);
                    break;

                default:
                    throw new NotImplementedException($"InstanceLifeStyle.{implementationConstruction.InstanceLifeStyle}");
            }
        }

        public override object PopNamedSingletonInstance(Type tInterface, string singletonKey)
        {
            IImplementationConstruction implementationConstruction =
                IocContainer.GetNamedSingletonImplementationConstruction(tInterface, singletonKey);

            switch (implementationConstruction.InstanceLifeStyle)
            {
                case InstanceLifeStyle.Default:
                case InstanceLifeStyle.InstancePerDomain:
                    return DomainSingletonInstanceCache[new InterfaceDefinition(tInterface) {SingletonKey = singletonKey}].ThreadContextStack.Pop();

                case InstanceLifeStyle.InstancePerThread:
                    return ThreadSingletonInstanceCache.Value[new InterfaceDefinition(tInterface) {SingletonKey = singletonKey}].ThreadContextStack.Pop();

                default:
                    throw new NotImplementedException($"InstanceLifeStyle.{implementationConstruction.InstanceLifeStyle}");
            }
        }
    }
}