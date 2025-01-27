namespace CVB.NET.Domain.Host.Launcher.Configuration
{
    using System;
    using NET.Configuration.Base;

    public class DomainHostLauncherConfigurationSection : ConfigurationSectionBase
    {
        public Type DomainHostImplementationType { get; set; }
    }
}