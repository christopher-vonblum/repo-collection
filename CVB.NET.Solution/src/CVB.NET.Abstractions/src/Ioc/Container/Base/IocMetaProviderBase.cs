namespace CVB.NET.Abstractions.Ioc.Container.Base
{
    using System;
    using System.Collections.Generic;

    using CVB.NET.Abstractions.Ioc.Base;

    public abstract class IocMetaProviderBase : IIocMetaProvider
    {
        public bool HasServiceRegistration(Type tService)
        {
            return this.HasServiceRegistration(tService, (string)null);
        }

        public bool HasServiceRegistration(Type tService, string name)
        {
            return this.HasServiceRegistration(tService, new Dictionary<string, string> { { nameof(name), name } });
        }

        public abstract bool HasServiceRegistration(Type tService, IReadOnlyDictionary<string, string> keys);

        public bool HasServiceRegistration<TService>()
            where TService : class
        {
            return this.HasServiceRegistration(typeof(TService));
        }

        public bool HasServiceRegistration<TService>(string name)
            where TService : class
        {
            return this.HasServiceRegistration(typeof(TService), name);
        }

        public bool HasServiceRegistration<TService>(IReadOnlyDictionary<string, string> keys)
            where TService : class
        {
            return this.HasServiceRegistration(typeof(TService), keys);
        }

        public Type ResolveImplementationType(Type tService)
        {
            return this.ResolveImplementationType(tService, (string)null);
        }

        public Type ResolveImplementationType(Type tService, string name)
        {
            return this.ResolveImplementationType(tService, new Dictionary<string, string> { { nameof(name), name } });
        }

        public abstract Type ResolveImplementationType(Type tService, IReadOnlyDictionary<string, string> keys);

        public Type ResolveImplementationType<TService>()
        {
            return this.ResolveImplementationType(typeof(TService));
        }

        public Type ResolveImplementationType<TService>(string name)
        {
            return this.ResolveImplementationType(typeof(TService), name);
        }

        public Type ResolveImplementationType<TService>(IReadOnlyDictionary<string, string> keys)
        {
            return this.ResolveImplementationType(typeof(TService), keys);
        }
    }
}