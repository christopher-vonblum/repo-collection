using System;
using System.Collections.Generic;
using CoreUi.Model;

namespace CoreUi.Razor.Dialog
{
    public class DialogModel
    {
        public Guid DialogId { get; set; }
        public IEnumerable<PropertyDefinition> Properties { get; set; }
        public DialogViewState ViewState { get; set; }
        public DialogControlState ControlState { get; set; }
    }
}