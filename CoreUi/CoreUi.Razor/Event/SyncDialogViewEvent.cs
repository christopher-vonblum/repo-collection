using CoreUi.Razor.Dialog;
using CoreUi.Razor.Event.Base;

namespace CoreUi.Razor.Event
{
    public class SyncDialogViewEvent : IFilterForSourceClient
    {
        public DialogViewState Dialog { get; set; }
    }
}