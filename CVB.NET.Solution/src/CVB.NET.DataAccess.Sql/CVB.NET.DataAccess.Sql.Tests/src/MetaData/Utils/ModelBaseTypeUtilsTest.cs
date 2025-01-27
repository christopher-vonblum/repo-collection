namespace CVB.NET.DataAccess.Sql.Tests.MetaData.Utils
{
    using System.Linq;
    using DataAccess.MetaData.Utils;
    using DataAccess.MetaData.Views;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ModelBaseTypeUtilsTest
    {
        [TestMethod]
        public void InheritsFromModelRootBaseType_DetectsInheritance()
        {
            Assert.IsTrue(ModelBaseTypeUtils.InheritsFromModelRootBaseType(TestOrmTypeDefinitions.ModelBase), nameof(TestOrmTypeDefinitions.ModelBase));

            Assert.IsTrue(ModelBaseTypeUtils.InheritsFromModelRootBaseType(TestOrmTypeDefinitions.TreeModelBase), nameof(TestOrmTypeDefinitions.TreeModelBase));

            Assert.IsTrue(ModelBaseTypeUtils.InheritsFromModelRootBaseType(TestOrmTypeDefinitions.ModelType), nameof(TestOrmTypeDefinitions.ModelType));
        }

        [TestMethod]
        public void GetModelRootBaseType_ReturnsRightType()
        {
            Assert.AreEqual(TestOrmTypeDefinitions.ModelBase.InnerReflectionInfo, ModelBaseTypeUtils.GetModelRootBaseType(TestOrmTypeDefinitions.ModelBase).InnerReflectionInfo);

            Assert.AreEqual(TestOrmTypeDefinitions.ModelBase.InnerReflectionInfo, ModelBaseTypeUtils.GetModelRootBaseType(TestOrmTypeDefinitions.ModelType).InnerReflectionInfo);

            Assert.AreEqual(TestOrmTypeDefinitions.ModelBase.InnerReflectionInfo, ModelBaseTypeUtils.GetModelRootBaseType(TestOrmTypeDefinitions.TreeModelBase).InnerReflectionInfo);
        }

        [TestMethod]
        public void GetAllDependencyTypes_ReturnsAllDependencies_ModelBase()
        {
            bool rootTypeDependenciesOk = true;

            OrmTypeMetaDataInfoViewBase[] rootTypeDependencies = ModelBaseTypeUtils.GetAllDependencyTypes(TestOrmTypeDefinitions.ModelBase);

            rootTypeDependenciesOk &= rootTypeDependencies.Length == 1;

            Assert.IsTrue(rootTypeDependenciesOk);
        }

        [TestMethod]
        public void GetAllDependencyTypes_ReturnsAllDependencies_ModelType()
        {
            bool level1TypeDependenciesOk = true;

            OrmTypeMetaDataInfoViewBase[] level1TypeDependencies = ModelBaseTypeUtils.GetAllDependencyTypes(TestOrmTypeDefinitions.ModelType);

            level1TypeDependenciesOk &= level1TypeDependencies.Contains(TestOrmTypeDefinitions.ModelBase);

            level1TypeDependenciesOk &= level1TypeDependencies.Length == 2;

            Assert.IsTrue(level1TypeDependenciesOk);
        }
    }
}