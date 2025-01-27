namespace CVB.NET.Abstractions.Ioc.Injection.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DependencyInjectionTargetAttribute : Attribute
    {
    }
}
