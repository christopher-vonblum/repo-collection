namespace CVB.NET.Abstractions.Adapters.Tests.res
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class NamedDependencyAttribute : Attribute
    {
        public string Name { get; }

        /// <summary>
        /// Use the parameter name as resolving key.
        /// </summary>
        public NamedDependencyAttribute()
        {
        }

        /// <summary>
        /// Use the custom name as resolving key.
        /// </summary>
        public NamedDependencyAttribute(string name)
        {
            Name = name;
        }
    }
}