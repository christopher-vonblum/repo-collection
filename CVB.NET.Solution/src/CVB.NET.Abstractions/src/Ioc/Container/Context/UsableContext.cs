namespace CVB.NET.Abstractions.Ioc.Container.Context
{
    using System;

    /// <summary>
    /// Genral wrapper pattern for usage with "using"
    /// </summary>
    public class UsableContext : IDisposable
    {
        private readonly Action onDispose;

        public UsableContext(Action onDispose)
        {
            this.onDispose = onDispose;
        }

        public void Dispose()
        {
            this.onDispose?.Invoke();
        }
    }
}