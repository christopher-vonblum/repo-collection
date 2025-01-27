using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using CoreUi.Model;
using CoreUi.Objects;

namespace CoreUi.Proxy.Factory
{
    public static class ProxyFactory
    {
        private static ConcurrentDictionary<(Type, Type proxy), MethodInfo> createProxy_genericOverloads = new ConcurrentDictionary<(Type, Type proxy), MethodInfo>();

        public static object CreateProxy(Type t, Type proxy)
        {
            return createProxy_genericOverloads
                .GetOrAdd((t, proxy), tuple =>
                    typeof(ProxyFactory)
                        .GetMethod(nameof(CreateProxyInternal), BindingFlags.Static | BindingFlags.Public)
                        .MakeGenericMethod(tuple.Item1, tuple.proxy)).Invoke(null, null);
        }

        public static object CreateProxyInternal<T, TProxy>() where TProxy : DispatchProxy
        {
            return DispatchProxy.Create<T, TProxy>();
        }
        
        public static T CreateInputModel<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return default(T);
            }
            if (IsSimpleField(typeof(T)))
            {
                return (T)Activator.CreateInstance(typeof(T));
            }

            if (!typeof(T).IsInterface)
            {
                return default;
            }
            IObjectProxy proxy = null;
            IObject o = new O();
            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                Type el = typeof(T).GetGenericArguments()[0];
                proxy = (IObjectProxy)CreateProxy(typeof(T), typeof(EnumerableProxy<>).MakeGenericType(el));
            }
            else
            {
                proxy = (IObjectProxy) DispatchProxy.Create<T, ObjectProxy>();
                typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Concat(typeof(T).GetInterfaces().SelectMany(i => i.GetProperties(BindingFlags.Public | BindingFlags.Instance)))
                    .Select(p => new PropertyDefinition()
                    {
                        Name = p.DeclaringType.Name + "." + p.Name,
                        ClrType = p.PropertyType,
                        ClrDeclaringType = p.DeclaringType
                    })
                    .ToList()
                    .ForEach(p => o.CreateProperty(p));
            }
            
            proxy.T = typeof(T);
            proxy.Object = o;

            return (T)proxy;
        }

        private static ConcurrentDictionary<Type, MethodInfo> genericOverloads = new ConcurrentDictionary<Type, MethodInfo>();

        public static object CreateProxyOrValue(Type t)
        {
            return genericOverloads
                .GetOrAdd(t, pt => 
                    typeof(ProxyFactory)
                    .GetMethod(nameof(CreateInputModel), BindingFlags.Static | BindingFlags.Public)
                    .MakeGenericMethod(t))
                .Invoke(null, null);
        }
        
        public static bool IsSimpleField(Type fieldDefinition)
        {
            return fieldDefinition.IsValueType || fieldDefinition == typeof(string) || typeof(Enum).IsAssignableFrom(fieldDefinition);
        }
    }
}