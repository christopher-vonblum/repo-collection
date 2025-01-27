namespace CVB.NET.ReflectionCaching.Tests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Reflection.Caching;
    using Reflection.Caching.Aspect;
    using Reflection.Caching.Cached;
    using Reflection.Caching.Lookup;
    using Reflection.Caching.Wrapper;

    [TestClass]
    public class ReflectionCacheTest
    {
        public enum TestEnumType
        {
            A1,
            B2,
            C3
        }

        [TestMethod]
        public void GetCachedType_MatchesTestType()
        {
            CachedType cachedType = ReflectionCache.Get<CachedType>(TestReflectionCacheTypes.TestType);

            Assert.IsTrue(cachedType.Attributes.Length == 1);

            Assert.IsTrue(cachedType.Fields.Length == 2);

            Assert.IsTrue(cachedType.Properties.Length == 2);

            Assert.IsTrue(cachedType.Constructors.Length == 2);

            Assert.IsTrue(cachedType.Events.Length == 2);

            Assert.IsTrue(cachedType.Methods.Length == 2);

            Assert.IsTrue(cachedType.GenericTypeArguments.Length == 1);

            Assert.IsTrue(cachedType.DefaultConstructor != null);

            Assert.IsTrue(cachedType.GenericTypeDefinition.GenericTypeArguments.Length == 1);

            Assert.IsTrue(cachedType.Interfaces.Length == 1);
        }

        [TestMethod]
        public void GetCachedTypeView_MatchesTestType()
        {
            MetaDataTestInfoView cachedType = ReflectionCache.Get<MetaDataTestInfoView>(TestReflectionCacheTypes.TestType);

            Assert.IsTrue(cachedType.Properties.Length == 2);
        }

        [TestMethod]
        public void GetCachedInterface_MatchesTestType()
        {
            CachedInterfaceType cachedType = (CachedInterfaceType) ReflectionCache.Get(TestReflectionCacheTypes.TestInterfaceType);

            Assert.IsTrue(cachedType.Properties.Length == 2);

            Assert.IsTrue(cachedType.Events.Length == 2);

            Assert.IsTrue(cachedType.Methods.Length == 2);
        }

        [TestMethod]
        public void CachedEnum_MatchesTestType()
        {
            CachedEnum enm = (CachedEnum) ReflectionCache.Get(typeof (TestEnumType));

            string[] values = typeof (TestEnumType).GetEnumNames();

            CollectionAssert.AreEqual(enm.StringValues, values);
        }

        public class MetaDataTestInfoView : ReflectionInfoViewWrapperBase<Type>
        {
            [UseLookup(typeof (CachedTypeLookups), nameof(CachedTypeLookups.GetPublicImplementedProperties))]
            public CachedPropertyInfo[] Properties { get; }

            public MetaDataTestInfoView(Type reflectedType) : base(reflectedType)
            {
            }
        }
    }
}