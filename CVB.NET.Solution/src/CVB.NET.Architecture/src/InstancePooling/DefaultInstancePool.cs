namespace CVB.NET.Architecture.src.Pooling
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Abstractions.Ioc.Container;

    public class DefaultInstancePool<TService> : IInstancePool<TService> where TService : class
    {
        private readonly IInstancePoolDriver driver;

        public DefaultInstancePool(
            IIocContainer container,
            IReadOnlyDictionary<string, Func<object>> varyInstanceBy,
            IReadOnlyDictionary<string, Func<object>> varyServiceBy = null) 
            : this(CreateIocDriver(container, varyInstanceBy, varyServiceBy))
        {
        }

        public DefaultInstancePool(
            IIocContainer container,
            IReadOnlyDictionary<string, Func<object>> varyInstanceBy,
            IReadOnlyDictionary<string, object> varyServiceBy = null)
            : this(CreateIocDriver(container, varyInstanceBy, varyServiceBy))
        {
        }
        
        public DefaultInstancePool(
            IInstancePoolDriver driver)
        {
            this.driver = driver;
        }

        private static IInstancePoolDriver CreateIocDriver(IIocContainer container, IReadOnlyDictionary<string, Func<object>> varyInstanceBy, IReadOnlyDictionary<string, Func<object>> varyServiceBy = null)
        {
            return new DefaultIocPoolDriver(container, typeof(TService), varyInstanceBy, varyServiceBy);
        }

        private static IInstancePoolDriver CreateIocDriver(IIocContainer container, IReadOnlyDictionary<string, Func<object>> varyInstanceBy, IReadOnlyDictionary<string, object> varyServiceBy = null)
        {
            return new DefaultIocPoolDriver(container, typeof(TService), varyInstanceBy, varyServiceBy);
        }

        public virtual TService GetInstance()
        {
            return (TService)driver.GetCurrentInstance();
        }
    }
}