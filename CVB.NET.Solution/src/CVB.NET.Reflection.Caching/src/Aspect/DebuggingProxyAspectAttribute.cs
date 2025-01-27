using System;
using System.Linq;

using CVB.NET.Reflection.Caching.Cached;

using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace CVB.NET.Reflection.Caching.Aspect
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Property, Inheritance = MulticastInheritance.Multicast)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class DebuggingProxyAspectAttribute : LocationInterceptionAspect
    {
        public override void OnGetValue(LocationInterceptionArgs args)
        {
            if (args.Location.PropertyInfo == null || args.LocationName == nameof(IDebuggingProxy.Inner))
            {
                args.ProceedGetValue();
                return;
            }

            IDebuggingProxy proxy = (IDebuggingProxy) args.Instance;

            args.Value = proxy.Inner.GetType().GetProperty(args.LocationName).GetValue(proxy.Inner);
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (args.Location.PropertyInfo == null || args.LocationName == nameof(IDebuggingProxy.Inner))
            {
                args.ProceedSetValue();
                return;
            }

            IDebuggingProxy proxy = (IDebuggingProxy) args.Instance;

            CachedType proxyType = proxy.Inner.GetType();
            
            proxyType.Properties.Single(p => p.InnerReflectionInfo.Name == args.LocationName).InnerReflectionInfo.SetValue(proxy.Inner, args.Value);
        }
    }
}