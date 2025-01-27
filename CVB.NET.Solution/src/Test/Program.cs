using System;
using CVB.NET.DataAccess.Sql.TreeModelRepository.Schema;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TreeModelBase model = new TreeModelBase
            {
                Guid = new Guid("EADE7A1B-FC2B-4FD6-99EF-7758EB841887"),
                Name = "Root",
                Parent = new TreeModelBase()
            };

            BootstrapContext.BinaryTreeDataSource.Create(model);

            BootstrapContext.BinaryTreeDataSource.Create(new TreeModelBase
            {
                Guid = new Guid("E8CC1E0F-D8D2-43E9-9D44-916F31624130"),
                Name = "TestModel",
                Parent = new TreeModelBase
                {
                    Guid = new Guid("EADE7A1B-FC2B-4FD6-99EF-7758EB841887")
                }
            });



            //TreeModelBase testModel = BootstrapContext.BinaryTreeDataSource.Read();

            Console.ReadLine();
        }

    }
}
