namespace CVB.NET.DataAccess.Sql.T4Queries.Model.SubModels
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Base;

    public interface ITableDescription : ITypeScopedDescription
    {
        string Schema { get; }
        string Table { get; }
        ImmutableList<IColumnDescription> Columns { get; }
        IEnumerable<KeyValuePair<string, List<IColumnDescription>>> ForeignKeyDescriptionsGroupedByTable { get; }
    }
}