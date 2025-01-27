namespace CVB.NET.Abstractions.Ioc.Provider.Model
{
    public interface IServiceInstance
    {
        IServiceInstanceKey Key { get; }
        object Instance { get; }
    }
}