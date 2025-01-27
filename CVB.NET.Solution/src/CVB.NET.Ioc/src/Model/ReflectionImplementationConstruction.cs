namespace CVB.NET.Ioc.Model
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching.Cached;
    using Reflection.Caching.Extensions;

    [DebuggerDisplay("{SelectedConstructor}, {Type.ReflectedType.FullName}")]
    public class ReflectionImplementationConstruction : IImplementationConstruction
    {
        public ReflectionImplementationConstruction(
            [NotNull] CachedType type,
            [NotNull] CachedConstructorInfo selectedConstructor,
            [NotNull] IReadOnlyList<Type> typeArguments,
            [NotNull] IReadOnlyDictionary<string, object> constructorArguments)
            : this(type,
                selectedConstructor,
                typeArguments,
                selectedConstructor.GetParameterArgumentMapping(constructorArguments))
        {
        }

        public ReflectionImplementationConstruction(
            [NotNull] CachedType type,
            [NotNull] CachedConstructorInfo selectedConstructor,
            [NotNull] IReadOnlyList<Type> typeArguments,
            [NotNull] IReadOnlyDictionary<CachedParameterInfo, object> constructorArguments)
        {
            Type = type;
            SelectedConstructor = selectedConstructor;

            ConstructorArguments = SelectedConstructor.OrderInvocationTargetArguments(constructorArguments);

            if (Type.GenericTypeDefinition != null
                && typeArguments.Any())
            {
                Type = Type
                    .GenericTypeDefinition
                    .InnerReflectionInfo
                    .MakeGenericType(typeArguments.ToArray());
            }
        }

        public ReflectionImplementationConstruction(
            [NotNull] CachedType type,
            [NotNull] IReadOnlyDictionary<string, object> arguments,
            [NotNull] IReadOnlyList<Type> typeArguments)
        {
            Type = type;

            SelectedConstructor = type.ChooseConstructorOverload(arguments);

            ConstructorArguments = SelectedConstructor.OrderInvocationTargetArguments(arguments);

            if (Type.GenericTypeDefinition != null
                && typeArguments.Any())
            {
                Type = Type
                    .GenericTypeDefinition
                    .InnerReflectionInfo
                    .MakeGenericType(typeArguments.ToArray());
            }
        }

        public CachedType Type { get; }
        public CachedConstructorInfo SelectedConstructor { get; }
        public object[] ConstructorArguments { get; }
        public InstanceLifeStyle InstanceLifeStyle { get; set; }

        public object CreateInstance()
        {
            return SelectedConstructor.InnerReflectionInfo.Invoke(ConstructorArguments);
        }
    }
}