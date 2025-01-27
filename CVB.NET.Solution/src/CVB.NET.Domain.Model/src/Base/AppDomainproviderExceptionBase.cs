namespace CVB.NET.Domain.Model.Base
{
    public class AppDomainProviderExceptionBase : System.Exception
    {
        public AppDomainProviderExceptionBase()
        {
        }

        public AppDomainProviderExceptionBase(System.Exception innerException) : base(string.Empty, innerException)
        {
        }
    }
}