namespace CVB.NET.Abstractions.Ioc.Container.Registration
{
    using System;

    using CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base;
    using CVB.NET.Reflection.Caching.Cached;

    public interface IRegistrationInstructionProxy<TService> : IRegistrationInstructionProxy
    {
    }

    public interface IRegistrationInstructionProxy : IRegistrationInstruction, IDisposable
    {
        IRegistrationExtension[] Extensions { get; }

        Func<CachedType> GetImplementation { get; set; }
        
        void AddExtensions(params IRegistrationExtension[] extensions);
    }
}