namespace CVB.NET.DataAccess.Sql
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Data.SqlClient;
    using PostSharp.Patterns.Contracts;
    using Repository.GenericModel;
    using T4Queries.Model;
    using T4Queries.Model.MetaData;
    using T4Queries.Model.SubModels;

    public interface ISqlCommandProvider
    {
        ImmutableList<ITableDescription> GetDependantTableDescriptions(OrmSqlTypeMetaDataInfoView ormType);
        ITableDescription GetTableDescription(OrmSqlTypeMetaDataInfoView ormType);
        InsertRecordDescription GetInsertRecordDescription(IGenericModel<OrmSqlTypeMetaDataInfoView> model);
        SqlCommand GetCreateRecordCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model);
        SqlCommand GetCreateTableDependenciesCommand([NotNull] IList<ITableDescription> tableDescriptions);
        SqlCommand GetCreateTableForeignConstraintsCommand([NotNull] IList<ITableDescription> tableDescription);
        SqlCommand GetCreateTableCommand(ITableDescription tableDescription);
        SqlCommand GetDeleteCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model);
        SqlCommand GetExistsRecordCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model);
        SqlCommand GetExistsTableCommand(OrmSqlTypeMetaDataInfoView type);
        SqlCommand GetReadCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model);
        SqlCommand GetUpdateCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model);
        SqlCommand GetDeleteTableCommand(OrmSqlTypeMetaDataInfoView getCachedTypeBoundMetaDataInfo);
        SqlCommand GetSingleTableInsertRecordCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model);
    }
}