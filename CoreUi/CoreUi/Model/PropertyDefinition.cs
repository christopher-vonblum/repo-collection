using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreUi.Attributes;
using Newtonsoft.Json;
using ZeroFormatter;

namespace CoreUi.Model
{
    [ZeroFormattable]
    public class PropertyDefinition
    {
        [IgnoreFormat]
        public CoreUiAttribute[] Attributes => ClrDeclaringType?
                                                .GetProperty(Name.Split(".").Last())
                                                .GetCustomAttributes(false)
                                                .OfType<CoreUiAttribute>()
                                                .ToArray() ?? new CoreUiAttribute[0];

        [IndexAttribute(0)]
        [JsonIgnore]
        public virtual string type { get; set; }
        
        [IgnoreFormat]
        public Type ClrType {
            get { return Type.GetType(type); }
            set { type = value.AssemblyQualifiedName; } }
        
        [IndexAttribute(1)]
        [JsonIgnore]
        public virtual string declaringType { get; set; }
        
        [IgnoreFormat]
        public Type ClrDeclaringType {
            get { return declaringType == null ? null : Type.GetType(declaringType); }
            set { declaringType = value?.AssemblyQualifiedName; } }
        
        [IndexAttribute(2)]
        public virtual string Name { get; set; }
    }
}