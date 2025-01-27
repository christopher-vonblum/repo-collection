namespace CVB.NET.Abstractions.Ioc.Container.Lifestyle
{
    using Model;

    public interface IServiceInstanceContainer
    {
        object GetInstance(IServiceRegistration serviceKey);

        void Release(IServiceRegistration serviceKey);
    }
}