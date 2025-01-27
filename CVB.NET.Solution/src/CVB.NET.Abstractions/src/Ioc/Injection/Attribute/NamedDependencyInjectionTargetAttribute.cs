namespace CVB.NET.Abstractions.Ioc.Injection.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class NamedDependencyInjectionTargetAttribute : DependencyInjectionTargetAttribute
    {
        public string ServiceName { get; }

        public NamedDependencyInjectionTargetAttribute(string serviceName)
        {
            this.ServiceName = serviceName;
        }
    }
}