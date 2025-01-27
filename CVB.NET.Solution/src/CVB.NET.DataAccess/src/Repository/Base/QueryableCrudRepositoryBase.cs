namespace CVB.NET.DataAccess.Repository.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GenericModel;
    using MetaData.Views;
    using Reflection.Caching;
    using Reflection.Caching.Cached;

    public abstract class QueryableCrudRepositoryBase<TScheme, TOrmTypeView> : IQueryableCrudRepository<TScheme, TOrmTypeView>
        where TScheme : class
        where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
        protected TOrmTypeView RepositorySchemaType { get; private set; }

        protected static IGenericModelMapper<TOrmTypeView> Mapper { get; } = new GenericModelMapper<TOrmTypeView>();

        protected QueryableCrudRepositoryBase()
        {
            RepositorySchemaType = ReflectionCache.Get<TOrmTypeView>(typeof (TScheme));
        }

        #region abstract

        public abstract void Create(IGenericModel<TOrmTypeView> model);

        public abstract IGenericModel<TOrmTypeView> Read(IGenericModel<TOrmTypeView> model);

        public abstract IEnumerable<IGenericModel<TOrmTypeView>> Query(IGenericModel<TOrmTypeView> queryModel);

        public abstract IGenericModel<TOrmTypeView> Update(IGenericModel<TOrmTypeView> model);

        public abstract bool Exists(IGenericModel<TOrmTypeView> model);

        public abstract void Delete(IGenericModel<TOrmTypeView> model);

        #endregion

        #region half generic operation

        public void Create<TModel>(IDictionary<string, object> properties, TModel model = default(TModel)) where TModel : TScheme
        {
            if (default(TModel).Equals(model))
            {
                Create(Mapper.MapInstanceToNewGenericModel(
                    ((CachedType) typeof (TModel))
                        .DefaultConstructor
                        .InnerReflectionInfo
                        .Invoke(null),
                    properties));

                return;
            }

            Create(Mapper.MapInstanceToNewGenericModel(model, properties));
        }

        public TModel Read<TModel>(IDictionary<string, object> identifierProperties, TModel model = default(TModel)) where TModel : TScheme
        {
            if (default(TModel).Equals(model))
            {
                return (TModel) Read(Mapper.MapInstanceToNewGenericModel(
                    ((CachedType) typeof (TModel)).DefaultConstructor.InnerReflectionInfo.Invoke(null),
                    identifierProperties))
                    .Instance;
            }

            return (TModel) Read(Mapper.MapInstanceToNewGenericModel(model, identifierProperties)).Instance;
        }

        public IEnumerable<TModel> Query<TModel>(IDictionary<string, object> properties) where TModel : TScheme
        {
            return (IEnumerable<TModel>) Query(Mapper.MapPropertiesToNewGenericModel(typeof (TModel), properties));
        }

        public bool Exists<TModel>(IDictionary<string, object> identifierProperties, TModel model = default(TModel)) where TModel : TScheme
        {
            if (default(TModel).Equals(model))
            {
                return Exists(Mapper.MapPropertiesToNewGenericModel(typeof (TModel), identifierProperties));
            }

            return Exists(Mapper.MapInstanceToNewGenericModel(model, identifierProperties));
        }

        public TModel Update<TModel>(IDictionary<string, object> properties, TModel model = default(TModel)) where TModel : TScheme
        {
            if (default(TModel).Equals(model))
            {
                return (TModel) Update(Mapper.MapPropertiesToNewGenericModel(typeof (TModel), properties)).Instance;
            }

            return (TModel) Update(Mapper.MapInstanceToNewGenericModel(model, properties)).Instance;
        }

        public void Delete<TModel>(IDictionary<string, object> identifierProperties) where TModel : TScheme
        {
            Delete(Mapper.MapInstanceToNewGenericModel(typeof (TModel), identifierProperties));
        }

        #endregion

        #region generic operation

        public void Create<TModel>(TModel model) where TModel : TScheme
        {
            Create(Mapper.MapInstanceToNewGenericModel(model));
        }

        public TModel Read<TModel>(TModel model) where TModel : TScheme
        {
            return (TModel) Read(Mapper.MapInstanceToNewGenericModel(model)).Instance;
        }

        public IEnumerable<TModel> Query<TModel>(TModel model) where TModel : TScheme
        {
            return (IEnumerable<TModel>) Query(Mapper.MapInstanceToNewGenericModel(model)).Select(genericModel => genericModel.Instance);
        }

        public bool Exists<TModel>(TModel model) where TModel : TScheme
        {
            return Exists(Mapper.MapInstanceToNewGenericModel(model));
        }

        public TModel Update<TModel>(TModel model) where TModel : TScheme
        {
            return (TModel) Update(Mapper.MapInstanceToNewGenericModel(model)).Instance;
        }

        public void Delete<TModel>(TModel model) where TModel : TScheme
        {
            Delete(Mapper.MapInstanceToNewGenericModel(model));
        }

        #endregion

        #region dynamic operation

        public void Create(IDictionary<string, object> properties, TScheme model)
        {
            Create(Mapper.MapInstanceToNewGenericModel(model));
        }

        public void Create(Type tModel, IDictionary<string, object> properties, TScheme model = default(TScheme))
        {
            if (model == default(TScheme))
            {
                Create(Mapper.MapPropertiesToNewGenericModel(tModel, properties));
            }

            Create(Mapper.MapInstanceToNewGenericModel(model, properties));
        }

        public TScheme Read(IDictionary<string, object> identifierProperties, TScheme model)
        {
            return (TScheme) Read(Mapper.MapInstanceToNewGenericModel(model, identifierProperties)).Instance;
        }

        public TScheme Read(Type tModel, IDictionary<string, object> identifierProperties = null, TScheme model = default(TScheme))
        {
            if (model == default(TScheme))
            {
                return (TScheme) Read(Mapper.MapPropertiesToNewGenericModel(tModel, identifierProperties)).Instance;
            }

            return (TScheme) Read(Mapper.MapInstanceToNewGenericModel(model, identifierProperties)).Instance;
        }

        public IEnumerable<TScheme> Query(IDictionary<string, object> properties, TScheme model)
        {
            return (IEnumerable<TScheme>) Query(Mapper.MapInstanceToNewGenericModel(model, properties));
        }

        public IEnumerable<TScheme> Query(Type tModel, IDictionary<string, object> properties)
        {
            return (IEnumerable<TScheme>) Query(Mapper.MapInstanceToNewGenericModel(tModel, properties));
        }

        public bool Exists(IDictionary<string, object> identifierProperties, TScheme model)
        {
            return Exists(Mapper.MapInstanceToNewGenericModel(model, identifierProperties));
        }

        public bool Exists(Type tModel, IDictionary<string, object> identifierProperties, TScheme model = default(TScheme))
        {
            if (model == default(TScheme))
            {
                return Exists(Mapper.MapPropertiesToNewGenericModel(tModel, identifierProperties));
            }

            return Exists(Mapper.MapInstanceToNewGenericModel(model, identifierProperties));
        }

        public TScheme Update(IDictionary<string, object> properties, TScheme model)
        {
            return (TScheme) Update(Mapper.MapInstanceToNewGenericModel(model, properties));
        }

        public TScheme Update(Type tModel, IDictionary<string, object> properties)
        {
            return (TScheme) Update(Mapper.MapInstanceToNewGenericModel(tModel, properties));
        }

        public void Delete(IDictionary<string, object> properties, TScheme model)
        {
            Delete(Mapper.MapInstanceToNewGenericModel(model, properties));
        }

        public void Delete(Type tModel, IDictionary<string, object> identifierProperties)
        {
            Delete(Mapper.MapPropertiesToNewGenericModel(tModel, identifierProperties));
        }

        #endregion
    }
}