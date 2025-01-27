namespace CVB.NET.DataAccess.Repository
{
    using MetaData.Views;

    public interface IQueryableCrudRepository<TOrmTypeView> : ICrudRepository<TOrmTypeView>, IQueryableRepository<TOrmTypeView> where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
    }

    public interface IQueryableCrudRepository<TScheme, TOrmTypeView> : IQueryableCrudRepository<TOrmTypeView>, ICrudRepository<TScheme, TOrmTypeView>, IQueryableRepository<TScheme, TOrmTypeView>
        where TScheme : class where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
    }
}