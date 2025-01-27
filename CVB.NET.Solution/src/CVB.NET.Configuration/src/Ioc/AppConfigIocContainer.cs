using CVB.NET.Reflection.Caching.Cached;
using CVB.NET.Reflection.Caching.Extensions;

namespace CVB.NET.Configuration.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using ConfigurationElements;
    using ConfigurationElements.Elements;
    using ConfigurationElements.Groups;
    using NET.Ioc.Container;
    using NET.Ioc.Model;
    using PostSharp.Patterns.Contracts;

    public class AppConfigIocContainer : IocContainer
    {
        public AppConfigIocContainer(string sectionName)
        {
            ServiceLocationSection locationSection =
                (ServiceLocationSection) ConfigurationManager.GetSection(sectionName);

            if (locationSection == null)
            {
                throw new MissingConfigurationSectionException(sectionName);
            }

            Initialize(locationSection);
        }

        public void Initialize([NotNull] ServiceLocationSection configurationSection)
        {
            foreach (DependencyElement dependencyElement in configurationSection.Dependencies)
            {
                if (dependencyElement.InterfaceType.Type == null
                    && dependencyElement.InterfaceType.ElementInformation.Properties.Keys.Cast<string>().Contains("type")
                    && !string.IsNullOrWhiteSpace(dependencyElement.InterfaceType["type"] as string))
                {
                    throw new ConfigurationErrorsException("Interface type \"" + dependencyElement.InterfaceType["type"] +
                                                           "\" could not be found.");
                }

                if (dependencyElement.ImplementationType.Type == null || !dependencyElement.ImplementationType.Type.IsClass)
                {
                    throw new ConfigurationErrorsException("Class type \"" + dependencyElement.ImplementationType["type"] +
                                                           "\" could not be found.");
                }

                Type interfaceType = dependencyElement.InterfaceType.Type,
                     implementationType = dependencyElement.ImplementationType.Type;

                if (dependencyElement.InterfaceType.Type != null && dependencyElement.InterfaceType.Type.IsGenericType && dependencyElement.InterfaceType.Arguments.Any())
                {
                    interfaceType =
                        dependencyElement.InterfaceType.Type.MakeGenericType(
                            dependencyElement.InterfaceType.Arguments.Select(arg => arg.Type).ToArray());
                }

                if (interfaceType == null)
                {
                    interfaceType = implementationType;
                }

                if (dependencyElement.ImplementationType.Type.IsGenericType && dependencyElement.ImplementationType.Arguments.Any())
                {
                    implementationType =
                        dependencyElement.ImplementationType.Type.MakeGenericType(
                            dependencyElement.ImplementationType.Arguments.Select(arg => arg.Type).ToArray());
                }

                ReflectionImplementationConstruction reflectionImplementationConstruction =
                    GetImplementationConstruction(
                        dependencyElement
                            .Constructor,
                        implementationType,
                        dependencyElement
                            .ImplementationType
                            .Arguments
                            .Select(
                                param => param.Type).ToArray());

                reflectionImplementationConstruction.InstanceLifeStyle = dependencyElement.LifeStyle;

                if (string.IsNullOrWhiteSpace(dependencyElement.Id))
                {
                    SetDefaultImplementationConstruction(
                        interfaceType,
                        reflectionImplementationConstruction);
                    continue;
                }

                SetNamedSingletonImplementationConstruction(
                    interfaceType,
                    dependencyElement.Id,
                    reflectionImplementationConstruction);
            }
        }

        private ReflectionImplementationConstruction GetImplementationConstruction(ConstructorElement constructorValues, CachedType implementationType, Type[] typeParameters)
        {
            CachedConstructorInfo selectedConstructor = 
                implementationType.ChooseConstructorOverload(constructorValues.Arguments.ToDictionary(arg => arg.Name, val => (object)val));

            List<ArgumentConstruction> childConstructions = ArgumentConstruction.GetChildConstructions(
                constructorValues,
                selectedConstructor,
                this);

            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            foreach (ArgumentConstruction construction in childConstructions)
            {
                try
                {
                    dictionary.Add(construction.ParameterInfo.InnerReflectionInfo.Name, construction.GetInstance());
                }
                catch (Exception ex)
                {
                    throw new ConfigurationErrorsException(ex.Message, ex);
                }
            }

            return new ReflectionImplementationConstruction(
                implementationType,
                selectedConstructor,
                typeParameters,
                dictionary);
        }
    }
}