using System;

using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;

namespace CVB.NET.Architecture.Facades
{
    public class IocFacadeCache : IFacadeCache
    {
        private readonly IDependencyService dependencyService;
        public IocFacadeCache(IDependencyService dependencyService)
        {
            this.dependencyService = dependencyService;
        }

        public bool HasFacadeRegistered(Type facadeType)
        {
            return this.dependencyService.HasRegistration(facadeType);
        }

        public object ResolveFacade(Type facadeType)
        {
            return this.dependencyService.Resolve(facadeType);
        }

        public void RegisterFacade(Type facadeType, object facade)
        {
            this.dependencyService.Register(Service.For(facadeType, facade));
        }
    }
}