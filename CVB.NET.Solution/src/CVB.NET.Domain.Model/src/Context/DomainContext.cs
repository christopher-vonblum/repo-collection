namespace CVB.NET.Domain.Model.Context
{
    using Base;
    using Rewriting;

    [ImplementStaticClassProxyAspect]
    public static class DomainContext
    {
        public static IDomainHost DomainHost { get; }
    }
}