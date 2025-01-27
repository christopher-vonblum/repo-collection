namespace CVB.NET.Abstractions.Ioc.Container.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class ExecutionContext<T, TOuter> : IExecutionContext<T, TOuter> where T : class, TOuter, new() where TOuter : class
    {
        private ThreadLocal<Stack<TOuter>> environmentStorage = new ThreadLocal<Stack<TOuter>>(() => new Stack<TOuter>());

        public virtual TOuter CurrentEnvironment
        {
            get
            {
                if (!this.environmentStorage.Value.Any())
                {
                    this.environmentStorage.Value.Push(new T());
                }

                return this.environmentStorage.Value.Peek();
            }
        }

        public IDisposable EnvironmentSwitch(TOuter newEnvironment)
        {
            this.environmentStorage.Value.Push(newEnvironment);

            return new UsableContext(
                () =>
                    {
                        if (this.environmentStorage.Value.Peek() != newEnvironment)
                        {
                            throw new InvalidOperationException();
                        }

                        this.environmentStorage.Value.Pop();

                        if (newEnvironment is IDisposable)
                        {
                            ((IDisposable)newEnvironment).Dispose();
                        }
                    });
        }

        public virtual IDisposable EnvironmentOverride(TOuter overrideEnvironment)
        {
            return this.EnvironmentSwitch(overrideEnvironment);
        }
    }
}