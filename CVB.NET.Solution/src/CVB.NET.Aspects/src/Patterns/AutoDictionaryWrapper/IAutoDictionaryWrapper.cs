namespace CVB.NET.Aspects.Patterns.AutoDictionaryWrapper
{
    using System.Collections.Generic;

    public interface IAutoDictionaryWrapper
    {
        IDictionary<string, object> WrappedDictionary { get; }
    }
}