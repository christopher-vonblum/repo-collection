namespace CVB.NET.DataAccess.Repository.GenericModel
{
    using System;
    using System.Collections.Generic;
    using MetaData.Views;
    using Reflection.Caching.Cached;

    public interface IGenericModelMapper<TOrmTypeView> where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
        IGenericModel<TOrmTypeView> MapInstanceToNewGenericModel(object instance,
                                                                 IDictionary<CachedPropertyInfo, object> propertiesDictionary);

        IGenericModel<TOrmTypeView> MapInstanceToNewGenericModel(object instance,
                                                                 IDictionary<string, object> propertiesDictionary);

        IGenericModel<TOrmTypeView> MapInstanceToNewGenericModel(object instance);

        IGenericModel<TOrmTypeView> MapPropertiesToNewGenericModel(Type type,
                                                                   IDictionary<CachedPropertyInfo, object> propertiesDictionary);

        IGenericModel<TOrmTypeView> MapPropertiesToNewGenericModel(Type type,
                                                                   IDictionary<string, object> propertiesDictionary);

        object MapGenericModelToNewInstance(Type modelType, IDictionary<CachedPropertyInfo, object> propertiesDictionary);
        object MapGenericModelToNewInstance(Type modelType, IDictionary<string, object> propertiesDictionary);

        object MapGenericModelToInstance(object instance, IDictionary<CachedPropertyInfo, object> properties);
        object MapGenericModelToInstance(object instance, IDictionary<string, object> properties);

        IDictionary<CachedPropertyInfo, object> GetStrongProperties(CachedType modelType, IDictionary<string, object> propertiesDictionary);
    }
}