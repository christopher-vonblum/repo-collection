namespace CVB.NET.Configuration.Base
{
    using System;
    using System.Configuration;

    [Serializable]
    public abstract class ConfigurationElementCollectionBase : ConfigurationElementCollection, IConfigurationElement
    {
        private Guid Guid = System.Guid.NewGuid();

        protected object GetInner(int index)
        {
            return BaseGet(index);
        }
        public virtual new object this[string propertyName]
        {
            get { return  base[propertyName]; }
            set
            {
                base[propertyName] = value;
            }
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

        public override string ToString()
        {
            return Guid.ToString();
        }
    }
}