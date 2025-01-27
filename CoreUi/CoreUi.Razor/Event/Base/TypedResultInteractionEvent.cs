using System.Collections.Generic;
using CoreUi.Model;

namespace CoreUi.Razor.Event.Base
{
    public class TypedResultInteractionEvent : InteractionEvent
    {
        public string ModelType { get; set; }
        public IEnumerable<PropertyDefinition> Properties { get; set; }
    }
}