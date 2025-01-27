namespace CVB.NET.DataAccess.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using MetaData.Utils;
    using PostSharp.Patterns.Contracts;
    using Repository.GenericModel;
    using T4Queries.Model;
    using T4Queries.Model.Base;
    using T4Queries.Model.MetaData;
    using T4Queries.Model.SubModels;
    using T4Queries.Queries.Record;
    using T4Queries.Queries.Table;
    using T4Queries.Queries.Utils;

    public class SqlCommandProvider : ISqlCommandProvider
    {
        private SqlConnection Connection { get; }

        private string Database { get; }

        private string Schema { get; }

        private IGenericModelMapper<OrmSqlTypeMetaDataInfoView> ModelMapper { get; }

        public SqlCommandProvider([NotNull] SqlConnection connection, [NotNull] string schema, [NotNull] IGenericModelMapper<OrmSqlTypeMetaDataInfoView> mapper)
        {
            ModelMapper = mapper;
            Connection = connection;

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            Database = connection.Database;
            Schema = schema;
        }

        public SqlCommand GetCreateTableDependenciesCommand([NotNull] IList<ITableDescription> tableDescriptions)
        {
            return new SqlCommand(GetWriteQueryHeader() + GetCreateTableDependenciesQuery(tableDescriptions) + GetWriteQueryFooter(), Connection);
        }

        public ImmutableList<ITableDescription> GetDependantTableDescriptions(OrmSqlTypeMetaDataInfoView ormType)
        {
            return GetTableDescriptionsInternal(ormType);
        }

        public ITableDescription GetTableDescription(OrmSqlTypeMetaDataInfoView ormType)
        {
            return ormType.TableDescription;
        }

        public SqlCommand GetCreateTableCommand(ITableDescription tableDescription)
        {
            string query = new CreateTableQuery().Process(tableDescription);

            return new SqlCommand(query, Connection);
        }

        public SqlCommand GetCreateTableForeignConstraintsCommand(IList<ITableDescription> tableDescriptions)
        {
            string query = string.Empty;

            foreach (ITableDescription description in tableDescriptions)
            {
                query += new CreateTableConstraintsQuery().Process(description);
            }

            return new SqlCommand(query, Connection);
        }

        public SqlCommand GetCreateRecordCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            return new SqlCommand(GetCreateRecordQuery(model), Connection);
        }

        public SqlCommand GetSingleTableInsertRecordCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            return new SqlCommand(
                GetWriteQueryHeader()
                + new InsertRecordQuery()
                    .Process(GetInsertRecordDescription(model))
                + GetWriteQueryFooter(),
                Connection);
        }

        public InsertRecordDescription GetInsertRecordDescription(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            List<ColumnValue> values = new List<ColumnValue>();

            values.AddRange(SqlQueryUtils.GetPrimaryKeyValues(model.Properties, model.OrmTypeDefinition));

            values.AddRange(SqlQueryUtils.GetForeignKeyValues(model.Properties, model.OrmTypeDefinition));

            values.AddRange(SqlQueryUtils.GetPrimitiveColumnValues(model.Properties, model.OrmTypeDefinition));

            return new InsertRecordDescription(values, Schema, Database) {Connection = Connection};
        }

        public SqlCommand GetReadCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            return new SqlCommand(GetReadSqlCommandQuery(model), Connection);
        }

        public SqlCommand GetUpdateCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            UpdateRecordDescription updateQuery
                = new UpdateRecordDescription(model, Connection)
                  {
                      Database = Database,
                      Schema = Schema
                  };

            string queryText =
                GetWriteQueryHeader()
                + new UpdateRecordQuery().Process(updateQuery)
                + GetWriteQueryFooter();

            return new SqlCommand(queryText, Connection);
        }

        public SqlCommand GetDeleteTableCommand(OrmSqlTypeMetaDataInfoView getCachedTypeBoundMetaDataInfo)
        {
            string queryText = GetWriteQueryHeader()
                            + $"DROP TABLE {Schema}.[{getCachedTypeBoundMetaDataInfo.TableName}];"
                            + GetWriteQueryFooter();

            return new SqlCommand(queryText, Connection);
        }

        public SqlCommand GetDeleteCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            DeleteRecordDescription deleteQuery
                = new DeleteRecordDescription(
                    model.OrmTypeDefinition,
                    SqlQueryUtils.GetPrimaryKeyValues(model.Properties, model.OrmTypeDefinition))
                  {
                      Connection = Connection,
                      Schema = Schema,
                      Database = Database
                  };

            string queryText =
                GetWriteQueryHeader()
                + new DeleteRecordQuery().Process(deleteQuery)
                + GetWriteQueryFooter();

            return new SqlCommand(queryText, Connection);
        }

        public SqlCommand GetExistsRecordCommand(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            return new SqlCommand(GetExistsQuery(GetReadSqlCommandQuery(model)), Connection);
        }

        public SqlCommand GetExistsTableCommand(OrmSqlTypeMetaDataInfoView type)
        {
            SqlCommand existsCommand =
                new SqlCommand(
                    GetExistsQuery($@"SELECT * 
                                    FROM INFORMATION_SCHEMA.TABLES 
                                    WHERE TABLE_SCHEMA = '{Schema}' 
                                    AND TABLE_NAME = '{type.TableName}'"), Connection);

            return existsCommand;
        }

        private string GetCreateTableDependenciesQuery([NotNull] IList<ITableDescription> tableDescriptions)
        {
            string queryText = string.Empty;

            foreach (ITableDescription description in tableDescriptions)
            {
                queryText += new CreateTableQuery().Process(description);
            }

            return queryText;
        }

        private ImmutableList<ITableDescription> GetTableDescriptionsInternal(OrmSqlTypeMetaDataInfoView ormType, List<OrmSqlTypeMetaDataInfoView> baseTypes = null)
        {
            if (baseTypes == null)
            {
                baseTypes = new List<OrmSqlTypeMetaDataInfoView>();
            }

            List<OrmSqlTypeMetaDataInfoView> dependencyTypes = new List<OrmSqlTypeMetaDataInfoView>();

            dependencyTypes.AddRange(ModelBaseTypeUtils.GetAllDependencyTypes(ormType));

            List<ITableDescription> descriptions = new List<ITableDescription>();

            foreach (OrmSqlTypeMetaDataInfoView dependencyType in dependencyTypes)
            {
                if (baseTypes.Contains(dependencyType))
                {
                    continue;
                }

                baseTypes.Add(dependencyType);

                descriptions.Add(dependencyType.TableDescription);

                descriptions.AddRange(GetTableDescriptionsInternal(dependencyType, baseTypes));
            }

            return descriptions.ToImmutableList();
        }

        private List<string> GetSelectTableNames(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            return new List<OrmSqlTypeMetaDataInfoView> {model.OrmTypeDefinition}.Concat(ModelBaseTypeUtils.GetModelBaseDependencyTypes(model.OrmTypeDefinition))
                .Select(type => type.TableName)
                .ToList();
        }

        private string GetCreateRecordQuery(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            List<OrmSqlTypeMetaDataInfoView> baseTypes =
                ModelBaseTypeUtils.GetModelBaseDependencyTypes(model.OrmTypeDefinition).ToList();

            if (!baseTypes.Contains(model.OrmTypeDefinition))
            {
                baseTypes.Add(model.OrmTypeDefinition);
            }

            List<InsertRecordDescription> prioritizedBaseTypes = GetPrioritizedBaseDescriptions(model, baseTypes);

            string queryText = string.Empty;

            foreach (InsertRecordDescription prioritizedInsertRecordDescription in prioritizedBaseTypes)
            {
                queryText += new InsertRecordQuery().Process(prioritizedInsertRecordDescription);
            }

            return GetWriteQueryHeader()
                   + queryText
                   + GetWriteQueryFooter();
        }

        private List<InsertRecordDescription> GetPrioritizedBaseDescriptions(IGenericModel<OrmSqlTypeMetaDataInfoView> model, List<OrmSqlTypeMetaDataInfoView> baseTypes)
        {
            List<Tuple<int, InsertRecordDescription>> baseTypesByPriority = new List<Tuple<int, InsertRecordDescription>>();

            foreach (OrmSqlTypeMetaDataInfoView baseType in baseTypes)
            {
                InsertRecordDescription insertQuery =
                    GetInsertRecordDescription(
                        ModelMapper.MapPropertiesToNewGenericModel(
                            baseType.InnerReflectionInfo,
                            model.Properties));

                baseTypesByPriority.Add(new Tuple<int, InsertRecordDescription>(ModelBaseTypeUtils.GetModelTypeLevel(baseType),
                    insertQuery));
            }

            return baseTypesByPriority
                .OrderBy(pair => pair.Item1)
                .Select(pair => pair.Item2)
                .ToList();
        }

        private string GetReadSqlCommandQuery(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            List<IColumnDescription> valueColumns = new List<IColumnDescription>();

            valueColumns.AddRange(model.OrmTypeDefinition.PrimitiveColumnDescriptions);

            valueColumns.AddRange(model.OrmTypeDefinition.TableForeignKeyDescriptions);

            OrmSqlTypeMetaDataInfoView[] baseTypes = ModelBaseTypeUtils.GetModelBaseDependencyTypes(model.OrmTypeDefinition);

            foreach (OrmSqlTypeMetaDataInfoView baseType in baseTypes)
            {
                valueColumns.AddRange(baseType.PrimitiveColumnDescriptions);
                valueColumns.AddRange(baseType.TableForeignKeyDescriptions);
            }

            SelectRecordDescription selectDescription =
                new SelectRecordDescription(
                    Database,
                    Schema,
                    GetSelectTableNames(model),
                    SqlQueryUtils.GetPrimaryKeyValues(model.Properties, model.OrmTypeDefinition),
                    valueColumns);

            return new SelectRecordQuery().Process(selectDescription);
        }

        private string GetExistsQuery(string selectQueryText)
        {
            return GetWriteQueryHeader()
                   + $@"IF(
                        EXISTS({selectQueryText}))
                            SELECT CAST(1 AS BIT)
                        ELSE
                            SELECT CAST(0 AS BIT)"
                   + GetWriteQueryFooter();
        }

        private string GetWriteQueryHeader()
        {
            return new WriteQueryHeader().Process(new SchemaScopedQueryDescription(Schema, Database, Connection));
        }

        private string GetWriteQueryFooter()
        {
            return new WriteQueryFooter().Process(new SchemaScopedQueryDescription(Schema, Database, Connection));
        }
    }
}