namespace CVB.NET.Ioc.Provider
{
    using System.Collections.Generic;
    using System.Threading;
    using PostSharp.Patterns.Contracts;

    public class IocSingletonInstanceContainer
    {
        private object SharedInstance { get; }

        private ThreadLocal<Stack<object>> threadContextStack => new ThreadLocal<Stack<object>>(() => new Stack<object>());

        public Stack<object> ThreadContextStack => threadContextStack.Value;

        public object ContextualInstance
        {
            get
            {
                if (ThreadContextStack.Count == 0)
                {
                    return SharedInstance;
                }

                return ThreadContextStack.Peek();
            }
        }

        public IocSingletonInstanceContainer([NotNull] object sharedInstance)
        {
            SharedInstance = sharedInstance;
        }
    }
}