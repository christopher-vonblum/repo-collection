using CVB.NET.Reflection.Caching.Cached;

namespace CVB.NET.Configuration.Aspects
{
    using System;
    using System.Collections.Concurrent;
    using System.Configuration;
    using System.Linq;
    using Base;
    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Aspects.Dependencies;
    using PostSharp.Extensibility;
    

    [ProvideAspectRole(StandardRoles.Caching)]
    [MulticastAttributeUsage(MulticastTargets.Class | MulticastTargets.Interface,
        Inheritance = MulticastInheritance.Strict)]
    [AttributeUsage(AttributeTargets.Interface)]
    [Serializable]
    public class ConfigurationElementAspect : TypeLevelAspect
    {
        private ConcurrentDictionary<string, object> PropertyValues { get; } =
            new ConcurrentDictionary<string, object>();

        [OnLocationGetValueAdvice,
         MulticastPointcut(Targets = MulticastTargets.Property,
             Attributes = MulticastAttributes.Instance | MulticastAttributes.NonAbstract)]
        public void OnPropertyGet(LocationInterceptionArgs args)
        {
            if (args.Location.PropertyInfo.GetIndexParameters().Any() || args.LocationName == "ProxyTarget")
            {
                args.ProceedGetValue();
                return;
            }

            IConfigurationElement configurationElement = (IConfigurationElement) args.Instance;

            CachedPropertyInfo interceptionTarget = args.Location.PropertyInfo;

            ConfigurationPropertyAttribute configurationPropertyAttribute = interceptionTarget.Attributes
                .OfType<ConfigurationPropertyAttribute>()
                .FirstOrDefault();

            if (configurationPropertyAttribute == null)
            {
                args.ProceedGetValue();
                return;
            }

            bool isProxyProperty = false;

            if (configurationElement.ProxyTarget != null)
            {
                isProxyProperty = configurationElement.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(configurationPropertyAttribute.Name, StringComparison.InvariantCultureIgnoreCase)) != null
                    && configurationElement.ProxyTarget.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(configurationPropertyAttribute.Name, StringComparison.InvariantCultureIgnoreCase)) == null;
                configurationElement = configurationElement.ProxyTarget;
            }

            ConfigurationCollectionAttribute configurationCollectionAttribute =
                interceptionTarget.Attributes.OfType<ConfigurationCollectionAttribute>().FirstOrDefault();

            if (configurationPropertyAttribute.IsDefaultCollection)
            {
                args.Value = PropertyValues.GetOrAdd(
                    configurationElement.ToString(),
                    alias =>
                        isProxyProperty ? configurationElement.GetProperty(string.Empty) : configurationElement[string.Empty]
                        ?? GetEmptyConstructorCollectionInstance(args.Location.PropertyInfo.PropertyType));

                return;
            }

            if (configurationCollectionAttribute != null)
            {
                args.Value = PropertyValues.GetOrAdd(
                    configurationElement + configurationPropertyAttribute.Name,
                    alias =>
                        isProxyProperty ? configurationElement.GetProperty(configurationPropertyAttribute.Name) : configurationElement[configurationPropertyAttribute.Name]
                        ?? GetEmptyConstructorCollectionInstance(args.Location.PropertyInfo.PropertyType));

                return;
            }

            args.Value = PropertyValues.GetOrAdd(configurationElement + configurationPropertyAttribute.Name,
                alias =>
                {
                    var value = isProxyProperty ? configurationElement.GetProperty(configurationPropertyAttribute.Name) : configurationElement[configurationPropertyAttribute.Name];

                    if (value == null)
                    {
                        args.ProceedGetValue();
                        return args.Value;
                    }

                    return value;
                });
        }

        [OnLocationSetValueAdvice,
         MulticastPointcut(Targets = MulticastTargets.Property,
             Attributes = MulticastAttributes.Instance | MulticastAttributes.NonAbstract)]
        public void OnPropertySet(LocationInterceptionArgs args)
        {
            if (args.Location.PropertyInfo.GetIndexParameters().Any() || args.LocationName == "ProxyTarget")
            {
                args.ProceedSetValue();
                return;
            }

            IConfigurationElement configurationElement = (IConfigurationElement)args.Instance;

            CachedPropertyInfo interceptionTarget = args.Location.PropertyInfo;

            ConfigurationPropertyAttribute configurationPropertyAttribute = interceptionTarget.Attributes
                .OfType<ConfigurationPropertyAttribute>()
                .FirstOrDefault();

            if (configurationPropertyAttribute == null)
            {
                args.ProceedSetValue();
                return;
            }

            if (configurationElement.ProxyTarget != null)
            {
                configurationElement = configurationElement.ProxyTarget;
            }

            ConfigurationCollectionAttribute configurationCollectionAttribute =
                interceptionTarget.Attributes.OfType<ConfigurationCollectionAttribute>().FirstOrDefault();

            if (configurationPropertyAttribute.IsDefaultCollection)
            {
                configurationElement.SetProperty(
                    string.Empty,
                    PropertyValues[configurationElement.ToString()] = args.Value ?? GetEmptyConstructorCollectionInstance(args.Location.PropertyInfo.PropertyType));

                return;
            }

            if (configurationCollectionAttribute != null)
            {
                configurationElement.SetProperty(
                    configurationPropertyAttribute.Name,
                    PropertyValues[configurationElement.ToString()] = args.Value);

                return;
            }

            configurationElement.SetProperty(
                configurationPropertyAttribute.Name,
                 PropertyValues[configurationElement + configurationPropertyAttribute.Name] = args.Value);
        }

        private object GetEmptyConstructorCollectionInstance(Type collectionType)
        {
            return collectionType.GetConstructor(Type.EmptyTypes).Invoke(null);
        }
    }
}