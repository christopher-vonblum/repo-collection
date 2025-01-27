namespace CVB.NET.DataAccess.Sql.TreeModelRepository.Schema
{
    using System;
    using System.Runtime.InteropServices;
    using MetaData.Attributes;
    using Reflection.Caching.Base;

    [ModelBase]
    [Guid("11111111-1111-1111-1111-111111111111")]
    public class ModelBase
    {
        [Identifier]
        public Guid Guid { get; set; }

        [ModelTypeDefinition]
        public ModelType Type
        {
            get
            {
                if (type == null)
                {
                    type = new DebuggableLazy<ModelType>(() => new ModelType {Guid = this.GetType().GUID});
                }

                return type.Value;
            }
            set { type = new DebuggableLazy<ModelType>(() => value); }
        }

        private DebuggableLazy<ModelType> type;

        public ModelBase()
        {
        }

        public virtual ModelBase GetRootIdentity()
        {
            return new ModelBase()
                   {
                       Guid = new Guid(),
                       Type = new ModelType()
                              {
                                  Guid = new Guid()
                              }
                   };
        }

        public virtual ModelType GetTypeIdentity()
        {
            Guid typeGuid = this.GetType().GUID;

            return new ModelType()
                   {
                       Guid = typeGuid,
                       Type = new ModelType()
                              {
                                  Guid = new Guid()
                              }
                   };
        }
    }
}