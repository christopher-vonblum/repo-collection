namespace CVB.NET.Architecture.src.Context
{
    using System;

    public interface IExecutionContext : IDisposable
    {
        event Action<IExecutionContext> OnDestroy;
    }
}