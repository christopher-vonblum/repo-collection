using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace CVB.NET.Domain.Model.Rewriting
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Mono.Cecil;
    using NET.Rewriting.Compiler.CompilationUnits.Tasks.Cecil.Base;
    using Reflection.Caching;
    using Reflection.Caching.Cached;

    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Property, PersistMetaData = true)]
    public class ImplementStaticClassProxyAspect : LocationInterceptionAspect, ITypeTransformationAttribute
    {
        public void ApplyTransformation(Assembly reflectionAssembly, AssemblyDefinition cecilAssembly,
                                        Type reflectionType,
                                        TypeDefinition cecilType)
        {
            StaticClassPropertyProxyTypeGenerationUtils.CreateStaticClassPropertyProxyType(cecilType, reflectionType);
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            args.Value = GetValue(args.Location.PropertyInfo.DeclaringType, args.Location.PropertyInfo.Name);
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            throw new NotSupportedException();
        }

        private object GetValue(CachedType declaringType, string name)
        {
            ProxyTypeAttribute proxyTypeAttribute =
                declaringType.Attributes.OfType<ProxyTypeAttribute>().FirstOrDefault();

            if (proxyTypeAttribute == null)
            {
                throw new ProxyMissingStaticClassReferenceAttribute();
            }

            CachedPropertyInfo[] staticProperties =
                ReflectionCache.GetLookup(ProxyPropertiesLookup.GetPublicStaticProperties, proxyTypeAttribute.ProxyClass);

            CachedPropertyInfo property = staticProperties.FirstOrDefault(prop => prop.InnerReflectionInfo.Name.Equals(name));

            if (property == null)
            {
                throw new MissingMemberException();
            }

            return property.InnerReflectionInfo.GetValue(null);
        }
    }
}