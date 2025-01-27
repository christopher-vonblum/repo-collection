namespace CVB.NET.Abstractions.Ioc.Injection
{
    using System;
    using System.Linq;

    using CVB.NET.Abstractions.Ioc.Base;
    using CVB.NET.Abstractions.Ioc.Container.Base;
    using CVB.NET.Abstractions.Ioc.Injection.Attribute;
    using CVB.NET.Reflection.Caching.Cached;

    public abstract class InjectionProviderBase : IocMetaProviderBase, IIocInjectionProvider
    {
        private IIocContainer container;
        private static readonly Func<CachedPropertyInfo, bool> defaultPropertySelector = prop => prop.InheritedAttributes.OfType<DependencyInjectionTargetAttribute>().Any();
        private static readonly Func<CachedType, CachedConstructorInfo> defaultConstructorSelector = type => type.Constructors.Single(c => c.InheritedAttributes.OfType<DependencyInjectionTargetAttribute>().Any());
        private static readonly Func<CachedMethodInfo, bool> defaultMethodSelector = prop => prop.InheritedAttributes.OfType<DependencyInjectionTargetAttribute>().Any();
        private static readonly Func<IIocContainer, Type, string, object> resolveDependency = (container, tService, name) => name == null ? container.ResolveService(tService) : container.ResolveImplementationType(tService, name);
        private readonly IDependencyInjectionHelper injectionHelper;

        public InjectionProviderBase(IDependencyInjectionHelper injectionHelper, IIocContainer container)
        {
            this.injectionHelper = injectionHelper;
            this.container = container ?? (this as IIocContainer);
        }

        public void ManualInjection<TInjectionTarget>(TInjectionTarget injectionTarget, Action<TInjectionTarget> injectionAction)
        {
            this.injectionHelper.ManualInjection((tService, name) => resolveDependency(this.container, tService, name), injectionTarget, injectionAction);
        }

        public TInjectionTarget ManualConstruction<TInjectionTarget>(Func<TInjectionTarget> injectionFunc)
        {
            return this.injectionHelper.ManualConstruction((tService, name) => resolveDependency(this.container, tService, name), injectionFunc);
        }

        public object AutoConstruct(CachedType targetType, Func<CachedType, CachedConstructorInfo> chooseConstructor = null, Func<CachedParameterInfo, string> getDependencyName = null)
        {
            return this.injectionHelper.AutoConstruct((tService, name) => resolveDependency(this.container, tService, name), targetType, chooseConstructor ?? defaultConstructorSelector, getDependencyName ?? DefaultGetDependencyName);
        }

        public void AutoInjectProperties(object injectionTarget, Func<CachedPropertyInfo, bool> chooseProperties = null, Func<CachedPropertyInfo, string> getDependencyName = null)
        {
            this.injectionHelper.AutoInjectProperties((tService, name) => resolveDependency(this.container, tService, name), injectionTarget, chooseProperties ?? defaultPropertySelector, getDependencyName ?? DefaultGetPropertyDependencyName);
        }

        public void AutoInjectMethods(object injectionTarget, Func<CachedMethodInfo, bool> chooseMethods = null, Func<CachedParameterInfo, string> getDependencyName = null)
        {
            this.injectionHelper.AutoInjectMethods((tService, name) => resolveDependency(this.container, tService, name), injectionTarget, chooseMethods ?? defaultMethodSelector, getDependencyName ?? DefaultGetDependencyName);
        }

        private static string DefaultGetDependencyName(CachedParameterInfo param)
        {
            if (param.InheritedAttributes.OfType<NamedDependencyInjectionTargetAttribute>().Any())
            {
                var attr = param.InheritedAttributes.OfType<NamedDependencyInjectionTargetAttribute>().Single();

                if (attr.ServiceName == null)
                {
                    return param.InnerReflectionInfo.Name;
                }

                return attr.ServiceName;
            }

            return null;
        }

        private static string DefaultGetPropertyDependencyName(CachedPropertyInfo prop)
        {
            if (prop.InheritedAttributes.OfType<NamedDependencyInjectionTargetAttribute>().Any())
            {
                var attr = prop.InheritedAttributes.OfType<NamedDependencyInjectionTargetAttribute>().Single();

                if (attr.ServiceName == null)
                {
                    return prop.InnerReflectionInfo.Name;
                }

                return attr.ServiceName;
            }

            return null;
        }
    }
}