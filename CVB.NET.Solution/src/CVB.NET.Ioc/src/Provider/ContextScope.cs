namespace CVB.NET.Ioc.Provider
{
    using System;
    using System.Linq;
    using Aspects;
    using Exception;
    using Model;
    using PostSharp.Patterns.Contracts;
    using Reflection.Aspects.ParameterValidation;
    using Reflection.Caching.Cached;

    public sealed class ContextScope<TOverrideInterface> : IDisposable
        where TOverrideInterface : class
    {
        private IocAccessorAspect IocAccessorAspect { get; }

        private CachedType StaticContextType { get; }

        private InterfaceDefinition InterfaceDefinition { get; }

        public ContextScope(
            [IsStatic] Type staticContext,
            [NotNull] TOverrideInterface contextValue,
            [NotNull] string overrideSingletonKey = "")
        {
            InterfaceDefinition = new InterfaceDefinition(typeof (TOverrideInterface)) {SingletonKey = overrideSingletonKey};

            StaticContextType = staticContext;

            IocAccessorAspect = StaticContextType.Attributes.OfType<IocAccessorAspect>().SingleOrDefault();

            if (IocAccessorAspect == null)
            {
                throw new CanNotApplyContextScopeToNonIocAccessorContext(StaticContextType.InnerReflectionInfo);
            }

            if (InterfaceDefinition.IsDefaultDefinition)
            {
                IocAccessorAspect.IocProvider.PushDefaultContextSingletonInstance(InterfaceDefinition.InterfaceType, contextValue);
                return;
            }

            IocAccessorAspect.IocProvider.PushNamedContextSingletonInstance(InterfaceDefinition.InterfaceType, InterfaceDefinition.SingletonKey, contextValue);
        }

        public void Dispose()
        {
            if (InterfaceDefinition.IsDefaultDefinition)
            {
                IocAccessorAspect.IocProvider.PopDefaultContextSingletonInstance(InterfaceDefinition.InterfaceType);
                return;
            }

            IocAccessorAspect.IocProvider.PopNamedSingletonInstance(InterfaceDefinition.InterfaceType, InterfaceDefinition.SingletonKey);
        }
    }
}