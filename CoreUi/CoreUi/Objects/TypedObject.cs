using CoreUi.Model;
using ZeroFormatter;

namespace CoreUi.Objects
{
    [ZeroFormattable]
    public class TypedObject
    {
        [Index(0)]
        public virtual string T { get; set; }
        
        [Index(1)]
        public virtual byte[] Data { get; set; }
        
        [Index(2)]
        public virtual PropertyDefinition PropertyDefinition { get; set; }
    }
}