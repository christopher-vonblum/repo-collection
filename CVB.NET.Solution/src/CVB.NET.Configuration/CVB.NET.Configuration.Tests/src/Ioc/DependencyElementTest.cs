namespace CVB.NET.Configuration.Tests.Ioc
{
    using System.Configuration;
    using Configuration.Ioc;
    using Configuration.Ioc.ConfigurationElements;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NET.Ioc.Provider;

    [TestClass]
    public class DependencyElementTest
    {
        [TestMethod]
        public void GetServiceLocationSectionTest()
        {
            ServiceLocationSection testSection = (ServiceLocationSection) ConfigurationManager.GetSection("sqlTest");

            Assert.IsTrue(testSection.ArrayTest.Length == 2);

            Assert.IsTrue(testSection != null);

            IIocProvider configIocProvider = new AppConfigIocProvider("sqlTest");

            Assert.IsTrue(configIocProvider != null);
        }
    }
}