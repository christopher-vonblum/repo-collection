using System.Collections.Generic;
using CoreUi.Model;
using CoreUi.Razor.Event.Base;

namespace CoreUi.Razor.Event
{
    public class DialogRequestedEvent : TypedResultInteractionEvent
    {
        public string DialogTitle { get; set; }
        
    }
}