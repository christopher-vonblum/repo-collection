namespace CVB.NET.Domain.Model.Rewriting
{
    using System;
    using System.Reflection;
    using Mono.Cecil;
    using NET.Rewriting.Utils;
    using Reflection.Caching;
    using Reflection.Caching.Cached;
    using TypeAttributes = Mono.Cecil.TypeAttributes;

    public static class StaticClassPropertyProxyTypeGenerationUtils
    {
        public static string GetStaticClassPropertyProxyTypeName(Type staticClass)
        {
            return staticClass.Namespace + $".<{staticClass.Name}>__staticProxy," + staticClass.Assembly.FullName;
        }

        public static void CreateStaticClassPropertyProxyType(TypeDefinition cecilType, Type staticClass)
        {
            TypeDefinition staticClassPropertiesProxy =
                new TypeDefinition(
                    staticClass.Namespace,
                    $"<{staticClass.Name}>__staticProxy",
                    TypeAttributes.Class | TypeAttributes.SpecialName);

            cecilType.Module.Types.Add(staticClassPropertiesProxy);

            staticClassPropertiesProxy.BaseType = staticClassPropertiesProxy.Module.Import(typeof (MarshalByRefObject));

            staticClassPropertiesProxy
                .CustomAttributes
                .Add(new CustomAttribute(
                        staticClassPropertiesProxy.Module.Import(
                            typeof (ImplementStaticClassProxyAspect)
                                .GetConstructor(Type.EmptyTypes))));

            CustomAttribute staticClassReference = new CustomAttribute(
                staticClassPropertiesProxy.Module.Import(
                    typeof (ProxyTypeAttribute)
                        .GetConstructor(new[] { typeof (string) })));

            staticClassReference.ConstructorArguments.Add(new CustomAttributeArgument(staticClassPropertiesProxy.Module.Import(typeof (string)), staticClassPropertiesProxy.FullName + ", " + staticClassPropertiesProxy.Module.Assembly.FullName));

            cecilType
                .CustomAttributes
                .Add(staticClassReference);

            CachedPropertyInfo[] propertiesLookup = ReflectionCache.GetLookup(ProxyPropertiesLookup.GetPublicStaticProperties, staticClass);

            foreach (PropertyInfo property in propertiesLookup)
            {
                PropertyGenerationUtils.CreateAutoProperty(
                    staticClassPropertiesProxy,
                    staticClassPropertiesProxy.Module.Import(property.PropertyType),
                    property.Name);
            }
        }
    }
}