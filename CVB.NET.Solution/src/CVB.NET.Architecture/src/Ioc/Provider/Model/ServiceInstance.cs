namespace CVB.NET.Abstractions.Ioc.Provider.Model
{
    using System;

    public struct ServiceInstance : IServiceInstance, IEquatable<IServiceInstance>
    {
        public IServiceInstanceKey Key { get; private set; }
        public object Instance { get; private set; }

        public static IServiceInstance Create(IServiceInstanceKey instanceKey, object instance)
        {
            return new ServiceInstance
                   {
                       Key = instanceKey,
                       Instance = instance
                   };
        }

        public override bool Equals(object obj)
        {
            return obj is IServiceInstance && Equals((IServiceInstance) obj);
        }

        public bool Equals(ServiceInstance other)
        {
            return Equals(Key, other.Key);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public bool Equals(IServiceInstance other)
        {
            return other != null && Key != null && other.Key.Equals(Key);
        }
    }
}