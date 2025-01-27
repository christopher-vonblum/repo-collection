namespace CVB.NET.Abstractions.Ioc.Injection.Parameter
{
    using System;

    public class ArgContainerContext : IDisposable
    {
        private Action onDispose;

        public ArgContainerContext(Action onDispose)
        {
            this.onDispose = onDispose;
        }

        public void Dispose()
        {
            this.onDispose?.Invoke();
        }
    }
}