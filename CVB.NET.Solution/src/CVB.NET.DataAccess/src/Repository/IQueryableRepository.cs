namespace CVB.NET.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using GenericModel;
    using MetaData.Views;

    public interface IQueryableRepository<TOrmTypeView> where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
        IEnumerable<IGenericModel<TOrmTypeView>> Query(IGenericModel<TOrmTypeView> queryModel);
    }

    public interface IQueryableRepository<TScheme, TOrmTypeView> : IQueryableRepository<TOrmTypeView>
        where TScheme : class where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
        IEnumerable<TScheme> Query(IDictionary<string, object> properties, TScheme model);
        IEnumerable<TScheme> Query(Type tModel, IDictionary<string, object> properties);
        IEnumerable<TModel> Query<TModel>(IDictionary<string, object> properties) where TModel : TScheme;
        IEnumerable<TModel> Query<TModel>(TModel model) where TModel : TScheme;
    }
}