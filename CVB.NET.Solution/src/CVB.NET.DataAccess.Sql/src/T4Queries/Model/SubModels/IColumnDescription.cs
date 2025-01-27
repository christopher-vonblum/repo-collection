namespace CVB.NET.DataAccess.Sql.T4Queries.Model.SubModels
{
    using MetaData;
    using Reflection.Caching.Cached;

    public interface IColumnDescription : INamedColumn
    {
        CachedPropertyInfo PropertyInfo { get; }
        OrmSqlTypeMetaDataInfoView DeclaringType { get; }
        string Type { get; }

        bool IsPrimaryKey { get; }

        bool IsForeignKey { get; }
        string ReferenceTable { get; }
        string ReferenceColumn { get; }

        bool AllowNull { get; }
    }
}