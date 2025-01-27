namespace CoreUi.Tests.Proxy
{
    public interface IProxyTestModel : IProxyTestModelWithoutSelfReference
    {
        IProxyTestModel C { get; set; }
    }
}