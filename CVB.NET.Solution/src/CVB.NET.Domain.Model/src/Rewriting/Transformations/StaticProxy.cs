namespace CVB.NET.Domain.Model.Rewriting.Transformations
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class StaticProxy
    {
        public static object GetCurrentProxy(Type staticType)
        {
            return GetProxyInfo(staticType).StaticInstanceStorage.GetValue(null);
        }

        public static void SetCurrentProxy(Type staticType, object value)
        {
            GetProxyInfo(staticType).StaticInstanceStorage.SetValue(null, value);
        }

        public static object CreateProxy(Type staticType)
        {
            return GetProxyInfo(staticType).ProxyConstructor.Invoke(null);
        }

        public static object CreateAndAssignProxy(Type staticType)
        {
            object proxy = CreateProxy(staticType);
            SetCurrentProxy(staticType, proxy);
            return proxy;
        }

        private static ProxyInfo GetProxyInfo(Type staticType)
        {
            ProxyTypeAttribute proxyTypeAttribute =
                staticType
                    .GetCustomAttributes(true)
                    .OfType<ProxyTypeAttribute>()
                    .FirstOrDefault();

            if (proxyTypeAttribute == null)
            {
                throw new NoLinkedProxyTypeFoundException();
            }

            Type proxyType = proxyTypeAttribute.ProxyType;

            return new ProxyInfo
                   {
                       ProxyType = proxyType,
                       ProxyConstructor = proxyType.GetConstructor(Type.EmptyTypes),
                       StaticInstanceStorage = proxyType.GetProperty("<Instance>____specialField")
                   };
        }

        private class ProxyInfo
        {
            public Type ProxyType { get; set; }
            public ConstructorInfo ProxyConstructor { get; set; }
            public PropertyInfo StaticInstanceStorage { get; set; }
        }
    }
}