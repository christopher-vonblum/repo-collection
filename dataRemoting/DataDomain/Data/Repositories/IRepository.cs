namespace DataDomain
{
    public interface IRepository : IEntity
    {
        bool Exists(IEntity entity);

        bool HasDataOf<T>(IEntity entity);
        IEntityProxy CreateProxy(IEntity entity);
        void Create(IEntity entity);
        IEntity Activate(IEntity entity);
        IEntity Read(IEntity entity);
        void Update(IEntity entity);
        void Delete(IEntity entity);
    }
}