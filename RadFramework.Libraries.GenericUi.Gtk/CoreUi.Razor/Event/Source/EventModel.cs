using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreUi.Razor.Event.Source
{
    public class EventModel
    {
        public Guid SourceClient { get; set; }
        public string Type { get; set; }
        public JToken Data { get; set; }
        
        [JsonIgnore]
        public ConcurrentDictionary<Guid, bool> ReceivedBy { get; set; }
    }
}