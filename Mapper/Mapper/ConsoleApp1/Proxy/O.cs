using System;
using System.Collections.Generic;
using ZeroFormatter;

namespace ConsoleApp1
{
    [ZeroFormattable]
    public class O : IObject
    {
        [IndexAttribute(0)]
        public virtual ILazyDictionary<string, byte[]> Properties { get; set; } = new Dictionary<string, byte[]>().AsLazyDictionary();

        public O(){}
        
        [IgnoreFormat]
        public virtual object this[string propertyName]
        {
            get => Properties.ContainsKey(propertyName) ? Properties[propertyName] : null;
            set => Properties[propertyName] = value is byte[] b ? b : Serialize(value);
        }

        private static byte[] Serialize(object value)
        {
            if (value is IObjectProxy o)
            {
                return ZeroFormatterSerializer.Serialize<O>((O)o.Object);
            }
            
            return ZeroFormatterSerializer.NonGeneric.Serialize(value.GetType(), value);
        }

        public virtual object ConvertToReturnValue(Type propertyType, object value)
        {
            if (value == null && !propertyType.IsValueType)
            {
                return value;
            }else if (value == null)
            {
                return Activator.CreateInstance(propertyType);
            }
            
            return ZeroFormatterSerializer.NonGeneric.Deserialize(propertyType, (byte[])value);
        }

        public virtual object ConvertToInternalValue(Type propertyType, object value)
        {
            if (value == null)
            {
                return null;
                
            }
            return ZeroFormatterSerializer.NonGeneric.Serialize(propertyType, value);
        }
    }
}