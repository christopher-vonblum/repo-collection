namespace CVB.NET.DataAccess.Sql.Tests
{
    using System;
    using Context;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TreeModelRepository.Schema;

    [TestClass]
    public class GenericCrudRepositoryTests
    {
        [TestMethod]
        public void Does_Not_Exist_Before_Creation()
        {
            TreeModelBase testModel =
                new TreeModelBase
                {
                    Guid = new Guid("{8a2032be-cf93-4267-83f1-076bce37b734}")
                };

            Assert.IsFalse(SqlTestContext.Core.Exists(testModel));
        }

        [TestMethod]
        public void Create_Test()
        {
            TreeModelBase testModel =
                new TreeModelBase
                {
                    Guid = new Guid("{8a2032be-cf93-4267-83f1-076bce37b734}"),
                    Name = "Root",
                    Parent = new TreeModelBase()
                };

            SqlTestContext.Core.Create(testModel);
        }

        [TestMethod]
        public void Does_Exist_After_Creation()
        {
            TreeModelBase testModel =
                new TreeModelBase
                {
                    Guid = new Guid("{8a2032be-cf93-4267-83f1-076bce37b734}")
                };

            Assert.IsTrue(SqlTestContext.Core.Exists(testModel));
        }

        [TestMethod]
        public void Update_Test()
        {
            TreeModelBase testModel =
                new TreeModelBase
                {
                    Guid = new Guid("{8a2032be-cf93-4267-83f1-076bce37b734}"),
                    Name = "RootNode",
                    Parent = new TreeModelBase()
                };

            SqlTestContext.Core.Update(testModel);
        }

        [TestMethod]
        public void Read_Test()
        {
            TreeModelBase testModel =
                new TreeModelBase()
                {
                    Guid = new Guid("{8a2032be-cf93-4267-83f1-076bce37b734}")
                };

            SqlTestContext.Core.Read(testModel);

            Assert.AreEqual(testModel.Name, "RootNode");
            Assert.AreEqual(testModel.Type.Guid, typeof (TreeModelBase).GUID);
        }

        [TestMethod]
        public void Delete_Test()
        {
            TreeModelBase testModel =
                new TreeModelBase
                {
                    Guid = new Guid("{8a2032be-cf93-4267-83f1-076bce37b734}")
                };

            SqlTestContext.Core.Delete(testModel);
        }

        [TestMethod]
        public void Does_Not_Exist_After_Deletion()
        {
            TreeModelBase testModel =
                new TreeModelBase
                {
                    Guid = new Guid("{8a2032be-cf93-4267-83f1-076bce37b734}")
                };

            Assert.IsFalse(SqlTestContext.Core.Exists(testModel));
        }
    }
}