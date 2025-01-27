using System;
using System.Net;

namespace PromiseService
{
    public interface IDomainServiceResolverService
    {
        IPEndPoint ResolveService(string domainRole, Type serviceType);
    }
}