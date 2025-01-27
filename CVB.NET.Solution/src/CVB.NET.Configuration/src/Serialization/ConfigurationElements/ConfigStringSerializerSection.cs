namespace CVB.NET.Configuration.Serialization.ConfigurationElements
{
    using System.Collections.Generic;
    using Configuration.Base;

    public class ConfigStringSerializerSection : ConfigurationSectionBase, IStringSerializerContainerConfiguration
    {
        public ConfigurationElementCollection<ConfigStringSerializerElement> Serializers { get; }

        IEnumerable<IStringSerializerConfiguration> IStringSerializerContainerConfiguration.Serializers => Serializers;
    }

    public interface IStringSerializerContainerConfiguration : IConfigurationElement
    {
        IEnumerable<IStringSerializerConfiguration> Serializers { get; }
    }

    public interface IStringSerializerConfiguration
    {
        string Type { get; }
        IEnumerable<ITargetTypeElement> TargetTypes { get; }
    }

    public interface ITargetTypeElement
    {
        string Type { get; }
        string Type2 { get; }
    }
}