namespace
    CVB.NET.Configuration.Ioc.ConfigurationElements.Groups
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Attributes;
    using Base;
    using Items;

    [DebuggerDisplay("{Type.FullName}<{GenericDisplayString}>")]
    public class TypeDefinitionElement : ConfigurationElementBase
    {
        [RequiredProperty]
        public Type Type { get; set; }

        private string GenericDisplayString => string.Join(",", Arguments.Select(arg => arg.Type.FullName));

        public ConfigurationElementCollection<TypeParameterElement> Arguments { get; set; }
    }
}