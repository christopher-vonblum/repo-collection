using System;
using CoreUi.Gtk.Base;
using CoreUi.Model;
using CoreUi.Proxy;
using Gtk;

namespace CoreUi.Gtk.Widgets.Field
{
    public class CalendarDateTime : FieldBase
    {
        private Calendar control;

        public CalendarDateTime(IObject controlState, PropertyDefinition propertyDefinition) : base(controlState, propertyDefinition)
        {
            this.control = new Calendar();
            control.Date = (DateTime) (this.ControlStateValue ?? DateTime.Now);
            this.Add(control);
        }

        public override void SaveControlState()
        {
            this.ControlStateValue = control.Date;
        }
    }
}