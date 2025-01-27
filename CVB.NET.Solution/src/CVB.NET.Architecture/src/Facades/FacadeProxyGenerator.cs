namespace CVB.NET.Architecture.Facades
{
    using System;

    using Abstractions.Ioc;
    using Abstractions.Ioc.Container;
    using Castle.DynamicProxy;

    using CVB.NET.Abstractions.Ioc.Base;

    public class FacadeProxyGenerator : IFacadeProxyGenerator
    {
        private readonly IDependencyResolver dependencyResolver;

        private readonly IFacadeCache cache;
        public FacadeProxyGenerator(IDependencyResolver dependencyResolver, IFacadeCache facadeCache)
        {
            this.dependencyResolver = dependencyResolver;
            this.cache = facadeCache;
        }

        private static ProxyGenerator Generator = new ProxyGenerator();
        public TFacade Expand<TFacade, TBaseFacade>(TBaseFacade innerFacade) where TFacade : class, TBaseFacade
        {
            return (TFacade)Create(typeof(TFacade), innerFacade);
        }
        public object Expand(Type tFacade, object innerFacade)
        {
            return Create(tFacade, innerFacade);
        }
        public TFacade Create<TFacade>() where TFacade : class
        {
            return (TFacade)Create(typeof (TFacade));
        }

        public object Create(Type tFacade)
        {
            return Create(tFacade, null);
        }

        private object Create(Type tFacade, object baseFacade = null)
        {
            if (this.cache.HasFacadeRegistered(tFacade))
            {
                return this.cache.ResolveFacade(tFacade);
            }

            object facade;

            if (baseFacade == null)
            {
                facade = Generator.CreateInterfaceProxyWithoutTarget(tFacade, new FacadeInterceptor(this, this.dependencyResolver, null));
            }
            else
            {
                facade = Generator.CreateInterfaceProxyWithTarget(tFacade, baseFacade, new FacadeInterceptor(this, this.dependencyResolver, baseFacade.GetType()));
            }

            this.cache.RegisterFacade(tFacade, facade);

            return facade;
        }
    }
}
