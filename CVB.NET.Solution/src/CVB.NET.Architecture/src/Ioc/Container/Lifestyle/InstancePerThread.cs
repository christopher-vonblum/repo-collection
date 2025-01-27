namespace CVB.NET.Abstractions.Ioc.Container.Lifestyle
{
    using System.Collections.Generic;
    using System.Threading;
    using Model;

    public class InstancePerThread : IServiceInstanceContainer
    {
        public IReadOnlyDictionary<string, object> GetInstanceVarianceModifiers(IServiceRegistrationKey serviceRegistrationKey)
        {
            return new Dictionary<string, object>
                   {
                       { "Thread", Thread.CurrentThread }
                   };
        }
    }
}
