namespace CVB.NET.Abstractions.Ioc.Container.Activator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Provider;
    using Reflection.Caching.Cached;
    using Reflection.Caching.Extensions;
    using src.Ioc.Container.Attributes;

    public class DefaultReflectionActivator : IServiceActivator
    {
        public object CreateInstance(IReadOnlyIocProvider sourceContainer, IServiceRegistration registration)
        {
            CachedType constructionType = registration.ComponentType;

            constructionType.ChooseConstructorOverload(
                p => GetArgument(sourceContainer, p),
                ctor => !ctor.Attributes.OfType<NoInjectionAttribute>().Any());
        }

        private object GetArgument(IReadOnlyIocProvider sourceContainer, CachedParameterInfo parameter)
        {
            IReadOnlyDictionary<string, object> serviceVarianceModifier = parameter.Attributes.OfType<ServiceVarianceModifier>().ToDictionary(v => v.VarianceModifier.Key, v => v.VarianceModifier.Value);

            if (ShouldResolveArgument(parameter)
             && sourceContainer.Container.HasServiceRegistration(parameter.Reflected.ParameterType, serviceVarianceModifier))
            {
                return sourceContainer
                        .GetInstance(
                            parameter.Reflected.ParameterType,
                            serviceVarianceModifier);
            }

            
        }

        private bool ShouldResolveArgument(CachedParameterInfo parameter)
        {
            return !parameter.Attributes.OfType<NoInjectionAttribute>().Any();
        }
    }

    public class NoInjectionAttribute : Attribute
    {
    }
}