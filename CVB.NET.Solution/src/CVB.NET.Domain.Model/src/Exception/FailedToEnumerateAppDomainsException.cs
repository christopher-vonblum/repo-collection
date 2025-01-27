namespace CVB.NET.Domain.Model.Exception
{
    using Base;

    public class FailedToEnumerateAppDomainsException : AppDomainProviderExceptionBase
    {
        public FailedToEnumerateAppDomainsException(System.Exception exception) : base(exception)
        {
        }
    }
}