namespace CVB.NET.Abstractions.Ioc.Base
{
    using System;

    public interface IIocContainerAdapter
    {
        void RegisterServiceInternal(Type tService, Type tImplementation, string serviceKey = null);
        void RegisterServiceInstanceInternal(Type tService, object instance, string serviceKey = null);
        Type ResolveImplementationTypeInternal(Type tService, string serviceKey = null);
        bool HasServiceRegistrationInternal(Type tService, string serviceKey = null);
        object ResolveServiceInternal(Type tService, string serviceKey = null);
    }
}