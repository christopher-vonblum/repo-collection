using System;
using System.Collections.Generic;

namespace DataDomain
{
    class IdentityProjectionRepository<TIdentity> : IIdentityProjectionRepository<TIdentity>
    {
        private readonly IRepository _inner;

        public IdentityProjectionRepository(IRepository inner)
        {
            _inner = inner;
        }

        public bool Exists(TIdentity identity)
        {
            return Exists(GetIdentityModel(identity));
        }

        private static CustomRepositoryEntity GetIdentityModel(TIdentity identity)
        {
            return new CustomRepositoryEntity{Identities = new Dictionary<string, object>
            {
                {$"clrtypes/{typeof(TIdentity).FullName.Replace(".", ",")}", identity}
            }};
        }

        public bool HasDataOf<T>(TIdentity identity)
        {
            return HasDataOf<T>(GetIdentityModel(identity));
        }

        public IEntity Activate(TIdentity entity)
        {
            return Activate(GetIdentityModel(entity));
        }

        public IEntity Read(TIdentity identity)
        {
            return Read(GetIdentityModel(identity));
        }

        public void Delete(TIdentity identity)
        {
            Delete(GetIdentityModel(identity));
        }

        public string Path
        {
            get => _inner.Path;
            set => _inner.Path = value;
        }

        public bool HasSegment<TSegment>()
        {
            return _inner.HasSegment<TSegment>();
        }

        public TSegment GetSegment<TSegment>()
        {
            return _inner.GetSegment<TSegment>();
        }

        public object GetSegment(Type segment)
        {
            return _inner.GetSegment(segment);
        }

        public object GetSegment(IClrType segment)
        {
            return _inner.GetSegment(segment);
        }

        public void SetSegment(IClrType segment, object model)
        {
            _inner.SetSegment(segment, model);
        }

        public bool Exists(IEntity entity)
        {
            return _inner.Exists(entity);
        }

        public bool HasDataOf<T>(IEntity entity)
        {
            return _inner.HasDataOf<T>(entity);
        }

        public IEntityProxy CreateProxy(IEntity entity)
        {
            return _inner.CreateProxy(entity);
        }

        public void Create(IEntity entity)
        {
            _inner.Create(entity);
        }

        public IEntity Activate(IEntity entity)
        {
            return _inner.Activate(entity);
        }

        public IEntity Read(IEntity entity)
        {
            return _inner.Read(entity);
        }

        public void Update(IEntity entity)
        {
            _inner.Update(entity);
        }

        public void Delete(IEntity entity)
        {
            _inner.Delete(entity);
        }
    }
}