namespace CVB.NET.DataAccess.Sql.Tests.Context
{
    using System.Data.SqlClient;
    using Configuration.Ioc;
    using Ioc.Aspects;
    using Repository;
    using Repository.GenericModel;
    using T4Queries.Model.MetaData;
    using TreeModelRepository.Schema;

    [IocAccessorAspect(typeof (AppConfigIocProvider),
        resolveNamedSingletons: true,
        resolveDefaultSingletons: false,
        factoryMode: false,
        iocProviderConstructorParams: "sqlTest")]
    public static class SqlTestContext
    {
        public static SqlConnection TestConnection { get; }
        public static IGenericModelMapper<OrmSqlTypeMetaDataInfoView> GenericModelMapper { get; }
        public static ISqlCommandProvider SqlCommandProvider { get; }
        public static IQueryableCrudRepository<ModelBase, OrmSqlTypeMetaDataInfoView> Core { get; }
    }
}