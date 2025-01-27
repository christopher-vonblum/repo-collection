namespace
    CVB.NET.Configuration.Ioc.ConfigurationElements.Groups
{
    using Base;
    using Items;

    public class ConstructorElement : ConfigurationElementBase
    {
        public ConfigurationElementCollection<ArgumentElement> Arguments { get; set; }
    }
}