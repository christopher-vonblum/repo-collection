namespace CVB.NET.DataAccess.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using DataAccess.Exception;
    using Exception;
    using MetaData.Attributes;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching;
    using Reflection.Caching.Cached;
    using Repository.Base;
    using Repository.GenericModel;
    using T4Queries.Model.MetaData;
    using T4Queries.Model.SubModels;
    using Utils;

    public class MsSqlQueryableCrudRepository<TScheme> : QueryableCrudRepositoryBase<TScheme, OrmSqlTypeMetaDataInfoView> where TScheme : class
    {
        protected SqlConnection SqlConnection { get; }

        protected ISqlCommandProvider CommandProvider { get; }

        protected string Database { get; }

        protected OrmSqlTypeMetaDataInfoView SchemeOrmTypeMetaDataInfoView { get; } = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(typeof (TScheme));

        protected string DatabaseSchema { get; } = "dbo";

        public MsSqlQueryableCrudRepository([NotNull] SqlConnection sqlConnection, string database)
        {
            SqlConnection = sqlConnection;

            if (SqlConnection.State == ConnectionState.Closed)
            {
                SqlConnection.Open();
            }

            CommandProvider = new SqlCommandProvider(SqlConnection, DatabaseSchema, Mapper);

            Database = database;

            if (!SchemeOrmTypeMetaDataInfoView.Cast<CachedType>().Attributes.OfType<ModelBaseAttribute>().Any())
            {
                throw new InvalidOperationException(SchemeOrmTypeMetaDataInfoView.InnerReflectionInfo.FullName + " is not marked with the ModelBase-Attribute.");
            }
        }

        public override void Create(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            EnsureDependantTablesCreated(model.OrmTypeDefinition);

            CreateRecordInternal(model);
        }


        private void CreateRecordInternal(IGenericModel<OrmSqlTypeMetaDataInfoView> model, bool @throw = false)
        {
            SqlCommand createCommand;

            if (@throw)
            {
                createCommand = CommandProvider.GetCreateRecordCommand(model);
                createCommand.ExecuteNonQuery();
                return;
            }

            createCommand = CommandProvider.GetCreateRecordCommand(model);

            try
            {
                createCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new CreateRecordException(ex) {Command = createCommand};
            }
        }

        public override IGenericModel<OrmSqlTypeMetaDataInfoView> Read(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            SqlCommand selectCommand = CommandProvider.GetReadCommand(model);

            try
            {
                using (SqlDataReader modelReader = selectCommand.ExecuteReader())
                {
                    return SelectSingleModelInternal(model, modelReader);
                }
            }
            catch (SqlException ex)
            {
                throw new ReadRecordException(ex)
                      {
                          Command = selectCommand
                      };
            }
        }

        private IGenericModel<OrmSqlTypeMetaDataInfoView> SelectSingleModelInternal(IGenericModel<OrmSqlTypeMetaDataInfoView> model, SqlDataReader modelReader)
        {
            // No results
            if (!modelReader.Read())
            {
                throw new ReadRecordException(null);
            }

            model = ReadModelInternal(modelReader, model);

            // multiple results
            if (modelReader.Read())
            {
                throw new ReadRecordException(null);
            }

            return model;
        }

        private IGenericModel<OrmSqlTypeMetaDataInfoView> ReadModelInternal(SqlDataReader modelReader, IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            model = GetForeignKeyValues(modelReader, model);

            model = GetPrimitiveValues(modelReader, model);

            return model;
        }

        private static IGenericModel<OrmSqlTypeMetaDataInfoView> GetForeignKeyValues(SqlDataReader modelReader, IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            foreach (CachedPropertyInfo foreignKeyProperty in model.OrmTypeDefinition.ForeignModelReferences)
            {
                CachedType propertyType = foreignKeyProperty.InnerReflectionInfo.PropertyType;

                object foreignPropertyInstance = CreateForeignPropertyInstanceInternal(propertyType);

                model.Properties[foreignKeyProperty] = foreignPropertyInstance;

                SetForeignPropertyPrimaryKeys(modelReader, foreignKeyProperty, foreignPropertyInstance);
            }

            return model;
        }

        private static object CreateForeignPropertyInstanceInternal(CachedType propertyType)
        {
            //TODO: better way to create foreign models here. Constructor injection?
            return propertyType.DefaultConstructor.InnerReflectionInfo.Invoke(null);
        }

        private static void SetForeignPropertyPrimaryKeys(SqlDataReader modelReader, CachedPropertyInfo foreignKeyProperty, object foreignPropertyInstance)
        {
            OrmSqlTypeMetaDataInfoView propertyOrmTypeDefinition = ((CachedType) foreignKeyProperty.InnerReflectionInfo.PropertyType).Cast<OrmSqlTypeMetaDataInfoView>();

            // set foreign primary keys
            foreach (CachedPropertyInfo primaryKey in propertyOrmTypeDefinition.PrimaryKeyProperties)
            {
                object foreignKeyColumnValue =
                    modelReader[SqlModelUtils.GetForeignColumnName(foreignKeyProperty, primaryKey)];

                if (foreignKeyColumnValue is DBNull)
                {
                    primaryKey.InnerReflectionInfo.SetValue(foreignPropertyInstance, null);
                    continue;
                }

                primaryKey.InnerReflectionInfo.SetValue(foreignPropertyInstance, foreignKeyColumnValue);
            }
        }

        private static IGenericModel<OrmSqlTypeMetaDataInfoView> GetPrimitiveValues(SqlDataReader modelReader, IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            foreach (CachedPropertyInfo primitiveProperty in model.OrmTypeDefinition.PrimitiveValueProperties)
            {
                object primitivePropertyValue = modelReader[primitiveProperty.InnerReflectionInfo.Name];

                if (primitivePropertyValue is DBNull)
                {
                    model.Properties[primitiveProperty] = null;
                    continue;
                }

                model.Properties[primitiveProperty] = primitivePropertyValue;
            }

            return model;
        }

        public override IGenericModel<OrmSqlTypeMetaDataInfoView> Update(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            SqlCommand updateCommand = CommandProvider.GetUpdateCommand(model);

            if (!Exists(model))
            {
                throw new UpdateRecordException(null) {Command = updateCommand};
            }

            try
            {
                updateCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new UpdateRecordException(ex) {Command = updateCommand};
            }

            return model;
        }

        public override void Delete(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            SqlCommand deleteCommand = CommandProvider.GetDeleteCommand(model);

            try
            {
                deleteCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DeleteRecordException(ex);
            }
        }

        public override bool Exists(IGenericModel<OrmSqlTypeMetaDataInfoView> model)
        {
            try
            {
                return TableExists(model.OrmTypeDefinition)
                       && CommandProvider.GetExistsRecordCommand(model).ExecuteScalar().Equals(true);
            }
            catch
            {
                return false;
            }
        }

        public override IEnumerable<IGenericModel<OrmSqlTypeMetaDataInfoView>> Query(IGenericModel<OrmSqlTypeMetaDataInfoView> queryModel)
        {
            SqlCommand selectRecordCommand = CommandProvider.GetReadCommand(queryModel);

            List<IGenericModel<OrmSqlTypeMetaDataInfoView>> models = new List<IGenericModel<OrmSqlTypeMetaDataInfoView>>();

            try
            {
                SqlDataReader modelReader = selectRecordCommand.ExecuteReader();

                while (modelReader.Read())
                {
                    Dictionary<CachedPropertyInfo, object> properties = new Dictionary<CachedPropertyInfo, object>();

                    for (int i = 0; i < modelReader.FieldCount; i++)
                    {
                        properties[
                            queryModel.Properties.Single(prop => prop.Key.InnerReflectionInfo.Name.Equals(modelReader.GetName(i))).Key]
                            = modelReader[i];
                    }
                    ReadModelInternal(modelReader, queryModel);
                    models.Add(Mapper.MapPropertiesToNewGenericModel(queryModel.Type, properties));
                }
            }
            catch (SqlException ex)
            {
                throw new ReadRecordException(ex) {Command = selectRecordCommand};
            }

            return models;
        }

        private void EnsureDependantTablesCreated(OrmSqlTypeMetaDataInfoView modelType)
        {
            if (TableExists(modelType)) return;

            IList<ITableDescription> tableDescriptions = CommandProvider.GetDependantTableDescriptions(modelType);

            try
            {
                CreateTables(tableDescriptions);

                CreateRootIdentity();

                CreateIdentityData(tableDescriptions);

                CreateForeignKeyConstraints(tableDescriptions);
            }
            catch (SqlException ex)
            {
                throw new CreateTypeTableException(ex);
            }
        }

        private void CreateRootIdentity()
        {
            try
            {
                TScheme rootIdentity = (TScheme) OrmSqlTypeMetaDataInfoView.GetRootIdentityModel(typeof (TScheme));

                if (!Exists(rootIdentity))
                {
                    CreateRecordInternal(Mapper.MapInstanceToNewGenericModel(rootIdentity), true);
                }
            }
            catch (SqlException ex)
            {
                throw new CreateRootIdentityException(ex);
            }
        }

        private void CreateIdentityData(IList<ITableDescription> tableDescriptions)
        {
            try
            {
                foreach (ITableDescription tableDescription in tableDescriptions)
                {
                    if (tableDescription.OrmTypeMetaDataInfoView.HasTypeReferencePropertyInfo)
                    {
                        EnsureModelTypeRecordCreated(tableDescription);
                    }

                    EnsureTypeIdentityRecordCreated(tableDescription);
                }
            }
            catch (SqlException ex)
            {
                throw new CreateTypeIdentityException(ex);
            }
        }

        private void EnsureModelTypeRecordCreated(ITableDescription tableDescription)
        {
            try
            {
                CachedType typeReferenceType =
                    tableDescription.OrmTypeMetaDataInfoView.TypeReferencePropertyInfo.InnerReflectionInfo.PropertyType;

                IGenericModel<OrmSqlTypeMetaDataInfoView> model = Mapper.MapInstanceToNewGenericModel(CreateForeignPropertyInstanceInternal(typeReferenceType));

                if (!Exists(model))
                {
                    CreateRecordInternal(model, true);
                }
            }
            catch (SqlException ex)
            {
                throw new CreateTypeIdentityException(ex);
            }
        }

        private void EnsureTypeIdentityRecordCreated(ITableDescription tableDescription)
        {
            try
            {
                TScheme identity = (TScheme) OrmSqlTypeMetaDataInfoView.GetTypeIdentityModel(tableDescription.OrmTypeMetaDataInfoView.InnerReflectionInfo);

                IGenericModel<OrmSqlTypeMetaDataInfoView> model =
                    Mapper.MapInstanceToNewGenericModel(identity);

                if (!Exists(model))
                {
                    CreateRecordInternal(model, true);
                }
            }
            catch (SqlException ex)
            {
                throw new CreateTypeIdentityException(ex);
            }
        }

        private void CreateForeignKeyConstraints(IList<ITableDescription> descriptions)
        {
            try
            {
                SqlCommand createTableForeignConstraintsCommand =
                    CommandProvider.GetCreateTableForeignConstraintsCommand(descriptions);

                createTableForeignConstraintsCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new CreateForeignKeyConstraintsException(ex);
            }
        }

        private void CreateTables(IList<ITableDescription> descriptions)
        {
            try
            {
                SqlCommand createTableDependenciesCommand =
                    CommandProvider.GetCreateTableDependenciesCommand(descriptions);

                createTableDependenciesCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new CreateTypeTableException(ex);
            }
        }

        private bool TableExists(OrmSqlTypeMetaDataInfoView modelType)
        {
            return CommandProvider.GetExistsTableCommand(modelType).ExecuteScalar().Equals(true);
        }
    }
}