using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PromiseService
{
    /// <summary>
    /// Can bes used to store information about open requests by adding them to a queue.
    /// Multiple slaves can be configured to process these requests. Using filters slaves can be dedicated to specific tasks.
    /// </summary>
    public interface IRequestDistributionService
    {
        void Enqueue(RequestModel model);
        IEnumerable<RequestModel> ProcessRequests(Expression<Func<RequestModel, bool>> filter = null);
        void Respond(Guid requestId, object response);
    }
}