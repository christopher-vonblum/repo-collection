using CoreUi.Gtk.Base;
using CoreUi.Model;
using CoreUi.Proxy;
using Gtk;

namespace CoreUi.Gtk.Widgets.Field
{
    public class CheckBool : FieldBase
    {
        private CheckButton control;

        public CheckBool(IObject controlState, PropertyDefinition propertyDefinition) : base(controlState, propertyDefinition)
        {
            this.control = new CheckButton();
            control.Active = (bool) (this.ControlStateValue ?? default(bool));
            this.Add(control);
        }

        public override void SaveControlState()
        {
            this.ControlStateValue = control.Active;
        }
    }
}