namespace CVB.NET.Configuration.Ioc.ConfigurationElements.Elements
{
    using Attributes;
    using Base;
    using Groups;
    using NET.Ioc.Model;

    public class DependencyElement : ConfigurationElementBase
    {
        public string Id { get; set; }

        public InstanceLifeStyle LifeStyle { get; set; }

        public TypeDefinitionElement InterfaceType { get; set; }

        [RequiredProperty]
        public TypeDefinitionElement ImplementationType { get; set; }

        public ConstructorElement Constructor { get; set; }

        public InjectPropertiesElement InjectProperties { get; set; }
    }
}