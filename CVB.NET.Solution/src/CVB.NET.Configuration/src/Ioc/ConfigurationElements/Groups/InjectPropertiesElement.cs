namespace
    CVB.NET.Configuration.Ioc.ConfigurationElements.Groups
{
    using Base;
    using Items;

    public class InjectPropertiesElement : ConfigurationElementBase
    {
        public ConfigurationElementCollection<ArgumentElement> InjectProperties { get; set; }
    }
}