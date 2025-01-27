namespace CVB.NET.Domain.Model.Exception
{
    using System;
    using Base;

    public class AppDomainNotFoundException : AppDomainProviderExceptionBase
    {
        public AppDomainNotFoundException(string domainPath)
        {
            throw new NotImplementedException();
        }
    }
}