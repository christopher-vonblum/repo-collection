using CoreUi.Gtk.Base;
using CoreUi.Model;
using CoreUi.Proxy;

namespace CoreUi.Gtk.Widgets.Action
{
    public class InvokeAction : FieldBase
    {
        public InvokeAction(IObject controlState, PropertyDefinition propertyDefinition) : base(controlState, propertyDefinition)
        {
        }
        public override void SaveControlState()
        {
            throw new System.NotImplementedException();
        }
    }
}