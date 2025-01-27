namespace CVB.NET.Architecture.Tests.Facades
{
    using Architecture.Facades;
    using TestComponents;

    public interface ISubFacade : IFacadeBase
    {
        IComponent3 Component3 { get; }
    }
}