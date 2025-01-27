using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ZeroFormatter;

namespace ConsoleApp1.Repository
{
    class MemoryRepository<TId> : IRepository<TId>
    {
        private Dictionary<string, byte[]> storage = new Dictionary<string, byte[]>();
        
        public void Create<T>(T entity) where T : IEntity<TId>
        {
            string key = entity.Id.ToString();
            
            byte[] data = ZeroFormatterSerializer.Serialize((O)((IObjectProxy)entity).Object);

            storage.Add(key, data);
        }

        public T Create<T>(TId id) where T : IEntity<TId>
        {
            string key = id.ToString();

            IEntityProxy<TId> proxy = (IEntityProxy<TId> )DispatchProxy.Create<T, EntityProxy<TId>>();
            
            proxy.Object = new O();
            proxy.Repository = this;
            
            T t = (T) proxy;

            t.Id = id;
            
            byte[] data = ZeroFormatterSerializer.Serialize((O)proxy.Object);

            storage.Add(key, data);
            
            return t;
        }

        public T Read<T>(TId id) where T : IEntity<TId>
        {
            string key = id.ToString();

            IEntityProxy<TId> proxy = (IEntityProxy<TId>)DispatchProxy.Create<T, EntityProxy<TId>>();
            
            T entity = (T) proxy;
            
            O data = ZeroFormatterSerializer.Deserialize<O>(storage[key]);

            proxy.Object = data;
            proxy.Repository = this;
            
            entity.Id = id;
            
            return entity;
        }

        private ConcurrentDictionary<Type, MethodInfo> genericOverloads = new ConcurrentDictionary<Type, MethodInfo>();
        
        public object Read(Type t, TId id)
        {
            return genericOverloads
                .GetOrAdd(typeof(TId), pt => this
                    .GetType()
                    .GetMethod(nameof(Read), BindingFlags.Instance | BindingFlags.Public, null, new[] {pt}, null)
                    .MakeGenericMethod(t))
                .Invoke(this, new[]{(object)id});
        }

        public void Update(IEntity<TId> entity)
        {
            string key = entity.Id.ToString();
            
            IEntityProxy<TId> e = (IEntityProxy<TId>)entity;
            
            foreach (KeyValuePair<TId, object> eTrackedAggregate in e.TrackedAggregates)
            {
                Update((IEntity<TId>)eTrackedAggregate.Value);
            }
            
            byte[] data = ZeroFormatterSerializer.Serialize((O)(e.Object));

            storage[key] = data;
        }

        public void Delete(TId id)
        {
            storage.Remove(id.ToString());
        }
    }
}