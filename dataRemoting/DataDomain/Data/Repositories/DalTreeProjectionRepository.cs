using System;
using MongoDB.Driver;

namespace DataDomain
{
    class DalTreeProjectionRepository : DalRepository, IIdentityProjectionRepository<string>
    {
        private readonly IDalDatabase _database;

        public DalTreeProjectionRepository(IServiceProvider activationServiceProvider, IDalDatabase database, IMongoDatabase db, string collectionName) : base(activationServiceProvider, db, collectionName)
        {
            _database = (this as IDalDatabase) ?? database;
        }

        public bool Exists(string identity)
        {
            return Exists(IdentityToEntity(identity));
        }

        public bool HasDataOf<T>(string identity)
        {
            return Exists(identity) && HasDataOf<T>(IdentityToEntity(identity));
        }

        public IEntity Activate(string entity)
        {
            return Activate(IdentityToEntity(entity));
        }

        public IEntity Read(string identity)
        {
            return Read(new DataEntity {Path = identity});
        }

        public void Delete(string identity)
        {
            Delete(IdentityToEntity(identity));
        }

        private static DataEntity IdentityToEntity(string identity)
        {
            return new DataEntity() {Path = identity};
        }
    }
}