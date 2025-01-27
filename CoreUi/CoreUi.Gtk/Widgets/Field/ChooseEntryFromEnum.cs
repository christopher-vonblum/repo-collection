using System;
using System.Linq;
using CoreUi.Gtk.Base;
using CoreUi.Model;
using CoreUi.Proxy;
using Gtk;

namespace CoreUi.Gtk.Widgets.Field
{
    public class ChooseEntryFromEnum : FieldBase
    {
        public ChooseEntryFromEnum(IObject controlState, PropertyDefinition propertyDefinition) : base(controlState, propertyDefinition)
        {
            string[] options = Enum.GetNames(propertyDefinition.ClrType);
            
            RadioButton prev = null;
            foreach (string option in options)
            {
                RadioButton choice = new RadioButton(option);
                choice.Name = $"{PropertyDefinition.Name}/{option}";
                choice.Label = option;

                if (prev != null)
                {
                    choice.JoinGroup(prev);
                }

                prev = choice;

                this.Add(choice);
            }
        }
        
        public override void SaveControlState()
        {
            foreach (Widget wd in this.Children)
            {
                if (wd is RadioButton r)
                {
                    RadioButton b = r.Group.Single(gr => gr.Active);
                    string sel = b.Name.Split("/")[1];
                    this.ControlStateValue = Enum.Parse(PropertyDefinition.ClrType, sel);
                    // only a single choice
                    break;
                }
            }
        }
    }
}