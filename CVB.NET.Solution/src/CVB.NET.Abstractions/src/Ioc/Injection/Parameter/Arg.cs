namespace CVB.NET.Abstractions.Ioc.Injection.Parameter
{
    using System;
    using System.Collections.Generic;

    using CVB.NET.Abstractions.Ioc.Base;

    public static class Arg<TArg>
    {
        public static TArg Dependency() => (TArg)Arg.Resolve(typeof(TArg));
        public static TArg Dependency(string dependencyName) => (TArg)Arg.Resolve(typeof(TArg), dependencyName);
    }

    public static class Arg
    {
        public static Func<Type, string, object> CurrentResolver => resolverStack.Peek();

        [ThreadStatic]
        private static Stack<Func<Type, string, object>> resolverStack;

        internal static IDisposable UseContextualResolver(IDependencyResolver container)
        {
            return UseContextualResolver((type, name) => container.Resolve(type, name));
        }

        internal static IDisposable UseContextualResolver(Func<Type, string, object> resolveArgument)
        {
            if (resolverStack == null)
            {
                resolverStack = new Stack<Func<Type, string, object>>();
            }

            resolverStack.Push(resolveArgument);

            return new ArgContainerContext(() => resolverStack.Pop());
        }

        internal static object Resolve(Type tService)
        {
            return CurrentResolver(tService, null);
        }

        internal static object Resolve(Type tService, string dependencyName)
        {
            return CurrentResolver(tService, dependencyName);
        }

        public static object Dependency(Type tService) => Resolve(tService);
        public static object Dependency(Type tService, string dependencyName) => Resolve(tService, dependencyName);
    }
}