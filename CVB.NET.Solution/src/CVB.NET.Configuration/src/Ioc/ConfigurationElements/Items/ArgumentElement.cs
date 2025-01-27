namespace
    CVB.NET.Configuration.Ioc.ConfigurationElements.Items
{
    using System;
    using System.Diagnostics;
    using Attributes;
    using Base;
    using Groups;

    [DebuggerDisplay("{Name} = {Value}")]
    public class ArgumentElement : ConfigurationElementBase
    {
        [IdentifierProperty]
        public string Name { get; set; }

        public string Value { get; set; }

        public string InjectId { get; set; }

        public Type InjectType { get; set; }

        public ConstructorElement Inject { get; set; }
    }
}