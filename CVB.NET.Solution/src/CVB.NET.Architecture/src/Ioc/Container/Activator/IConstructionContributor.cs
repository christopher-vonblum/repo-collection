using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVB.NET.Abstractions.src.Ioc.Container.Activator
{
    using Abstractions.Ioc.Container.Model;

    public interface IConstructionContributor
    {
        ConstructorInjectionMode ContributionMode { get; }
        IReadOnlyDictionary<string, object> Contribute(IServiceRegistration registration);
    }
}
