namespace CVB.NET.Architecture.Tests.Facades
{
    using Architecture.Facades;
    using TestComponents;

    public interface ITestFacade : IFacadeBase
    {
        ISubFacade SubFacade { get; }
        IComponent1 Component1 { get; }
        IComponent2 Component2 { get; }
    }
}