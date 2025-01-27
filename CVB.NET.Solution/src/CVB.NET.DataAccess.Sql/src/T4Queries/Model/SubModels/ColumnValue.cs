namespace CVB.NET.DataAccess.Sql.T4Queries.Model.SubModels
{
    using System.Diagnostics;
    using PostSharp.Patterns.Contracts;

    [DebuggerDisplay("{Name} = {Value}")]
    public class ColumnValue : INamedColumn
    {
        public IColumnDescription ColumnDescription { get; }

        public object Value { get; }

        public ColumnValue([NotNull] IColumnDescription columnDescription, object value)
        {
            ColumnDescription = columnDescription;
            Value = value;
        }

        public string Name => ColumnDescription.Name;
    }
}