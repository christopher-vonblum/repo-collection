namespace CVB.NET.Domain.Model.Aspect
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using Exception;
    using PostSharp.Aspects;
    using PostSharp.Extensibility;
    using Provider;
    using Reflection.Caching.Cached;
    using Rewriting;

    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    [MulticastAttributeUsage(MulticastTargets.Property, PersistMetaData = true, TargetMemberAttributes = MulticastAttributes.Public)]
    public class DomainServicesRemotingAccessorAspect : LocationInterceptionAspect
    {
        private static Lazy<AppDomain> lazyRemoteDomain;
        private static AppDomain RemoteDomain => lazyRemoteDomain.Value;

        private readonly ConcurrentDictionary<CachedType, object> typeToProxyDictionary = new ConcurrentDictionary<CachedType, object>();

        public DomainServicesRemotingAccessorAspect()
        {
            lazyRemoteDomain = new Lazy<AppDomain>(AppDomainProvider.GetRootDomain);
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                args.ProceedGetValue();
                return;
            }

            object proxy = GetProxy(args.Location.PropertyInfo.DeclaringType);

            CachedPropertyInfo proxyProperty = GetPropertyInfo(proxy.GetType(), args.LocationName);

            args.Value = proxyProperty.InnerReflectionInfo.GetValue(proxy);
        }

        private CachedPropertyInfo GetPropertyInfo(CachedType declaringType, string propertyName)
        {
            return declaringType.Properties.FirstOrDefault(prop => prop.InnerReflectionInfo.Name == propertyName);
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (!AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                throw new CantSetServiceFromClientDomainException();
            }

            args.ProceedSetValue();
        }

        private object GetProxy(Type declaringType)
        {
            string proxyClassName = StaticClassPropertyProxyTypeGenerationUtils.GetStaticClassPropertyProxyTypeName(declaringType);

            CachedType proxyClassType = Type.GetType(proxyClassName);

            return typeToProxyDictionary.GetOrAdd(declaringType, type => CreateProxy(proxyClassType));
        }

        private object CreateProxy(CachedType proxyClassType)
        {
            return RemoteDomain.CreateInstance(proxyClassType.InnerReflectionInfo.Assembly.FullName, proxyClassType.InnerReflectionInfo.FullName);
        }
    }
}