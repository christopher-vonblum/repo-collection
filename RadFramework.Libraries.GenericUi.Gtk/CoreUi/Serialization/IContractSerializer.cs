using System;

namespace CoreUi.Serialization
{
    public interface IContractSerializer
    {
        byte[] Serialize(Type t, object o);
        
        
        object Deserialize(Type t, byte[] d);
    }
}