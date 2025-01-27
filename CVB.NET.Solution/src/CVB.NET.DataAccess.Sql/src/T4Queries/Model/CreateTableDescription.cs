namespace CVB.NET.DataAccess.Sql.T4Queries.Model
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Data.SqlClient;
    using System.Linq;
    using Base;
    using MetaData;
    using PostSharp.Patterns.Contracts;
    using SubModels;

    public class CreateTableDescription : TableScopedQueryDescription, ITableDescription
    {
        public List<IColumnDescription> ColumnDescriptions { get; }

        public CreateTableDescription(
            [NotNull] OrmSqlTypeMetaDataInfoView ormType,
            SqlConnection sqlConnection = null) : base(ormType, sqlConnection)
        {
            ColumnDescriptions =
                ormType
                    .PrimitiveColumnDescriptions.Concat(ormType.TablePrimaryKeyDescriptions)
                    .ToList();

            ColumnDescriptions = (from column in ColumnDescriptions
                                  orderby column.IsPrimaryKey && column.IsForeignKey descending,
                                      column.IsPrimaryKey descending,
                                      column.IsForeignKey descending,
                                      column.Name
                                  select column)
                .ToList();

            ForeignKeyDescriptionsGroupedByTable =
                ormType.TableForeignKeyDescriptions
                    .GroupBy(col => col.ReferenceTable)
                    .ToDictionary(k => k.Key, v => v.ToList());
        }

        ImmutableList<IColumnDescription> ITableDescription.Columns
        {
            get { return ColumnDescriptions.ToImmutableList(); }
        }

        public IEnumerable<KeyValuePair<string, List<IColumnDescription>>> ForeignKeyDescriptionsGroupedByTable { get; }
    }
}