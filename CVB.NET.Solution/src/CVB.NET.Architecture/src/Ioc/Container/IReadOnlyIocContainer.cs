namespace CVB.NET.Abstractions.Ioc.Container
{
    using System;
    using System.Collections.Generic;
    using Model;

    public interface IReadOnlyIocContainer
    {
        bool HasServiceRegistration<TService>(
            IReadOnlyDictionary<string, object> varyServiceBy = null)
                where TService : class;

        bool HasServiceRegistration(
            Type tService,
            IReadOnlyDictionary<string, object> varyServiceBy = null);

        TService CreateInstance<TService>(
            IReadOnlyDictionary<string, object> varyServiceBy = null)
                where TService : class;

        object CreateInstance(
            Type tService,
            IReadOnlyDictionary<string, object> varyServiceBy = null);

        IEnumerable<IServiceRegistration> QueryServiceRegistrations();
    }
}