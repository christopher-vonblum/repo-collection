namespace CVB.NET.Domain.Host.Context
{
    using Ioc.Aspects;
    using Model.Aspect;
    using Model.Base;
    using NET.Configuration.Ioc;

    [DomainServicesRemotingAccessorAspect]
    [IocAccessorAspect(typeof (AppConfigIocProvider),
        resolveNamedSingletons: true,
        resolveDefaultSingletons: false,
        factoryMode: false,
        iocProviderConstructorParams: "host")]
    public static class HostDomainContext
    {
        public static IAppDomainProvider AppDomainProvider { get; set; }
    }
}