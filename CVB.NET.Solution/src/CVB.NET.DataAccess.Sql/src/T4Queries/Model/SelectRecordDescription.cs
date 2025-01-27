namespace CVB.NET.DataAccess.Sql.T4Queries.Model
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Base;
    using SubModels;

    public class SelectRecordDescription : SchemaScopedQueryDescription
    {
        public List<string> Tables { get; }

        public List<ColumnValue> Keys { get; }

        public List<IColumnDescription> Values { get; }

        public SelectRecordDescription(string database, string schema, List<string> tables, List<ColumnValue> keyProperties, List<IColumnDescription> valueProperties, SqlConnection connection = null)
            : base(schema, database, connection)
        {
            Tables = tables;
            Keys = keyProperties;
            Values = valueProperties;
        }
    }
}