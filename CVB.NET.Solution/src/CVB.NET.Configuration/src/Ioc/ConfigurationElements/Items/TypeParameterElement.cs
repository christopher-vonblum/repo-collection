namespace
    CVB.NET.Configuration.Ioc.ConfigurationElements.Items
{
    using System;
    using System.Diagnostics;
    using Base;

    [DebuggerDisplay("{Type.FullName}")]
    public class TypeParameterElement : ConfigurationElementBase
    {
        public Type Type { get; set; }
    }
}