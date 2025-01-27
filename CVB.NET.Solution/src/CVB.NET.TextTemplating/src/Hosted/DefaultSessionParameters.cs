namespace CVB.NET.TextTemplating.Hosted
{
    using System.Collections.Generic;
    using Aspects.Patterns.AutoDictionaryWrapper;
    using Microsoft.VisualStudio.TextTemplating;

    [AutoDictionaryWrapperAspect(keyPrefix: "_")]
    public class DefaultSessionParameters : IAutoDictionaryWrapper
    {
        public string PhysicalRootPath { get; }

        public TextTemplatingSession Session { get; }

        public DefaultSessionParameters(TextTemplatingSession session)
        {
            WrappedDictionary = session;
        }

        public IDictionary<string, object> WrappedDictionary { get; }
    }
}