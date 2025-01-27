using System;

namespace CVB.NET.Architecture.Facades
{
    public interface IFacadeCache
    {
        bool HasFacadeRegistered(Type facadeType);

        object ResolveFacade(Type facadeType);

        void RegisterFacade(Type facadeType, object facade);
    }
}