using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVB.NET.Architecture.src.Context
{
    using System.Collections.Concurrent;

    public static class ExecutionContext<TContext> where TContext : class, IExecutionContext, new()
    {
        public static TContext Current => GetCurrent();

        public static void Override(TContext context)
        {
            ContextOverrides.Push(context);   
        }


        [ThreadStatic] private static Stack<TContext> contextOverrides;

        private static Stack<TContext> ContextOverrides => contextOverrides ?? (contextOverrides = new Stack<TContext>());

        private static TContext GetCurrent()
        {
            if (ContextOverrides.Count > 0)
            {
                TContext context = ContextOverrides.Peek();

                context.OnDestroy += ContextOnOnDestroy;

                return context;
            }

            return new TContext();
        }

        private static void ContextOnOnDestroy(IExecutionContext executionContext)
        {
            executionContext.OnDestroy -= ContextOnOnDestroy;

            if (ContextOverrides.Count > 0 && executionContext == ContextOverrides.Peek())
            {
                ContextOverrides.Pop();
            }

            throw new InvalidOperationException();
        }
    }
}
