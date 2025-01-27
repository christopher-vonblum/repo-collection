namespace CVB.NET.DataAccess.Sql.T4Queries.Model.Base
{
    using MetaData;

    public interface ITypeScopedDescription
    {
        OrmSqlTypeMetaDataInfoView OrmTypeMetaDataInfoView { get; }
    }
}