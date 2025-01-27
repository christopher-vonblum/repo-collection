using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;

namespace CVB.NET.Architecture.Facades
{
    public class DefaultDependencyFacadeSetup : IDependencySetup
    {
        public void InstallInto(IDependencyInstaller dependencyInstaller)
        {
            dependencyInstaller.Register(Service.For<IFacadeCache, IocFacadeCache>());
            dependencyInstaller.Register(Service.For<IFacadeProxyGenerator, FacadeProxyGenerator>());
        }
    }
}
