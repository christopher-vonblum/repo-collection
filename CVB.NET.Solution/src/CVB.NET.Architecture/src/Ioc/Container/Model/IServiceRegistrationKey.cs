namespace CVB.NET.Abstractions.Ioc.Container.Model
{
    using System;
    using System.Collections.Generic;

    public interface IServiceRegistrationKey
    {
        Type Service { get; }
        IReadOnlyDictionary<string, object> VaryBy { get; }
        bool RedirectGenericArguments { get; }
    }
}