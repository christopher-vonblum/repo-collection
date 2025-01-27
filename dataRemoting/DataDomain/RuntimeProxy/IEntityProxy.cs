using System;
using System.Linq.Expressions;

namespace DataDomain
{
    public interface IEntityProxy
    {
        IEntityType EntityType { get; set; }
        DataEntity DataObject { get; set; }
        IServiceProvider ActivationServiceProvider { get; set; }
        void TryActivate();
    }
}