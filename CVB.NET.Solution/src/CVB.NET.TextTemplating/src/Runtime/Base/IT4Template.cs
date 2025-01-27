namespace CVB.NET.TextTemplating.Runtime.Base
{
    using System.Collections.Generic;
    using PostSharp.Patterns.Contracts;

    public interface IT4Template
    {
        string Process();
        string Process([NotNull] IDictionary<string, object> arguments);
    }

    public interface IT4Template<in TParameterType> : IT4Template where TParameterType : class
    {
        string Process([NotNull] TParameterType argumentObject);
    }
}