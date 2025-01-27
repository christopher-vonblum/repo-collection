namespace CVB.NET.Debugging.Proxies
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using PostSharp.Patterns.Contracts;

    public sealed class EnumerableDebugView<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items => collection.ToArray();

        private IEnumerable<T> collection;

        public EnumerableDebugView([NotNull] IEnumerable<T> collection)
        {
            this.collection = collection;
        }
    }
}