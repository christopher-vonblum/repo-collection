namespace CVB.NET.Domain.Model.Exception
{
    using Base;

    public class DuplicateAppDomainsFoundException : AppDomainProviderExceptionBase
    {
        public string DomainPath { get; }

        public DuplicateAppDomainsFoundException(string domainPath)
        {
            DomainPath = domainPath;
        }
    }
}