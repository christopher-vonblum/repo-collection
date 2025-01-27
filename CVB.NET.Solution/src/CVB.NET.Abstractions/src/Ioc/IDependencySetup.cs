namespace CVB.NET.Abstractions.Ioc
{
    public interface IDependencySetup
    {
        void InstallInto(IDependencyInstaller dependencyInstaller);
    }
}