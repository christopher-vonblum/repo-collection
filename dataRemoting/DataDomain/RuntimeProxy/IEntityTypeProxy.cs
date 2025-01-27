using System;
using System.Linq.Expressions;

namespace DataDomain
{
    public interface IEntityTypeProxy<T>
    {
        /// <summary>
        /// Define a property that gets evaluated when the property getter is called.
        /// DAL domains persistently caches the results of these properties.
        /// They can be also adressed when querying using a service proxy supplying a expresssion.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="evaluate"></param>
        /// <typeparam name="TP"></typeparam>
        void SetIndexProperty<TP>(Expression<Func<T, TP>> property, Expression<Func<T, TP>> evaluate);
    }
}