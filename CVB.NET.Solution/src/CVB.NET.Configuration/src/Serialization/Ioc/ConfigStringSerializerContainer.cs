namespace CVB.NET.Configuration.Serialization.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Base;

    using ConfigurationElements;
    using Exceptions.Reflection;
    using NET.Ioc.Container;
    using NET.Ioc.Model;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching;
    using Reflection.Caching.Cached;

    public class ConfigStringSerializerContainer : IocContainer
    {
        private static Type ConfigStringSerializerInterface { get; } = typeof (IConfigStringSerializer);

        public ConfigStringSerializerContainer()
        {
            Configure((IStringSerializerContainerConfiguration) ConfigurationManager.GetSection("complexConfigStringSerializers"));
        }

        public void Configure(IStringSerializerContainerConfiguration configuration)
        {
            foreach (IStringSerializerConfiguration serializerElement in configuration.Serializers)
            {
                CachedType serializerImplementationType = ReflectionCache.Get<CachedType>(Type.GetType(serializerElement.Type));

                if (serializerImplementationType.InnerReflectionInfo.IsGenericTypeDefinition)
                {
                    List<Type> targetTypes = serializerElement.TargetTypes.Select(targetType => Type.GetType(targetType.Type)).ToList();

                    if (targetTypes.Any())
                    {
                        RegisterGenericConfigStringSerializer(Type.GetType(serializerElement.Type), targetTypes);
                    }
                    else
                    {
                        RegisterOpenGenericConfigStringSerializer(Type.GetType(serializerElement.Type));
                    }

                    continue;
                }

                RegisterConfigStringSerializer(serializerImplementationType);
            }
        }

        private void RegisterOpenGenericConfigStringSerializer(Type serializerType)
        {
            CachedType type = serializerType;

            SetNamedSingletonImplementationConstruction(
                    ConfigStringSerializerInterface,
                    serializerType.AssemblyQualifiedName,
                    new ReflectionImplementationConstruction(
                        type,
                        type.DefaultConstructor,
                        new List<Type>(),
                        new Dictionary<string, object>()));
        }

        private void RegisterConfigStringSerializer([NotNull] CachedType serializerImplementationType)
        {
            Type configStringSerializerInterface = serializerImplementationType
                .Interfaces
                .SingleOrDefault(intf => intf.GenericTypeDefinition == typeof (IConfigStringSerializer<>));

            if (configStringSerializerInterface == null)
            {
                throw new InterfaceNotImplementedException(serializerImplementationType, ConfigStringSerializerInterface);
            }

            Type valueType = configStringSerializerInterface
                .GenericTypeArguments
                .Single();

            SetNamedSingletonImplementationConstruction(
                ConfigStringSerializerInterface,
                valueType.AssemblyQualifiedName,
                new ReflectionImplementationConstruction(
                    serializerImplementationType,
                    serializerImplementationType.DefaultConstructor,
                    new List<Type>(),
                    new Dictionary<string, object>()));
        }

        private void RegisterGenericConfigStringSerializer([NotNull] CachedType serializerImplementationType, [NotEmpty] List<Type> targetTypes)
        {
            foreach (Type targetType in targetTypes)
            {
                CachedType genericSerializerType = serializerImplementationType.InnerReflectionInfo.MakeGenericType(targetType);

                SetNamedSingletonImplementationConstruction(
                    ConfigStringSerializerInterface,
                    targetType.AssemblyQualifiedName,
                    new ReflectionImplementationConstruction(
                        genericSerializerType,
                        genericSerializerType.DefaultConstructor,
                        new List<Type>(),
                        new Dictionary<string, object>()));
            }
        }
    }
}