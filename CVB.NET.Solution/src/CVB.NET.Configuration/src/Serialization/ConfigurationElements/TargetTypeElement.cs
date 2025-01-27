namespace CVB.NET.Configuration.Serialization.ConfigurationElements
{
    using Configuration.Base;

    public class TargetTypeElement : ConfigurationElementBase, ITargetTypeElement
    {
        public string Type { get; }
        public string Type2 { get; }
    }
}