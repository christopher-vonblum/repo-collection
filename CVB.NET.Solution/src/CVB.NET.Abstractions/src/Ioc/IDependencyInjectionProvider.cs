namespace CVB.NET.Abstractions.Ioc
{
    public interface IDependencyInjectionProvider
    {
        void InjectDependencies(object target);
    }
}