namespace CVB.NET.Abstractions.Ioc.Container.Activator
{
    using Container;
    using Model;

    public interface IServiceActivator
    {
        object CreateInstance(IReadOnlyIocContainer sourceContainer, IServiceRegistration component);
    }
}