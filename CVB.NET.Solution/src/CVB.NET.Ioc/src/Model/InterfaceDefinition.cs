namespace CVB.NET.Ioc.Model
{
    using System;
    using System.Diagnostics;
    using PostSharp.Patterns.Contracts;

    [DebuggerDisplay("{SingletonKey}, {InterfaceType.FullName}")]
    public struct InterfaceDefinition
    {
        public string SingletonKey { get; set; }

        public Type InterfaceType { get; }

        public bool IsDefaultDefinition => string.Empty.Equals(SingletonKey);

        public InterfaceDefinition([NotNull] Type interfaceType)
        {
            InterfaceType = interfaceType;

            SingletonKey = null;
        }
    }
}