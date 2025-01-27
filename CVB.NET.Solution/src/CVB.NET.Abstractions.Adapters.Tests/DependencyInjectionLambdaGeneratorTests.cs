using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;
using Unity;

namespace CVB.NET.Abstractions.Adapters.Tests
{
    using System.Linq;
    using CVB.NET.Abstractions.Ioc.Injection.Lambda;
    using CVB.NET.Abstractions.Ioc.Injection.Parameter;

    using Ioc;
    using NUnit.Framework;
    using res;
    using Reflection.Caching.Cached;
    using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    [TestFixture]
    public class DependencyInjectionLambdaGeneratorTests
    {
        private IDependencyService container;

        [SetUp]
        public void Initialize()
        {
            container = new UnityDependencyService(new UnityContainer());
            
            container.Register(Service.For<IInjectMe, InjectMe>());
            container.Register(Service.For<IInjectMe, InjectMe>().Named("namedService1"));
            container.Register(Service.For<IInjectMe, InjectMe>().Named("namedService2"));
        }

        [Test]
        public void TestMethod1()
        {
            IDependencyInjectionLambdaGenerator gen = new DependencyInjectionLambdaGenerator();

            CachedType type = typeof (ConstructorInjectionTestModel);

            var lambda = gen.CreateConstructorInjectionLambda(
                                            type.Constructors.Single(),
                                            TestResources.GetDependencyNameForParameter);

            using (Arg.UseContextualResolver(this.container))
            {
                ConstructorInjectionTestModel constructorInjectionTestModel = (ConstructorInjectionTestModel)lambda();

                Assert.IsTrue(constructorInjectionTestModel.generalService == this.container.Resolve<IInjectMe>());
                Assert.IsTrue(constructorInjectionTestModel.namedService1 == this.container.Resolve<IInjectMe>("namedService1"));
                Assert.IsTrue(constructorInjectionTestModel.injectMeWithDifferentKey == this.container.Resolve<IInjectMe>("namedService2"));
            }
        }
    }
}
