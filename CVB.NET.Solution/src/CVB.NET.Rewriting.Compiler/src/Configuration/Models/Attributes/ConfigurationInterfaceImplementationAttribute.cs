namespace CVB.NET.Rewriting.Compiler.Configuration.Models.Attributes
{
    using System;

    public class ConfigurationInterfaceImplementationAttribute : Attribute
    {
        public Type ImplementationType { get; }

        public ConfigurationInterfaceImplementationAttribute()
        {
        }
        public ConfigurationInterfaceImplementationAttribute(string implementationType)
        {
            ImplementationType = Type.GetType(implementationType);
        }

        public ConfigurationInterfaceImplementationAttribute(Type implementationType)
        {
            ImplementationType = implementationType;
        }

    }
}
