using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;
using Unity;

namespace CVB.NET.Architecture.Tests.Facades
{
    using Abstractions.Adapters.Ioc;
    using Architecture.Facades;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Reflection.Caching.Cached;
    using TestComponents;

    [TestClass]
    public class FacadeProxyTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            IDependencyService container = new UnityDependencyService(new UnityContainer());

            CachedType component1Type = typeof (Component1);

            container.Register(Service.For<IComponent1, Component1>().Named("Component1"));

            CachedType component2Type = typeof(Component2);

            container.Register(Service.For<IComponent2, Component2>().Named("Component2"));

            CachedType component3Type = typeof(Component3);

            container.Register(Service.For<IComponent3, Component3>().Named("Component3"));

            ITestFacade facade = new FacadeProxyGenerator(container, new IocFacadeCache(container)).Create<ITestFacade>();

            Assert.IsTrue(facade.Component1.GetType() == component1Type);
            Assert.IsTrue(facade.Component2.GetType() == component2Type);
            Assert.IsTrue(facade.SubFacade.Component3.GetType() == component3Type);
        }
    }
}
