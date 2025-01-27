namespace CVB.NET.Abstractions.Ioc.Container.Registration.Extension
{
    using System.Collections.Generic;

    using CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base;

    public class NamedInstanceExtension : RegistrationExtensionBase
    {
        public string Name { get; }

        public NamedInstanceExtension(string name)
        {
            this.Name = name;
        }
        
        public override IReadOnlyDictionary<string, string> GetInstanceModifiers() => new Dictionary<string, string> { {"name", this.Name } };
    }
}