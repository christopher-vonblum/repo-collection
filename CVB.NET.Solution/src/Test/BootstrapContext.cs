using CVB.NET.Configuration.Ioc;
using CVB.NET.DataAccess.Repository;
using CVB.NET.DataAccess.Sql.T4Queries.Model.MetaData.Views;
using CVB.NET.DataAccess.Sql.TreeModelRepository.Schema;
using CVB.NET.Ioc.Aspects;

namespace Test
{
    [IocAccessorAspect(typeof(AppConfigIocProvider),
        resolveNamedSingletons: true,
        resolveDefaultSingletons: false,
        factoryMode: false,
        iocProviderConstructorParams: "bootstrap")]
    public static class BootstrapContext
    {
        public static IQueryableCrudRepository<ModelBase, OrmSqlTypeMetaDataView> BinaryTreeDataSource { get; }
    }
}
