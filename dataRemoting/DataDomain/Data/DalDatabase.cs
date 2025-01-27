using System;
using MongoDB.Driver;

namespace DataDomain
{
    class DalDatabase : DalTreeProjectionRepository, IDalDatabase
    {
        public IIdentityProjectionRepository<string> Repositories { get; private set; }
        
        public DalDatabase(IServiceProvider serviceProvider, IMongoClient client, string database) : base(serviceProvider,null, client.GetDatabase(database), "tree_root")
        {
            
        }

        public static IDalDatabase NewDatabase(IServiceProvider activationServiceProvider, IMongoClient client, string database)
        {
            var db = new DalDatabase(activationServiceProvider, client, database);
            
            db.Repositories = new DalTreeProjectionRepository(activationServiceProvider, db, client.GetDatabase(database), "tree_root.repository_repositories");
            db.Repositories.Create(new AggregationRepositoryDefinition
            {
                Path = "/repositories/entitytypes",
                CollectionName = "entitytypes",
                SourcingPath = "/entitytypes",
            });
            db.Repositories.Create(new AggregationRepositoryDefinition
            {
                Path = "/repositories/clrtypes",
                CollectionName = "clrtypes",
                SourcingPath = "/clrtypes"
            });
            db.Repositories.Create(new MountedRepositoryDefinition
            {
                Path = "/repositories/mount",
                SourcingPath = "/mount",
                ActivationExpression = ""
            });

            IRepository typeRepo = (IRepository)db.Repositories.Activate("/repositories/entitytypes");
            
            
            
            return db;
        }
    }
}