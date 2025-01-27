using System;

namespace CVB.NET.Rewriting.Compiler.Ioc.Service
{
    public class ServiceAssemblyAttribute : Attribute
    {
        public Type[] InstallerTypes { get; }
        public ServiceAssemblyAttribute(params Type[] installerTypes)
        {
            this.InstallerTypes = installerTypes;
        }
    }
}
