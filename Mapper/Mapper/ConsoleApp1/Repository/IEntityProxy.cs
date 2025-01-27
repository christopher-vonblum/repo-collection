using System.Collections.Generic;
using ConsoleApp1.Repository;

namespace ConsoleApp1
{
    public interface IEntityProxy<TId> : IObjectProxy
    {
        IDictionary<TId, object> TrackedAggregates { get; }
        IRepository<TId> Repository { get; set; }
    }
}