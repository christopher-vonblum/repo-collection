namespace CVB.NET.DataAccess.Tests.MetaData.Utils
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
            Assert.IsTrue(ModelBaseTypeUtils.InheritsFromModelRootBaseType(TestOrmTypeDefinitions.RootType), nameof(TestOrmTypeDefinitions.MyModelRootBase));

            Assert.IsFalse(ModelBaseTypeUtils.InheritsFromModelRootBaseType(TestOrmTypeDefinitions.SomeTypeWithAttributes), nameof(TestOrmTypeDefinitions.SomeTypeWithAttributes));

            Assert.IsTrue(ModelBaseTypeUtils.InheritsFromModelRootBaseType(TestOrmTypeDefinitions.TestModelType), nameof(TestOrmTypeDefinitions.MyModelRootBase));

            Assert.IsTrue(ModelBaseTypeUtils.InheritsFromModelRootBaseType(TestOrmTypeDefinitions.Level1Type), nameof(TestOrmTypeDefinitions.MyModelTypeLevel1));

            Assert.IsTrue(ModelBaseTypeUtils.InheritsFromModelRootBaseType(TestOrmTypeDefinitions.Level2Type), nameof(TestOrmTypeDefinitions.MyModelTypeLevel2));

            Assert.IsTrue(ModelBaseTypeUtils.InheritsFromModelRootBaseType(TestOrmTypeDefinitions.Level3Type), nameof(TestOrmTypeDefinitions.MyModelTypeLevel3));
        }

        [TestMethod]
        public void GetModelRootBaseType_ReturnsNullIfWrongType()
        {
            Assert.AreEqual(null, ModelBaseTypeUtils.GetModelRootBaseType(TestOrmTypeDefinitions.SomeTypeWithAttributes));

            Assert.AreEqual(null, ModelBaseTypeUtils.GetModelRootBaseType(TestOrmTypeDefinitions.ObjectType));
        }

        [TestMethod]
        public void GetModelRootBaseType_ReturnsRightType()
        {
            Assert.AreEqual(TestOrmTypeDefinitions.RootType.InnerReflectionInfo, ModelBaseTypeUtils.GetModelRootBaseType(TestOrmTypeDefinitions.RootType).InnerReflectionInfo);

            Assert.AreEqual(TestOrmTypeDefinitions.OtherRootType.InnerReflectionInfo, ModelBaseTypeUtils.GetModelRootBaseType(TestOrmTypeDefinitions.OtherRootType).InnerReflectionInfo);

            Assert.AreEqual(TestOrmTypeDefinitions.RootType.InnerReflectionInfo, ModelBaseTypeUtils.GetModelRootBaseType(TestOrmTypeDefinitions.Level1Type).InnerReflectionInfo);

            Assert.AreEqual(TestOrmTypeDefinitions.RootType.InnerReflectionInfo, ModelBaseTypeUtils.GetModelRootBaseType(TestOrmTypeDefinitions.Level2Type).InnerReflectionInfo);

            Assert.AreEqual(TestOrmTypeDefinitions.RootType.InnerReflectionInfo, ModelBaseTypeUtils.GetModelRootBaseType(TestOrmTypeDefinitions.Level3Type).InnerReflectionInfo);
        }

        [TestMethod]
        public void GetAllDependencyTypes_ReturnsAllDependencies()
        {
            GetAllDependencyTypes_ReturnsAllDependencies_RootType();

            GetAllDependencyTypes_ReturnsAllDependencies_Level1Type();

            GetAllDependencyTypes_ReturnsAllDependencies_Level2Type();

            GetAllDependencyTypes_ReturnsAllDependencies_Level3Type();
        }

        private void GetAllDependencyTypes_ReturnsAllDependencies_RootType()
        {
            bool rootTypeDependenciesOk = true;

            OrmTypeMetaDataInfoViewBase[] rootTypeDependencies = ModelBaseTypeUtils.GetAllDependencyTypes(TestOrmTypeDefinitions.RootType);

            rootTypeDependenciesOk &= rootTypeDependencies.Length == 0;

            Assert.IsTrue(rootTypeDependenciesOk);
        }

        private void GetAllDependencyTypes_ReturnsAllDependencies_Level1Type()
        {
            bool level1TypeDependenciesOk = true;

            OrmTypeMetaDataInfoViewBase[] level1TypeDependencies = ModelBaseTypeUtils.GetAllDependencyTypes(TestOrmTypeDefinitions.Level1Type);

            level1TypeDependenciesOk &= level1TypeDependencies.Contains(TestOrmTypeDefinitions.RootType);

            level1TypeDependenciesOk &= level1TypeDependencies.Length == 1;

            Assert.IsTrue(level1TypeDependenciesOk);
        }

        private void GetAllDependencyTypes_ReturnsAllDependencies_Level2Type()
        {
            bool level2TypeDependenciesOk = true;

            OrmTypeMetaDataInfoViewBase[] level2TypeDependencies = ModelBaseTypeUtils.GetAllDependencyTypes(TestOrmTypeDefinitions.Level2Type);

            level2TypeDependenciesOk &= level2TypeDependencies.Contains(TestOrmTypeDefinitions.RootType);

            level2TypeDependenciesOk &= level2TypeDependencies.Contains(TestOrmTypeDefinitions.OtherRootType);

            level2TypeDependenciesOk &= level2TypeDependencies.Contains(TestOrmTypeDefinitions.Level1Type);

            level2TypeDependenciesOk &= level2TypeDependencies.Contains(TestOrmTypeDefinitions.TestModelType);

            level2TypeDependenciesOk &= !level2TypeDependencies.Contains(TestOrmTypeDefinitions.SomeTypeWithAttributes);

            level2TypeDependenciesOk &= level2TypeDependencies.Length == 4;

            Assert.IsTrue(level2TypeDependenciesOk);
        }

        private void GetAllDependencyTypes_ReturnsAllDependencies_Level3Type()
        {
            bool level3TypeDependenciesOk = true;

            OrmTypeMetaDataInfoViewBase[] level3TypeDependencies = ModelBaseTypeUtils.GetAllDependencyTypes(TestOrmTypeDefinitions.Level3Type);

            level3TypeDependenciesOk &= level3TypeDependencies.Contains(TestOrmTypeDefinitions.RootType);

            level3TypeDependenciesOk &= level3TypeDependencies.Contains(TestOrmTypeDefinitions.OtherRootType);

            level3TypeDependenciesOk &= level3TypeDependencies.Contains(TestOrmTypeDefinitions.TestModelType);

            level3TypeDependenciesOk &= level3TypeDependencies.Contains(TestOrmTypeDefinitions.Level1Type);

            level3TypeDependenciesOk &= level3TypeDependencies.Contains(TestOrmTypeDefinitions.Level2Type);

            level3TypeDependenciesOk &= level3TypeDependencies.Length == 5;

            Assert.IsTrue(level3TypeDependenciesOk);
        }
    }
}