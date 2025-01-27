namespace CVB.NET.DataAccess.Sql.T4Queries.Model
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Base;
    using DataAccess.MetaData.Utils;
    using MetaData;
    using Queries.Utils;
    using Repository.GenericModel;
    using SubModels;

    public class UpdateRecordDescription : SchemaScopedQueryDescription
    {
        public List<ColumnValue> Keys { get; }

        public IDictionary<string, List<ColumnValue>> TableValues { get; }

        public UpdateRecordDescription(IGenericModel<OrmSqlTypeMetaDataInfoView> model, SqlConnection connection = null) : base(null, null, connection)
        {
            Keys = SqlQueryUtils.GetPrimaryKeyValues(model.Properties, model.OrmTypeDefinition);

            List<OrmSqlTypeMetaDataInfoView> baseTypes = ModelBaseTypeUtils.GetModelBaseDependencyTypes(model.OrmTypeDefinition).ToList();

            TableValues = new Dictionary<string, List<ColumnValue>>();

            baseTypes.Add(model.OrmTypeDefinition);

            foreach (OrmSqlTypeMetaDataInfoView ormType in baseTypes)
            {
                List<ColumnValue> values =
                    SqlQueryUtils.GetForeignKeyValues(model.Properties, ormType)
                        .Concat(
                            SqlQueryUtils.GetPrimitiveColumnValues(model.Properties, ormType))
                        .ToList();

                TableValues.Add(ormType.TableName, values);
            }
        }
    }
}