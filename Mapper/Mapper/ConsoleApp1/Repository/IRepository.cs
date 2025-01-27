using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Repository
{
    public interface IRepository<TId>
    {
        void Create<T>(T entity) where T : IEntity<TId>;
        T Create<T>(TId id) where T : IEntity<TId>;
        T Read<T>(TId id) where T : IEntity<TId>;
        object Read(Type t, TId id);
        void Update(IEntity<TId> entity);
        void Delete(TId id);
    }
}