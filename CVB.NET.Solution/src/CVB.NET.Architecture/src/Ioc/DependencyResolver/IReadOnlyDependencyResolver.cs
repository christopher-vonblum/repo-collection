using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVB.NET.Abstractions.src.Ioc.DependencyResolver
{
    using Abstractions.Ioc.Provider;
    using Abstractions.Ioc.ServiceLocator;

    interface IReadOnlyDependencyResolver : IReadOnlyIocProvider, IReadOnlyServiceLocator, IReadOnlyNamedServiceLocator
    {
    }

    interface IDependencyResolver : IIocProvider, IServiceLocator, INamedServiceLocator
    {
        
    }

    public interface IIocService
    {
        IReadOnlyServiceLocator ServiceLocator { get; }
        IReadOnlyNamedServiceLocator NamedServiceLocator { get; }
        IReadOnlyIocProvider IocProvider { get; }
    }

    interface IDependent
    {
        IEnumerable<IDependency> Dependencies { get; } 
    }

    internal interface IDependency
    {
    }
}
