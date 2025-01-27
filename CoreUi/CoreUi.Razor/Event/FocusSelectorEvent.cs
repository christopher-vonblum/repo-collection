using CoreUi.Razor.Event.Base;

namespace CoreUi.Razor.Event
{
    public class FocusSelectorEvent : IFilterForSourceClient
    {
        public string Selector { get; set; }
    }
}