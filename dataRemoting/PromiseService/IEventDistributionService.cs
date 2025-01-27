using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PromiseService
{
    public interface IEventDistributionService
    {
        void Raise(Guid eventSourceId, Guid eventId, object eventModel);
        IEnumerable<object> ProcessEvents(Guid subscriberId, Expression<Func<object, bool>> filter);
        void Confirm(Guid subscriberId, Guid eventId);
    }
}