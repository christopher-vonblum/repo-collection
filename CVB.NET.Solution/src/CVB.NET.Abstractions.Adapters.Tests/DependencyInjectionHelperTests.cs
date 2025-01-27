using CVB.NET.Abstractions.Ioc;
using CVB.NET.Abstractions.Ioc.Registration;
using Unity;

namespace CVB.NET.Abstractions.Adapters.Tests
{
    using System;
    using System.Linq;
    using CVB.NET.Abstractions.Ioc.Injection;
    using CVB.NET.Abstractions.Ioc.Injection.Lambda;
    using CVB.NET.Abstractions.Ioc.Injection.Parameter;

    using Ioc;
    using NUnit.Framework;
    using res;
    using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    [TestFixture]
    public class DependencyInjectionHelperTests
    {
        private IDependencyService container;
        private IDependencyInjectionHelper helper;
        private IDisposable resolverContext;

        [SetUp]
        public void Initialize()
        {
            container = new UnityDependencyService(new UnityContainer());

            container.Register(Service.For<IInjectMe, InjectMe>());
            container.Register(Service.For<IInjectMe,InjectMe>().Named("namedService1"));
            container.Register(Service.For<IInjectMe, InjectMe>().Named("namedService2"));

            helper = new DependencyInjectionHelper(new DependencyInjectionLambdaGenerator());

            resolverContext = Arg.UseContextualResolver(this.container);
        }

        [TearDown]
        public void Unload()
        {
            resolverContext.Dispose();
        }

        [Test]
        public void ManualConstruction_PlaceholdersGetResolved()
        {
            ConstructorInjectionTestModel instance = helper.ManualConstruction(Arg.CurrentResolver, () => new ConstructorInjectionTestModel(Arg<IInjectMe>.Dependency(), Arg<IInjectMe>.Dependency(), Arg<IInjectMe>.Dependency()));

            Assert.IsTrue(instance.generalService == instance.injectMeWithDifferentKey && instance.injectMeWithDifferentKey == instance.namedService1);
        }

        [Test]
        public void ManualInjection_PlaceholdersGetResolved()
        {
            PropertyInjectionTestModel instance = new PropertyInjectionTestModel();
                    
            helper.ManualInjection(Arg.CurrentResolver, instance, i => { i.DefaultService = Arg<IInjectMe>.Dependency();
                                                                                    i.InjectMeWithDifferentKey = Arg<IInjectMe>.Dependency();
                                                                                    i.NamedService1 = Arg<IInjectMe>.Dependency();
            });

            Assert.IsTrue(instance.DefaultService == instance.InjectMeWithDifferentKey && instance.InjectMeWithDifferentKey == instance.NamedService1);
        }

        [Test]
        public void AutoInjectConstructor_InjectsDefaultServices()
        {
            ConstructorInjectionTestModel instance = (ConstructorInjectionTestModel)helper.AutoConstruct(Arg.CurrentResolver, typeof(ConstructorInjectionTestModel), p => p.Constructors.FirstOrDefault(), TestResources.GetDependencyNameForParameter);

            Assert.IsTrue(instance.generalService == this.container.Resolve<IInjectMe>());
            Assert.IsTrue(instance.namedService1 == this.container.Resolve<IInjectMe>("namedService1"));
            Assert.IsTrue(instance.injectMeWithDifferentKey == this.container.Resolve<IInjectMe>("namedService2"));
        }

        [Test]
        public void AutoInjectProperties_InjectsDefaultServices()
        {
            PropertyInjectionTestModel instance = new PropertyInjectionTestModel();

            helper.AutoInjectProperties(Arg.CurrentResolver, instance, p => true, TestResources.GetDependencyNameForProperty);

            Assert.IsTrue(instance.DefaultService == this.container.Resolve<IInjectMe>());
            Assert.IsTrue(instance.NamedService1 == this.container.Resolve<IInjectMe>("namedService1"));
            Assert.IsTrue(instance.InjectMeWithDifferentKey == this.container.Resolve<IInjectMe>("namedService2"));
        }

        [Test]
        public void AutoInjectMethods_InjectsDefaultServices()
        {
            MethodInjectionModel instance = new MethodInjectionModel();

            helper.AutoInjectMethods(Arg.CurrentResolver, instance, p => true, TestResources.GetDependencyNameForParameter);

            Assert.IsTrue(instance.generalService == this.container.Resolve<IInjectMe>());
            Assert.IsTrue(instance.namedService1 == this.container.Resolve<IInjectMe>("namedService1"));
            Assert.IsTrue(instance.injectMeWithDifferentKey == this.container.Resolve<IInjectMe>("namedService2"));
        }
    }
}
