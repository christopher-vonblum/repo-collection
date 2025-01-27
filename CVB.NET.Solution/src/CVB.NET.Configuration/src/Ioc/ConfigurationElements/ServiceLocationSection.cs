using System;
using System.Runtime.Serialization;

namespace CVB.NET.Configuration.Ioc.
    ConfigurationElements
{
    using Base;
    using Elements;

    [Serializable]
    public class ServiceLocationSection : ConfigurationSectionBase
    {
        public int[] ArrayTest { get; set; }
        public ConfigurationElementCollection<DependencyElement> Dependencies { get; set; }
        public ServiceLocationSection() { }
        protected ServiceLocationSection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }
    }
}