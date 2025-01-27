using System;

namespace CoreUi.Serialization
{
    public class SerializationPackager : ISerializationPackager
    {
        private readonly IContractSerializer _serializer;

        public SerializationPackager(IContractSerializer serializer)
        {
            _serializer = serializer;
        }
        
        public byte[] Pack<TWrapper>(TWrapper wrapper)
        {
            return _serializer.Serialize(
                typeof(SerializationWrapper), 
                new SerializationWrapper
                {
                    InnerData = _serializer.Serialize(typeof(TWrapper), wrapper),
                    WrapperType = typeof(TWrapper).AssemblyQualifiedName
                });
        }

        public TWrapper UnPack<TWrapper>(byte[] data)
        {
            var x = (SerializationWrapper)_serializer.Deserialize(
                typeof(SerializationWrapper), 
                data);
            
            return (TWrapper)_serializer.Deserialize(Type.GetType(x.WrapperType), data);
        }
    }
}