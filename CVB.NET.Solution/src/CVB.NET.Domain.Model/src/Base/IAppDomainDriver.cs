namespace CVB.NET.Domain.Model.Base
{
    using System;

    public interface IAppDomainDriver : IDisposable
    {
        string Name { get; }
        void Run();
        void Stop();
    }
}