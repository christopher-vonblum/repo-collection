using System;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace PromiseService
{
    public interface IRadServiceDomain
    {
        void Initialize(IServiceCollection environmentCollection);
    }
    
    /// <summary>
    /// Can be used to store information about awaited data from external data sources and invoke the continuation action when the data actually arrived.
    /// Multiple slaves can be configured to resolve the promises when data arrives.
    /// </summary>
    public interface IPromiseDistributionService
    {
        void CreatePromise(Guid promiseId, TimeSpan expiration, Expression<Action<Guid, IServiceProvider, object>> resolveDelegate);
        void ResolvePromise(Guid promiseId, object data);
    }
}