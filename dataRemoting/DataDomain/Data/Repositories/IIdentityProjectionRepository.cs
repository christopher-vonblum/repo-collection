namespace DataDomain
{
    public interface IIdentityProjectionRepository<TIdentity> : IRepository
    {
        bool Exists(TIdentity identity);

        bool HasDataOf<T>(TIdentity identity);
        IEntity Activate(TIdentity entity);
        IEntity Read(TIdentity identity);
        void Delete(TIdentity identity);
    }
}