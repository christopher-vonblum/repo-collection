using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreUi.Razor.Dialog
{
    public class DialogControlState
    {
        [JsonIgnore]
        public Guid DialogId { get; set; }
        
        public JToken Data { get; set; }
    }
}