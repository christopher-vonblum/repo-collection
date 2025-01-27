namespace CVB.NET.DataAccess.Sql.T4Queries.Model
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Base;
    using SubModels;

    public class InsertRecordDescription : SchemaScopedQueryDescription
    {
        public IDictionary<string, List<ColumnValue>> TableValues { get; }

        public InsertRecordDescription(List<ColumnValue> values, string schema, string database, SqlConnection connection = null)
            : base(schema, database, connection)
        {
            TableValues = values.GroupBy(group => group.ColumnDescription.DeclaringType.InnerReflectionInfo.GUID.ToString("B")).ToDictionary(key => key.Key, value => value.ToList());
        }
    }
}