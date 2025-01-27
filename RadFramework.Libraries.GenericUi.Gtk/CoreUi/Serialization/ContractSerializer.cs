using System;
using ZeroFormatter;

namespace CoreUi.Serialization
{
    public class ContractSerializer : IContractSerializer
    {
        public byte[] Serialize(Type t, object o)
        {
            return ZeroFormatterSerializer.NonGeneric.Serialize(t, o);
        }

        public object Deserialize(Type t, byte[] d)
        {
            return ZeroFormatterSerializer.NonGeneric.Deserialize(t, d);
        }
    }
}