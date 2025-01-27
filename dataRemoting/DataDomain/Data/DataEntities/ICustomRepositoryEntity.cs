namespace DataDomain
{
    public interface ICustomRepositoryEntity : IEntity
    {
        TIdentity GetIdentity<TIdentity>();
        void SetIdentity<TIdentity>(TIdentity identity);
    }
}