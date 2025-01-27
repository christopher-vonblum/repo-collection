using System;
using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataDomain
{
    class DalRepository : DataEntity, IRepository, ISupportInitialize
    {
        private readonly IServiceProvider _activationServiceProvider;
        private readonly IMongoCollection<DataEntity> _collection;

        public DalRepository(IServiceProvider activationServiceProvider, IMongoDatabase db, string collectionName)
        {
            _activationServiceProvider = activationServiceProvider;
            _collection = db.GetCollection<DataEntity>(collectionName);
        }
        
        public bool Exists(IEntity entity)
        {
            return GetEntityByPathQuery(entity).Any();
        }

        private IFindFluent<DataEntity, DataEntity> GetEntityByPathQuery(IEntity entity)
        {
            return _collection.Find(GetEntityByPathFilter(entity));
        }

        private static FilterDefinition<DataEntity> GetEntityByPathFilter(IEntity entity)
        {
            return Builders<DataEntity>.Filter.Eq(e => e.Path, entity.Path);
        }

        public bool HasDataOf<TSegment>(IEntity entity)
        {
            return Exists(entity) && GetEntityByPathQuery(entity).Single().HasSegment<TSegment>();
        }

        public IEntityProxy CreateProxy(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Create(IEntity entity)
        {
            if (entity is IEntityProxy p)
            {
                entity = p.DataObject;
            }
            
            _collection.InsertOne((DataEntity)entity);
        }

        public IEntity Activate(IEntity entity)
        {
            if (entity is IEntityProxy p)
            {
                return (IEntity) p;
            }

            var info = entity.GetSegment<IRadActivationInformation>();

            return Activate(entity);
        }

        public IEntity Read(IEntity entity)
        {
            if (entity is IEntityProxy p)
            {
                entity = p.DataObject;
            }
            
            return GetEntityByPathQuery(entity).Single();
        }

        public void Update(IEntity entity)
        {
            if (entity is IEntityProxy p)
            {
                entity = p.DataObject;
            }
            
            _collection.UpdateOne(GetEntityByPathFilter(entity), new JsonUpdateDefinition<DataEntity>(entity.ToJson()));
        }

        public void Delete(IEntity entity)
        {
            _collection.DeleteOne(GetEntityByPathFilter(entity));
        }

        public virtual void BeginInit()
        {
        }

        public virtual void EndInit()
        {
        }
    }

    internal interface IRadActivationInformation
    {
        IEntityType EntityType { get; set; }
    }
}