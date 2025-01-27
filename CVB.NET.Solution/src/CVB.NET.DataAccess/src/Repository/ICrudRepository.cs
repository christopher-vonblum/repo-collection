namespace CVB.NET.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using GenericModel;
    using MetaData.Views;

    public interface ICrudRepository<TOrmTypeView> : IReadOnlyRepository<TOrmTypeView> where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
        void Create(IGenericModel<TOrmTypeView> model);

        IGenericModel<TOrmTypeView> Update(IGenericModel<TOrmTypeView> model);

        void Delete(IGenericModel<TOrmTypeView> model);
    }

    public interface ICrudRepository<TScheme, TOrmTypeView> : IReadOnlyRepository<TScheme, TOrmTypeView>, ICrudRepository<TOrmTypeView>
        where TScheme : class
        where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
        void Create(IDictionary<string, object> properties, TScheme model);
        void Create(Type tModel, IDictionary<string, object> properties = null, TScheme model = default(TScheme));
        void Create<TModel>(IDictionary<string, object> properties = null, TModel model = default(TModel)) where TModel : TScheme;
        void Create<TModel>(TModel model) where TModel : TScheme;

        TScheme Update(IDictionary<string, object> properties, TScheme model);
        TScheme Update(Type tModel, IDictionary<string, object> properties = null);
        TModel Update<TModel>(IDictionary<string, object> properties = null, TModel model = default(TModel)) where TModel : TScheme;
        TModel Update<TModel>(TModel model) where TModel : TScheme;

        void Delete(IDictionary<string, object> properties, TScheme model);
        void Delete(Type tModel, IDictionary<string, object> identifierProperties);
        void Delete<TModel>(IDictionary<string, object> identifierProperties) where TModel : TScheme;
        void Delete<TModel>(TModel model) where TModel : TScheme;
    }
}