using System;
using System.Linq.Expressions;

namespace PromiseService
{
    public class RequestModel
    {
        public Guid RequestId { get; set; }
        public object Request { get; set; }
        public Expression<Func<IServiceProvider, Guid, object, object>> DoProcessRequest { get; set; }
    }
}