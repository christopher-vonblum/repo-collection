namespace CVB.NET.Configuration.Serialization.ConfigurationElements
{
    using System.Collections.Generic;
    using Configuration.Base;

    public class ConfigStringSerializerElement : ConfigurationElementBase, IStringSerializerConfiguration
    {
        public string Type { get; }

        public ConfigurationElementCollection<TargetTypeElement> TargetTypes { get; }

        IEnumerable<ITargetTypeElement> IStringSerializerConfiguration.TargetTypes => TargetTypes;
    }
}