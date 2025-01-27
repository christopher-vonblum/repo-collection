using System.Diagnostics;

namespace CVB.NET.Configuration.Base
{
    using System;
    using System.Configuration;

    [Serializable]
    [DebuggerDisplay("{GetType().Name}")]
    public class ConfigurationElementBase : ConfigurationElement, IConfigurationElement
    {
        private Guid Guid = Guid.NewGuid();
        
        public override string ToString()
        {
            return Guid.ToString("B");
        }

        public virtual new object this[string propertyName]
        {
            get { return base[propertyName]; }
            set { base[propertyName] = value; }
        }

        public IConfigurationElement ProxyTarget { get; set; }

        public virtual object GetProperty(string propertyName)
        {
            return this[propertyName];
        }

        public virtual void SetProperty(string propertyName, object value)
        {
            this[propertyName] = value;
        }
    }
}