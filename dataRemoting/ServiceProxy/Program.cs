using System;
using Microsoft.Extensions.DependencyInjection;
using PromiseService;

namespace ServiceProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            // when implementing service proxy support tail calls
            // when transmitting enumerable results asynchronously read with zeroformatter from redis and use LazyDictionary and IEnumerbale to yield the results
            IServiceCollection rootCollection = new ServiceCollection();

            rootCollection.AddSingleton<Func<>>();
            rootCollection.AddSingleton<IDomainServiceResolverService>();

            rootCollection.Add(ServiceDescriptor.Describe(typeof(), typeof(), ServiceLifetime.Transient));
        }
    }

    public interface IDistributedService
    {
        
    }
}