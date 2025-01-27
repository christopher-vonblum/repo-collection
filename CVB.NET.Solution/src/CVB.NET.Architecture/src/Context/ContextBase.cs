namespace CVB.NET.Architecture.src.Context
{
    using System;

    public abstract class ContextBase : IExecutionContext
    {
        public void Dispose()
        {
            OnDestroy?.Invoke(this);
        }

        public event Action<IExecutionContext> OnDestroy;
    }
}