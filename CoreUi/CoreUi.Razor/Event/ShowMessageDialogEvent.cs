using CoreUi.Razor.Event.Base;

namespace CoreUi.Razor
{
    public class ShowMessageDialogEvent : TypedResultInteractionEvent
    {
        public string DialogTitle { get; set; }
        public string Message { get; set; }
    }
}