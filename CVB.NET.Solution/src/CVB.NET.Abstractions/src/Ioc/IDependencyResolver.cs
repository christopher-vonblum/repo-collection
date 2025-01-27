using System;

namespace CVB.NET.Abstractions.Ioc
{
    public interface IDependencyResolver
    {
        TService Resolve<TService>() where TService : class;
        TService Resolve<TService>(string name) where TService : class;

        object Resolve(Type tService);
        object Resolve(Type tService, string name);
    }
}