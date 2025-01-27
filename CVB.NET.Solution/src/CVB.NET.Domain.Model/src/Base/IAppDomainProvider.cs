namespace CVB.NET.Domain.Model.Base
{
    using System;

    public interface IAppDomainProvider
    {
        AppDomain GetRootDomain();
        AppDomain GetParentDomain(AppDomain childDomain);
        bool HasChildDomain(AppDomain parentDomain, string domainName);
        AppDomain CreateChildDomain(AppDomain parentDomain, string domainName);
        AppDomain GetChildDomain(AppDomain parentDomain, string domainName);
        AppDomain GetDomain(string domainPath);
    }
}