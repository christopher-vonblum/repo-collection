namespace CVB.NET.Abstractions.Ioc.Provider.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Container.Model;
    using Utils;

    public struct ServiceInstanceKey : IServiceInstanceKey, IEquatable<ServiceInstanceKey>
    {
        private IReadOnlyDictionary<string, object> varyBy;
        public IServiceRegistrationKey Key { get; private set; }

        public IReadOnlyDictionary<string, object> VaryBy
        {
            get { return varyBy ?? new Dictionary<string, object>(); }
            set { varyBy = value; }
        }

        public static IServiceInstanceKey Create(
            IServiceRegistrationKey serviceRegistrationKey,
            IReadOnlyDictionary<string, object> varyInstanceBy = null)
        {
            return new ServiceInstanceKey
            {
                Key = serviceRegistrationKey,
                VaryBy = varyInstanceBy
            };
        }

        public bool Equals(ServiceInstanceKey other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is ServiceInstanceKey && this == (ServiceInstanceKey)obj;
        }

        public override int GetHashCode()
        {
            int hashCode = Key.GetNamedHashcode(nameof(Key));

            return VaryBy.Aggregate(hashCode, (current, kv) => current ^ kv.GetNamedHashcode("$instance$" + kv.Key));
        }

        public static bool operator ==(ServiceInstanceKey x, IServiceInstanceKey y)
        {
            return y != null
                && x.Key.Equals(y.Key)
                && x.VaryBy.All(v => y.VaryBy.ContainsKey(v.Key)
                                  && v.Value != null
                                  && v.Value.Equals(y.VaryBy[v.Key]));
        }

        public static bool operator !=(ServiceInstanceKey x, IServiceInstanceKey y)
        {
            return !(x == y);
        }
    }
}