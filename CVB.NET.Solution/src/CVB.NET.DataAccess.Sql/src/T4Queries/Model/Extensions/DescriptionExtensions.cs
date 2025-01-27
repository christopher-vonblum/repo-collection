namespace CVB.NET.DataAccess.Sql.T4Queries.Model.Extensions
{
    using SubModels;

    public static class DescriptionExtensions
    {
        public static ITableDescription ToCreateTableDescription(this ITableDescription tableDescription)
        {
            return new CreateTableDescription(tableDescription.OrmTypeMetaDataInfoView);
        }
    }
}