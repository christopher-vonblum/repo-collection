using System;

namespace CVB.NET.Architecture.Facades
{
    public interface IFacadeProxyGenerator
    {
        object Create(Type tFacade);
        TFacade Create<TFacade>() where TFacade : class;
        object Expand(Type tFacade, object innerFacade);
        TFacade Expand<TFacade, TBaseFacade>(TBaseFacade innerFacade) where TFacade : class, TBaseFacade;
    }
}