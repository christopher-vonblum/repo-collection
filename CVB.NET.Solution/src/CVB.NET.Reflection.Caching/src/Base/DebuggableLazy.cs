namespace CVB.NET.Reflection.Caching.Base
{
    using System;
    using PostSharp.Patterns.Contracts;

    public class DebuggableLazy<T>
    {
        public T Value
        {
            get
            {
                if (!valueCreated && factory != null)
                {
                    storage = factory();
                    valueCreated = true;
                }

                return storage;
            }
        }

        private Func<T> factory;

        private T storage;
        private bool valueCreated = false;

        public DebuggableLazy([NotNull] Func<T> factory)
        {
            this.factory = factory;
        }

        public DebuggableLazy(T value)
        {
            storage = value;
            valueCreated = true;
        }
    }
}