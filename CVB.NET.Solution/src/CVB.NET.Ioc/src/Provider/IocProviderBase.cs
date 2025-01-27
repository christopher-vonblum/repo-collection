namespace CVB.NET.Ioc.Provider
{
    using System;
    using Container;
    using Model;
    using PostSharp.Patterns.Contracts;

    public abstract class IocProviderBase : IIocProvider
    {
        protected IocProviderBase([NotNull] IReadOnlyIocContainer iocContainer)
        {
            IocContainer = iocContainer;
        }

        public IReadOnlyIocContainer IocContainer { get; protected set; }

        public object CreateInstance(Type tInterface)
        {
            return CreateInstance(IocContainer.GetDefaultImplementationConstruction(tInterface));
        }

        public TInterface CreateInstance<TInterface>() where TInterface : class
        {
            return (TInterface) CreateInstance(typeof (TInterface));
        }

        public object GetDefaultSingletonInstance(Type tInterface)
        {
            return GetNamedSingletonInstance(tInterface, string.Empty);
        }

        public TInterface GetDefaultSingletonInstance<TInterface>() where TInterface : class
        {
            return (TInterface) GetDefaultSingletonInstance(typeof (TInterface));
        }

        public abstract object GetNamedSingletonInstance(Type tInterface, string singletonKey);

        public TInterface GetNamedSingletonInstance<TInterface>(string singletonKey) where TInterface : class
        {
            return (TInterface) GetNamedSingletonInstance(typeof (TInterface), singletonKey);
        }

        public void PushDefaultContextSingletonInstance(Type tInterface, object instance)
        {
            PushNamedContextSingletonInstance(tInterface, string.Empty, instance);
        }

        public void PushDefaultContextSingletonInstance<TInterface>(TInterface instance) where TInterface : class
        {
            PushDefaultContextSingletonInstance(typeof (TInterface), instance);
        }

        public object PopDefaultContextSingletonInstance(Type tInterface)
        {
            return PopNamedSingletonInstance(tInterface, string.Empty);
        }

        public TInterface PopDefaultContextSingletonInstance<TInterface>() where TInterface : class
        {
            return (TInterface) PopDefaultContextSingletonInstance(typeof (TInterface));
        }

        public abstract void PushNamedContextSingletonInstance(Type tInterface, string singletonKey, object instance);

        public void PushNamedContextSingletonInstance<TInterface>(string singletonKey, TInterface instance) where TInterface : class
        {
            PushNamedContextSingletonInstance(typeof (TInterface), singletonKey, instance);
        }

        public abstract object PopNamedSingletonInstance(Type tInterface, string singletonKey);

        public TInterface PopContextSingletonInstance<TInterface>(string singletonKey) where TInterface : class
        {
            return (TInterface) PopNamedSingletonInstance(typeof (TInterface), singletonKey);
        }

        protected virtual object CreateInstance([NotNull] IImplementationConstruction implementationConstruction)
        {
            return implementationConstruction.CreateInstance();
        }
    }
}