using System;
using CoreUi.Gtk.Base;
using CoreUi.Model;
using CoreUi.Proxy;
using Gtk;

namespace CoreUi.Gtk.Widgets.Field
{
    public class CheckFlagsOnEnum : FieldBase
    {
        public CheckFlagsOnEnum(IObject controlState, PropertyDefinition propertyDefinition) : base(controlState, propertyDefinition)
        {
            string[] options = Enum.GetNames(propertyDefinition.ClrType);
            
            foreach (string option in options)
            {
                CheckButton o = new CheckButton();

                o.Name = $"{PropertyDefinition.Name}/{option}";
                o.Label = option;

                this.Add(o);
            }
        }
        
        public override void SaveControlState()
        {
            int enm = 0;

            foreach (Widget wd in this.Children)
            {
                if (wd is CheckButton chk && chk.Active)
                {
                    // assign all choices to flags value
                    enm |= (int) Enum.Parse(PropertyDefinition.ClrType, chk.Name.Split("/")[1]);
                }
            }

            // convert flags value back to runtime value
            this.ControlStateValue = Enum.ToObject(PropertyDefinition.ClrType, enm);
        }
    }
}