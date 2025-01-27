using System;

namespace CVB.NET.Rewriting.Compiler.Configuration.Models.ServiceAssembly
{
    using NET.Configuration.Base;

    [Serializable]
    public class ServiceAssemblyReferenceConfigurationElement : ConfigurationElementBase, IServiceAssemblyReferenceConfiguration
    {
        public string FullName { get; set; }
    }
}