using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreUi.Razor.Dialog
{
    public class DialogControlValue
    {
        [JsonIgnore]
        public Guid DialogId { get; set; }
        
        public string Name { get; set; }
        public JToken Value { get; set; }
    }
}