namespace CVB.NET.Rewriting.Utils
{
    using System;
    using System.Linq;
    using Mono.Cecil;

    public static class CustomAttributeExtensions
    {
        /* Cecil */

        public static void AddCustomAttribute<TAttribute>(this ICustomAttributeProvider cecilAttributeProvider, params object[] attributeArguments)
            where TAttribute : Attribute
        {
            cecilAttributeProvider.AddCustomAttribute(typeof (TAttribute), attributeArguments);
        }

        public static void AddCustomAttribute(this ICustomAttributeProvider cecilAttributeProvider, Type tAttribute, params object[] attributeArguments)
        {
            ModuleDefinition importModule = null;

            if (cecilAttributeProvider is AssemblyDefinition)
            {
                importModule = ((AssemblyDefinition) cecilAttributeProvider).MainModule;
            }
            else if (cecilAttributeProvider is TypeDefinition)
            {
                importModule = ((TypeDefinition) cecilAttributeProvider).Module;
            }
            else if (cecilAttributeProvider is IMemberDefinition)
            {
                importModule = ((IMemberDefinition) cecilAttributeProvider).DeclaringType.Module;
            }

            CustomAttribute attribute =
                new CustomAttribute(
                    importModule.Import(
                        tAttribute.GetConstructor(attributeArguments != null && attributeArguments.Any()
                            ? attributeArguments.Select(attrArg => attrArg.GetType()).ToArray()
                            : Type.EmptyTypes)));

            attribute.ConstructorArguments.Clear();

            if (attributeArguments != null && attributeArguments.Any())
            {
                foreach (object arg in attributeArguments)
                {
                    attribute.ConstructorArguments.Add(new CustomAttributeArgument(importModule.Import(arg.GetType()), arg));
                }
            }

            cecilAttributeProvider.CustomAttributes.Add(attribute);
        }

        public static void EnsureHasCustomAttribute<TAttribute>(this ICustomAttributeProvider cecilAttributeProvider, params object[] attributeArguments)
            where TAttribute : Attribute
        {
            cecilAttributeProvider.EnsureHasCustomAttribute(typeof (TAttribute), attributeArguments);
        }

        public static void EnsureHasCustomAttribute(this ICustomAttributeProvider cecilAttributeProvider, Type attributeType, params object[] attributeArguments)
        {
            if (!cecilAttributeProvider.HasCustomAttribute(attributeType))
            {
                cecilAttributeProvider.AddCustomAttribute(attributeType, attributeArguments);
            }
        }

        public static bool HasCustomAttribute<TAttribute>(this ICustomAttributeProvider cecilAttributeProvider)
            where TAttribute : Attribute
        {
            return cecilAttributeProvider.HasCustomAttribute(typeof (TAttribute));
        }

        public static bool HasCustomAttribute(this ICustomAttributeProvider cecilAttributeProvider, Type tAttribute)
        {
            return cecilAttributeProvider.CustomAttributes.Any(attr => attr.GetType() == tAttribute);
        }

        /* Reflection */

        public static bool HasCustomAttribute<TAttribute>(this System.Reflection.ICustomAttributeProvider reflectionAssembly)
            where TAttribute : Attribute
        {
            return reflectionAssembly.HasCustomAttribute(typeof (TAttribute));
        }

        public static bool HasCustomAttribute(this System.Reflection.ICustomAttributeProvider reflectionAssembly, Type tAttribute)
        {
            return reflectionAssembly.GetCustomAttributes(tAttribute, false).Any();
        }
    }
}