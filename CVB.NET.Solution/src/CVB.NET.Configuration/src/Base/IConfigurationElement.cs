namespace CVB.NET.Configuration.Base
{
    using Aspects;
    using NET.Aspects.Validation.BuildTime;
    using PostSharp.Patterns.Contracts;

    [ConfigurationElementAspect]
    [ConfigurationPropertyAspectProvider]
    [RequiresConstructorContract]
    public interface IConfigurationElement
    {
        object this[[NotNull] string propertyName] { get; set; }
        IConfigurationElement ProxyTarget { get; set; }
        object GetProperty(string propertyName);
        void SetProperty(string propertyName, object value);
    }
}