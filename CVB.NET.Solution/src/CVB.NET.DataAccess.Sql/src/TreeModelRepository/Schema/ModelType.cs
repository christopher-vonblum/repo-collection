namespace CVB.NET.DataAccess.Sql.TreeModelRepository.Schema
{
    using System;
    using System.Runtime.InteropServices;

    [Guid("00000000-0000-0000-0000-000000000001")]
    public class ModelType : ModelBase
    {
        public Type ClrType { get; }

        public ModelType()
        {
            Type = null;
            Guid = this.GetType().GUID;
        }

        public override ModelType GetTypeIdentity()
        {
            Guid typeGuid = this.GetType().GUID;

            return new ModelType
                   {
                       Guid = typeGuid,
                       Type = new ModelType
                              {
                                  Guid = new Guid()
                              }
                   };
        }
    }
}