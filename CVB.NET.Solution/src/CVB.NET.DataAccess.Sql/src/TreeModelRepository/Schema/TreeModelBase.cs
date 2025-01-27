namespace CVB.NET.DataAccess.Sql.TreeModelRepository.Schema
{
    using System;
    using System.Runtime.InteropServices;
    using MetaData.Attributes;

    [Guid("9495ebd8-cb2f-309f-a919-337777669e3d")]
    public class TreeModelBase : ModelBase
    {
        [AllowNull]
        public TreeModelBase Parent { get; set; }

        [AllowNull]
        public string Name { get; set; }

        public override ModelBase GetRootIdentity()
        {
            Guid typeGuid = this.GetType().GUID;

            return new TreeModelBase
                   {
                       Guid = new Guid(),
                       Type = new ModelType
                              {
                                  Guid = typeGuid
                              },
                       Parent = new TreeModelBase
                                {
                                    Guid = new Guid()
                                }
                   };
        }
    }
}