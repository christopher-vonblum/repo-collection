namespace CVB.NET.Abstractions.Ioc.Container.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;

    public struct ServiceRegistrationKey : IServiceRegistrationKey, IEquatable<IServiceRegistrationKey>
    {
        private IReadOnlyDictionary<string, object> varyBy;
        public Type Service { get; private set; }

        public IReadOnlyDictionary<string, object> VaryBy
        {
            get { return varyBy ?? new Dictionary<string, object>(); }
            private set { varyBy = value; }
        }

        public static IServiceRegistrationKey Create(
            Type tService,
            IReadOnlyDictionary<string, object> varyServiceBy = null)
        {
            return new ServiceRegistrationKey
            {
                Service = tService,
                VaryBy = varyServiceBy,
                RedirectGenericArguments = tService == tService.GetGenericTypeDefinition()
            };
        }

        public bool RedirectGenericArguments { get; set; }

        public bool Equals(IServiceRegistrationKey other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is ServiceRegistrationKey && this == (ServiceRegistrationKey)obj;
        }

        public override int GetHashCode()
        {
            int hashCode = Service.GetNamedHashcode(nameof(Service)) ^ RedirectGenericArguments.GetNamedHashcode(nameof(RedirectGenericArguments));

            return VaryBy.Aggregate(hashCode, (current, kv) => current ^ kv.GetNamedHashcode("$service$" + kv.Key));
        }

        public static bool operator ==(ServiceRegistrationKey x, IServiceRegistrationKey y)
        {
            return y != null
                   && x.RedirectGenericArguments == y.RedirectGenericArguments
                   && x.Service == y.Service
                   && x.VaryBy.All(v => y.VaryBy.ContainsKey(v.Key) && v.Value != null && v.Value.Equals(y.VaryBy[v.Key]));
        }

        public static bool operator !=(ServiceRegistrationKey x, IServiceRegistrationKey y)
        {
            return !(x == y);
        }
    }
}