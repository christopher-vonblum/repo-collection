using System;
using CoreUi.Gtk.Base;
using CoreUi.Model;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using Gtk;

namespace CoreUi.Gtk.Widgets.Field
{
    public class DynamicEntryField : FieldBase
    {
        private Entry control;
        
        public DynamicEntryField(IObject controlState, PropertyDefinition propertyDefinition) : base(controlState, propertyDefinition)
        {
            control = new Entry();
            
            object def = null;

            if (ProxyFactory.IsSimpleField(PropertyDefinition.ClrType) && PropertyDefinition.ClrType != typeof(string))
            {
                def = ProxyFactory.CreateProxyOrValue(PropertyDefinition.ClrType);
                control.PlaceholderText = def?.ToString() ?? PropertyDefinition.ClrType.Name;
            }
            
            object data = ControlStateValue;
            
            if (data != null)
            {
                if (data is string s)
                {
                    control.Text = s;
                }
                else
                {
                    control.Text = data.ToString();
                }
            }
            
            this.Add(control);
        }

        public override void SaveControlState()
        {
            if (string.IsNullOrEmpty(control.Text) && ProxyFactory.IsSimpleField(PropertyDefinition.ClrType) && PropertyDefinition.ClrType != typeof(string))
            {
                this.ControlStateValue = ProxyFactory.CreateProxyOrValue(PropertyDefinition.ClrType);
                return;
            }

            if (this.PropertyDefinition.ClrType == typeof(Guid) && Guid.TryParse(control.Text, out Guid g))
            {
                this.ControlStateValue = g;
                return;
            }
            
            this.ControlStateValue = Convert.ChangeType(control.Text, this.PropertyDefinition.ClrType);
        }
    }
}