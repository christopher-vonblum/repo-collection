using CoreUi.Proxy;
using ZeroFormatter;

namespace CoreUi.Serialization
{
    [ZeroFormattable]
    public class SerializationWrapper
    {
        [Index(0)]
        public virtual string WrapperType { get; set; }
        
        [Index(1)]
        public virtual byte[] InnerData { get; set; }
    }
}