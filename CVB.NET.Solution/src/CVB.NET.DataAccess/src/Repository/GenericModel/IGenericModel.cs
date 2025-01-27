namespace CVB.NET.DataAccess.Repository.GenericModel
{
    using System.Collections.Generic;
    using MetaData.Views;
    using Reflection.Caching.Cached;

    public interface IGenericModel
    {
        CachedType Type { get; }
        IDictionary<CachedPropertyInfo, object> Properties { get; set; }
        object Instance { get; set; }
        OrmTypeMetaDataInfoViewBase MetaDataInfoViewBase { get; }
    }

    public interface IGenericModel<TOrmTypeDefinition> : IGenericModel where TOrmTypeDefinition : OrmTypeMetaDataInfoViewBase
    {
        TOrmTypeDefinition OrmTypeDefinition { get; }
    }
}