namespace CVB.NET.DataAccess.Sql.T4Queries.Model
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Base;
    using DataAccess.MetaData.Utils;
    using MetaData;
    using SubModels;

    public class DeleteRecordDescription : TableScopedQueryDescription
    {
        public Dictionary<string, List<ColumnValue>> TablePrimaryKeys { get; set; }

        public DeleteRecordDescription(OrmSqlTypeMetaDataInfoView ormInfoView, List<ColumnValue> selectConstraints, SqlConnection connection = null)
            : base(ormInfoView, connection)
        {
            IEnumerable<ColumnValue> primaryKeys = selectConstraints
                .Where(column => column.ColumnDescription.IsPrimaryKey);

            TablePrimaryKeys = new Dictionary<string, List<ColumnValue>>();

            List<OrmSqlTypeMetaDataInfoView> baseTypes = ModelBaseTypeUtils.GetModelBaseDependencyTypes(ormInfoView).ToList();

            baseTypes.Add(ormInfoView);

            foreach (OrmSqlTypeMetaDataInfoView ormType in baseTypes)
            {
                foreach (ColumnValue primaryKey in primaryKeys)
                {
                    TablePrimaryKeys.Add(
                        ormType.TableName,
                        ormType.TablePrimaryKeyDescriptions.Select(key =>
                            selectConstraints.FirstOrDefault(prop =>
                                prop.Name.Equals(primaryKey.Name))).ToList());
                }
            }

            TablePrimaryKeys = TablePrimaryKeys.Reverse().ToDictionary(key => key.Key, val => val.Value);
        }
    }
}