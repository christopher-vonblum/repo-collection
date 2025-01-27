namespace CVB.NET.Domain.Model.Rewriting.Transformations
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Mono.Cecil;
    using NET.Rewriting.Compiler.CompilationUnits.Tasks.Cecil.Base;
    using NET.Rewriting.Utils;
    using PostSharp.Aspects;
    using PostSharp.Extensibility;
    using PostSharp.Reflection;
    using TypeAttributes = Mono.Cecil.TypeAttributes;

    [MulticastAttributeUsage(MulticastTargets.Property, AllowMultiple = false, HideFromAspectBrowser = false,
        Inheritance = MulticastInheritance.None)]
    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public class GenerateProxyTypeAspect : LocationInterceptionAspect, ITypeTransformationAttribute
    {
        private Type ProxyType { get; set; }

        private object CurrentProxy => ProxyType.GetProperty("<Instance>____specialField", BindingFlags.Static | BindingFlags.Public).GetValue(null);

        public void ApplyTransformation(Assembly reflectionAssembly, AssemblyDefinition cecilAssembly, Type reflectionType,
                                        TypeDefinition cecilType)
        {
            ProcessType(cecilAssembly, reflectionType);
        }

        public override void RuntimeInitialize(LocationInfo locationInfo)
        {
            ProxyType = locationInfo.DeclaringType.GetCustomAttributes(true).OfType<ProxyTypeAttribute>().First().ProxyType;
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            args.Value = ProxyType.GetProperty(args.Location.Name).GetValue(CurrentProxy);
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            ProxyType.GetProperty(args.Location.Name).SetValue(CurrentProxy, args.Value);
        }

        private static void ProcessType(AssemblyDefinition assembly, Type targetType)
        {
            TypeDefinition type = GetCorrespondingTypeDefinition(assembly, targetType);

            GenerateProxyType(targetType, type);
        }

        private static void GenerateProxyType(Type targetType, TypeDefinition typeDefinition)
        {
            TypeDefinition proxyType =
                new TypeDefinition(
                    targetType.Namespace,
                    $"<{targetType.Name}>__staticProxy",
                    TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            proxyType.BaseType = typeDefinition.Module.Import(typeof (MarshalByRefObject));

            typeDefinition.Module.Types.Add(proxyType);

            TypeGenerationUtils.CreateEmptyConstructor(proxyType);

            typeDefinition.CustomAttributes.Add(
                new CustomAttribute(
                    typeDefinition.Module.Import(typeof (ProxyTypeAttribute)
                        .GetConstructor(new Type[] {typeof (string)})))
                {
                    ConstructorArguments =
                    {
                        new CustomAttributeArgument(typeDefinition.Module.TypeSystem.String, proxyType.FullName + ", " + targetType.Assembly.FullName)
                    }
                });

            MethodReference reference =
                proxyType.Module.Import(typeof (CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes));

            proxyType.CustomAttributes.Add(new CustomAttribute(reference));

            PropertyInfo[] staticProperties = targetType.GetProperties(BindingFlags.Static | BindingFlags.Public);

            foreach (PropertyInfo prop in staticProperties)
            {
                PropertyGenerationUtils.CreateAutoProperty(
                    proxyType,
                    proxyType.Module.Import(prop.PropertyType),
                    prop.Name);
            }

            AddSpecialPropertiesToProxy(proxyType, targetType);
        }

        private static void AddSpecialPropertiesToProxy(TypeDefinition proxyType, Type targetType)
        {
            PropertyGenerationUtils.CreateAutoProperty(
                proxyType,
                proxyType.Module.Import(proxyType),
                "<Instance>____specialField",
                isStatic: true);
        }

        private static TypeDefinition GetCorrespondingTypeDefinition(AssemblyDefinition assembly, Type reflectionType)
        {
            return assembly
                .MainModule
                .Types
                .Single(t =>
                    t.FullName.Equals(reflectionType.FullName));
        }

        private static Type[] GetTargetStaticClasses(Assembly reflectionAssembly)
        {
            return reflectionAssembly
                .GetTypes()
                .Where(asm =>
                    asm.GetCustomAttributes(false)
                        .OfType<GenerateProxyTypeAttribute>()
                        .Any())
                .ToArray();
        }
    }

    public class NoLinkedProxyTypeFoundException : System.Exception
    {
    }
}