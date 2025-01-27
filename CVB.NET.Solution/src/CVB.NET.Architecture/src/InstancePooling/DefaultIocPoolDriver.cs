namespace CVB.NET.Architecture.src.Pooling
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions.Ioc.Container;

    class DefaultIocPoolDriver : IInstancePoolDriver
    {
        public IReadOnlyDictionary<string, object> VaryServiceBy
        {
            get { return varyServiceBy ?? serviceVarianceDelegates.ToDictionary(k => k.Key, v => v.Value()); }
        }

        private readonly IReadOnlyDictionary<string, object> varyServiceBy;
        private readonly IReadOnlyDictionary<string, Func<object>> serviceVarianceDelegates;

        private readonly IReadOnlyDictionary<string, Func<object>> instanceVarianceDelegates;

        private readonly ConcurrentDictionary<IReadOnlyDictionary<string, object>, object> Instances = new ConcurrentDictionary<IReadOnlyDictionary<string, object>, object>();

        private Type ServiceType;
        

        protected IIocContainer Container { get; }

        public DefaultIocPoolDriver(
            IIocContainer container,
            Type tService,
            IReadOnlyDictionary<string, Func<object>> varyInstanceBy,
            IReadOnlyDictionary<string, object> varyServiceBy = null) : this(container, tService, varyInstanceBy)
        {
            this.varyServiceBy = varyServiceBy;
        }

        public DefaultIocPoolDriver(
            IIocContainer container,
            Type tService,
            IReadOnlyDictionary<string, Func<object>> varyInstanceBy,
            IReadOnlyDictionary<string, Func<object>> varyServiceBy = null) : this(container, tService, varyInstanceBy)
        {
            serviceVarianceDelegates = varyServiceBy;
        }

        private DefaultIocPoolDriver(
            IIocContainer container,
            Type tService,
            IReadOnlyDictionary<string, Func<object>> varyInstanceBy)
        {
            Container = container;
            ServiceType = tService;
            instanceVarianceDelegates = varyInstanceBy;
        }

        public IReadOnlyDictionary<string, object> GetCurrentVarianceIdentifier()
        {
            return instanceVarianceDelegates.ToDictionary(k => k.Key, v => v.Value());
        }

        public void Renew(IReadOnlyDictionary<string, object> varyInstanceBy)
        {
            var varianceId = GetCurrentVarianceIdentifier();

            object oldInstance;

            if (Instances.TryRemove(varianceId, out oldInstance))
            {
                Instances[varianceId] = CreateInstance();
            }

            IDisposable oldDisposable = oldInstance as IDisposable;

            oldDisposable?.Dispose();
        }

        public void ReleaseAll()
        {
            Instances.Clear();
        }
        public object GetCurrentInstance()
        {
            return GetInstance(GetCurrentVarianceIdentifier());
        }
        public object GetInstance(IReadOnlyDictionary<string, object> varyInstanceBy)
        {
            return Instances.GetOrAdd(varyInstanceBy, (key) => CreateInstance());
        }

        protected virtual object CreateInstance()
        {
            return Container.CreateInstance(ServiceType, VaryServiceBy);
        }
    }
}