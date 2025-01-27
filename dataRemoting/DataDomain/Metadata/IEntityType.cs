using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataDomain
{
    public interface IEntityType
    {
        IList<IClrType> DataSegments { get; set; }
        
        IDictionary<string, Expression<Func<IEntityProxy, Expression<Func<IEntityProxy, object>>[]>>> IdentityGroups { get; set; }
        
        Expression<Func<IServiceProvider, IEntityType, object>> ProxyFactoryExpression { get; set; }
        
        Expression<Func<IServiceProvider, IEntity, object>> ComponentActivationExpression { get; set; }
    }
}