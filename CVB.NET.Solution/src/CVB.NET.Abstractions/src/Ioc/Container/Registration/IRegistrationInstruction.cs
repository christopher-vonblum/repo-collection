namespace CVB.NET.Abstractions.Ioc.Container.Registration
{
    using System.Collections.Generic;

    using Reflection.Caching.Cached;
    
    public interface IRegistrationInstruction
    {
        CachedType Service { get; }
        IReadOnlyDictionary<string, string> EnvironmentModifiers { get; }
        IReadOnlyDictionary<string, string> ServiceModifiers { get; }
        string ServiceKey { get; }
    }
}