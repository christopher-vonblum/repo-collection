namespace CVB.NET.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using GenericModel;
    using MetaData.Views;
    using PostSharp.Patterns.Contracts;

    public interface IReadOnlyRepository<TOrmTypeView> where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
        IGenericModel<TOrmTypeView> Read([NotNull] IGenericModel<TOrmTypeView> model);
        bool Exists([NotNull] IGenericModel<TOrmTypeView> model);
    }

    public interface IReadOnlyRepository<TScheme, TOrmTypeView> : IReadOnlyRepository<TOrmTypeView>
        where TScheme : class
        where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
        TScheme Read([NotNull] IDictionary<string, object> identifierProperties, TScheme model);
        TScheme Read([NotNull] Type tModel, IDictionary<string, object> identifierProperties = null, TScheme model = default(TScheme));
        TModel Read<TModel>(IDictionary<string, object> identifierProperties = null, TModel model = default(TModel)) where TModel : TScheme;
        TModel Read<TModel>([NotNull] TModel model) where TModel : TScheme;

        bool Exists([NotNull] IDictionary<string, object> identifierProperties, TScheme model);
        bool Exists([NotNull] Type tModel, IDictionary<string, object> identifierPropertie = null, TScheme model = default(TScheme));
        bool Exists<TModel>(IDictionary<string, object> identifierProperties = null, TModel model = default(TModel)) where TModel : TScheme;
        bool Exists<TModel>([NotNull] TModel model) where TModel : TScheme;
    }
}