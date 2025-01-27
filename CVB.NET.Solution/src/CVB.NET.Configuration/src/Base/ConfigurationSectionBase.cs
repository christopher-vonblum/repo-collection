using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace CVB.NET.Configuration.Base
{
    using System;
    using System.Configuration;

    [Serializable]
    [DebuggerDisplay("Section Name = {this.SectionInformation.Name}")]
    public class ConfigurationSectionBase : ConfigurationSection, IConfigurationElement, ISerializable
    {
        private Guid Guid = Guid.NewGuid();
        public ConfigurationSectionBase()
        {
            
        }

        protected ConfigurationSectionBase(SerializationInfo info, StreamingContext context)
        {
            XmlReader reader = new XmlTextReader(new StringReader(info.GetString("serialized")));

            this.DeserializeSection(reader);

            reader.Dispose();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            string serialized = this.SerializeSection(this, "customCompilation", ConfigurationSaveMode.Full);

            info.AddValue("serialized", serialized);
        }

        public virtual new object this[string propertyName]
        {
            get { return  base[propertyName]; }
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

        /// <summary>
        /// Gibt eine Zeichenfolge zurück, die das aktuelle Objekt darstellt.
        /// </summary>
        /// <returns>
        /// Eine Zeichenfolge, die das aktuelle Objekt darstellt.
        /// </returns>
        public override string ToString()
        {
            return Guid.ToString("B");
        }
    }
}