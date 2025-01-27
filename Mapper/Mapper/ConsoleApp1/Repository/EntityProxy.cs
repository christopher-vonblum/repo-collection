using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using ConsoleApp1.Repository;
using ZeroFormatter;

namespace ConsoleApp1
{
    public class EntityProxy<TId> : ObjectProxy, IEntityProxy<TId>
    {
        public IDictionary<TId, object> TrackedAggregates { get; } = new ConcurrentDictionary<TId, object>();
        public IRepository<TId> Repository { get; set; }

        public override object ConvertToReturnValue(Type propertyType, object value)
        {
            if (typeof(IEntity<TId>).IsAssignableFrom(propertyType))
            {
                var id = ZeroFormatterSerializer.Deserialize<TId>(ZeroFormatterSerializer.Deserialize<O>((byte[]) value)
                    .Properties[
                        "ConsoleApp1.Repository.IEntity`1[[System.String, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]].Id"]);
                
                return ((ConcurrentDictionary<TId, Object>)TrackedAggregates).GetOrAdd(id, Repository.Read(propertyType, id));
            }
            
            return base.ConvertToReturnValue(propertyType, value);
        }

        public override object ConvertToInternalValue(Type propertyType, object value)
        {
            if (typeof(IEntity<TId>).IsAssignableFrom(propertyType))
            {
                IEntity<TId> entity = (IEntity<TId>)value;

                return entity;
            }
            
            return base.ConvertToInternalValue(propertyType, value);
        }
    }
}