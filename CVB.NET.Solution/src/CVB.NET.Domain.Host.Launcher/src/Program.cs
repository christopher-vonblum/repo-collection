namespace CVB.NET.Domain.Host.Launcher
{
    using System;
    using System.Configuration;
    using Configuration;
    using Model.Base;
    using Reflection.Caching.Cached;

    internal class Program
    {
        private static void Main()
        {
            DomainHostLauncherConfigurationSection launcherConfiguration = (DomainHostLauncherConfigurationSection) ConfigurationManager.GetSection("launcherConfiguration");

            IDomainHost host =
                (IDomainHost) ((CachedType) launcherConfiguration.DomainHostImplementationType).DefaultConstructor.InnerReflectionInfo
                    .Invoke(null);

            host.Initialize();

            Console.ReadLine();
        }
    }
}