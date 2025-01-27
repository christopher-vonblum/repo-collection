using System;
using System.Linq.Expressions;
using StackExchange.Redis;

namespace PromiseService
{
    class PromiseDistributionService : IPromiseDistributionService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public PromiseDistributionService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        
        public void CreatePromise(Guid promiseId, TimeSpan expiration, Expression<Action<Guid, IServiceProvider, object>> resolveDelegate)
        {
            throw new NotImplementedException();
        }

        public void ResolvePromise(Guid promiseId, object data)
        {
            throw new NotImplementedException();
        }

        class PromiseModel
        {
            public Guid Guid { get; set; }
            public Expression<Action<Guid, IServiceProvider, object>> OnResolved { get; set; }
        }
    }
}