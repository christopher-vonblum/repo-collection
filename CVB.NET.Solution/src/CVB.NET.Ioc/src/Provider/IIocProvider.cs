namespace CVB.NET.Ioc.Provider
{
    using System;
    using Container;
    using PostSharp.Patterns.Contracts;

    public interface IIocProvider
    {
        IReadOnlyIocContainer IocContainer { get; }

        object CreateInstance([NotNull] Type tInterface);
        TInterface CreateInstance<TInterface>() where TInterface : class;

        object GetDefaultSingletonInstance([NotNull] Type tInterface);

        TInterface GetDefaultSingletonInstance<TInterface>()
            where TInterface : class;

        object GetNamedSingletonInstance([NotNull] Type tInterface, [NotEmpty] string singletonKey);

        TInterface GetNamedSingletonInstance<TInterface>([NotEmpty] string singletonKey)
            where TInterface : class;

        void PushDefaultContextSingletonInstance([NotNull] Type tInterface, [NotNull] object instance);

        void PushDefaultContextSingletonInstance<TInterface>([NotNull] TInterface instance)
            where TInterface : class;

        object PopDefaultContextSingletonInstance([NotNull] Type tInterface);

        TInterface PopDefaultContextSingletonInstance<TInterface>()
            where TInterface : class;

        void PushNamedContextSingletonInstance([NotNull] Type tInterface, [NotEmpty] string singletonKey, [NotNull] object instance);

        void PushNamedContextSingletonInstance<TInterface>([NotEmpty] string singletonKey, [NotNull] TInterface instance)
            where TInterface : class;

        object PopNamedSingletonInstance([NotNull] Type tInterface, [NotEmpty] string singletonKey);

        TInterface PopContextSingletonInstance<TInterface>([NotEmpty] string singletonKey)
            where TInterface : class;
    }
}