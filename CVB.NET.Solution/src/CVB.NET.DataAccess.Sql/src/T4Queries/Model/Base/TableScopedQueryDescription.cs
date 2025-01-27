namespace CVB.NET.DataAccess.Sql.T4Queries.Model.Base
{
    using System.Data.SqlClient;
    using MetaData;
    using PostSharp.Patterns.Contracts;

    public class TableScopedQueryDescription : SchemaScopedQueryDescription, ITypeScopedDescription
    {
        public string Table { get; set; }

        public TableScopedQueryDescription([NotNull] OrmSqlTypeMetaDataInfoView ormInfoType, SqlConnection connection = null, string database = null, string schema = null) : base(database, schema, connection)
        {
            Table = ormInfoType.TableName;
            OrmTypeMetaDataInfoView = ormInfoType;
        }

        public OrmSqlTypeMetaDataInfoView OrmTypeMetaDataInfoView { get; }
    }
}