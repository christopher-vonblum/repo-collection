using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace DataDomain
{
    public delegate IEntityProxy EntityProxyFactoryDelegate(IEntityType entityType, IServiceProvider serviceProvider);
    
    public class ProxyCompiler
    {
        public Expression<EntityProxyFactoryDelegate> CreateDefaultFactory()
        {
            return (type, provider) => InitProxy(type, provider);
        }

        private IEntityProxy InitProxy(IEntityType type, IServiceProvider provider)
        {
            var proxy = (IEntityProxy)CreateProxy(type, typeof(EntityProxy));
            proxy.ActivationServiceProvider = provider;
            return proxy;
        }
        
        private object CreateProxy(IEntityType t, Type proxy)
        {
            var tp = CreateEntityProxyUnionType(t);

            return CreateProxy(tp.CreateType(), proxy);
        }

        private static TypeBuilder CreateEntityProxyUnionType(IEntityType t)
        {
            AssemblyBuilder a =
                AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Rad.ProxyTypes"), AssemblyBuilderAccess.RunAndCollect);
            var tp = a.DefineDynamicModule("d").DefineType("p", TypeAttributes.Public |
                                                                TypeAttributes.Interface |
                                                                TypeAttributes.Abstract |
                                                                TypeAttributes.AutoClass |
                                                                TypeAttributes.AnsiClass |
                                                                TypeAttributes.BeforeFieldInit |
                                                                TypeAttributes.AutoLayout
                , null);

            foreach (var segment in t.DataSegments)
            {
                tp.AddInterfaceImplementation(segment.GetRuntimeType());
            }

            return tp;
        }

        private static ConcurrentDictionary<(Type t, Type proxy), MethodInfo> createProxy_genericOverloads = new ConcurrentDictionary<(Type t, Type proxy), MethodInfo>();

        private static object CreateProxy(Type t, Type proxy)
        {
            return createProxy_genericOverloads
                .GetOrAdd((t, proxy), tuple =>
                    typeof(ProxyCompiler)
                        .GetMethod(nameof(CreateProxyInternal), BindingFlags.Static | BindingFlags.NonPublic)
                        .MakeGenericMethod(tuple.t, tuple.proxy)).Invoke(null, null);
        }
        private static object CreateProxyInternal<T, TProxy>() where TProxy : DispatchProxy
        {
            return DispatchProxy.Create<T, TProxy>();
        }
    }
}