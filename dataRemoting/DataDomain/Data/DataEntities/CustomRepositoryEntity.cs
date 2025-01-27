using System;
using System.Collections.Generic;

namespace DataDomain
{
    class CustomRepositoryEntity : DataEntity, ICustomRepositoryEntity
    {
        public Dictionary<string, object> Identities { get; set; }
        public TIdentity GetIdentity<TIdentity>()
        {
            throw new NotImplementedException();
        }

        public void SetIdentity<TIdentity>(TIdentity identity)
        {
            throw new NotImplementedException();
        }
    }
}