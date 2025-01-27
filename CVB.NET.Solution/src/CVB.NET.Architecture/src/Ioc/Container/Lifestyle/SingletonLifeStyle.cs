namespace CVB.NET.Abstractions.Ioc.Container.Lifestyle
{
    using System.Collections.Generic;
    using Model;

    public class SingletonLifeStyle : IServiceInstanceContainer
    {
        public IReadOnlyDictionary<string, object> GetInstanceVarianceModifiers(IServiceRegistrationKey serviceRegistrationKey)
        {
            return new Dictionary<string, object>();
        }
    }
}