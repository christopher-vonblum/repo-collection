using System.Collections.Generic;
using CoreUi.Model;
using CoreUi.Razor.Event.Base;

namespace CoreUi.Razor.Event
{
    public class DecisionRequestedEvent : InteractionEvent
    {
        public string DialogTitle { get; set; }
        public IEnumerable<PropertyDefinition> Properties { get; set; }
    }
}