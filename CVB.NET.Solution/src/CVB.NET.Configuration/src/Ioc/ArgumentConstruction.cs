namespace CVB.NET.Configuration.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ConfigurationElements.Groups;
    using ConfigurationElements.Items;
    using NET.Ioc.Container;
    using NET.Ioc.Model;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching.Base;
    using Reflection.Caching.Cached;
    using Reflection.Caching.Extensions;

    public class ArgumentConstruction
    {
        public CachedParameterInfo ParameterInfo { get; }

        public List<ArgumentConstruction> ArgumentConstructions { get; }

        public Type ParameterType { get; }
        private readonly DebuggableLazy<object> instance;

        public ArgumentConstruction([NotNull] ParameterInfo parameter, [NotNull] object instance)
        {
            ParameterInfo = parameter;
            ParameterType = ParameterInfo.InnerReflectionInfo.ParameterType;

            if (!parameter.ParameterType.IsInstanceOfType(instance))
            {
                throw new ArgumentOutOfRangeException(nameof(instance));
            }

            this.instance = new DebuggableLazy<object>(instance);
        }

        public ArgumentConstruction([NotNull] CachedParameterInfo parameter, [NotNull] ConstructorElement constructorElement, IIocContainer container = null, Type implementationType = null)
        {
            ParameterInfo = parameter;

            Type infoParameterType = ParameterInfo.InnerReflectionInfo.ParameterType;

            if (implementationType == null && ParameterInfo.InnerReflectionInfo.ParameterType.IsInterface)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            if (implementationType == null)
            {
                ParameterType = infoParameterType;
            }
            else if (infoParameterType.IsInterface)
            {
                ParameterType = implementationType;
            }

            Type[] infoGenericArguments;

            if (ParameterType.ContainsGenericParameters && (infoGenericArguments = infoParameterType.GetGenericArguments()).Any())
            {
                ParameterType = ParameterType.MakeGenericType(infoGenericArguments);
            }

            CachedConstructorInfo constructor =((CachedType)ParameterType).ChooseConstructorOverload(constructorElement.Arguments.ToDictionary(key => key.Name, value => (object)value));

            ArgumentConstructions = GetChildConstructions(constructorElement, constructor, container);

            instance = new DebuggableLazy<object>(()
                => new ReflectionImplementationConstruction
                    (
                    ParameterType,
                    constructor,
                    new List<Type>(),
                    ArgumentConstructions.ToDictionary(key => key.ParameterInfo.InnerReflectionInfo.Name, value => value.GetInstance())
                    ).CreateInstance());
        }

        public object GetInstance() => instance.Value;

        public static List<ArgumentConstruction> GetChildConstructions(ConstructorElement constructorElement, CachedConstructorInfo constructorInfo, IIocContainer container)
        {
            List<ArgumentConstruction> constructions = new List<ArgumentConstruction>();

            foreach (ArgumentElement argument in constructorElement.Arguments)
            {
                CachedParameterInfo parameterInfo =
                    constructorInfo.CachedParameterInfos.FirstOrDefault(param => param.InnerReflectionInfo.Name.Equals(argument.Name));

                Type parameterType = parameterInfo.InnerReflectionInfo.ParameterType;

                object value;

                if (parameterType != typeof (string) && string.IsNullOrEmpty(argument.Value))
                {
                    if (!string.IsNullOrWhiteSpace(argument.InjectId) && container != null)
                    {
                        constructions.Add(new ArgumentConstruction(parameterInfo, container.GetNamedSingletonImplementationConstruction(parameterType, argument.InjectId).CreateInstance()));

                        continue;
                    }

                    if (parameterType.IsInterface)
                    {
                        constructions.Add(
                            new ArgumentConstruction(
                                parameterInfo,
                                argument.Inject,
                                container,
                                argument.InjectType));
                    }
                    else
                    {
                        constructions.Add(
                            new ArgumentConstruction(
                                parameterInfo,
                                argument.Inject,
                                container,
                                parameterType));
                    }

                    continue;
                }

                constructions.Add(new ArgumentConstruction(parameterInfo, argument.Value));
            }

            return constructions;
        }
    }
}