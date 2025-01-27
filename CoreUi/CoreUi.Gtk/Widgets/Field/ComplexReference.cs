using System;
using CoreUi.Gtk.Base;
using CoreUi.Gtk.Helpers;
using CoreUi.Model;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using Gtk;

namespace CoreUi.Gtk.Widgets.Field
{
    public class ComplexReference : FieldBase
    {
        private readonly GtkInteractionProvider _interactionProvider;

        public ComplexReference(IObject controlState, PropertyDefinition propertyDefinition, GtkInteractionProvider interactionProvider) : base(controlState, propertyDefinition)
        {
            _interactionProvider = interactionProvider;
            Button editComplexField = new Button();
            this.Add(editComplexField);
            
            editComplexField.Label = TypeNameUtils.FriendlyTypeName(PropertyDefinition.ClrType);
            editComplexField.Xalign = 0;
            editComplexField.Clicked += EditComplexFieldOnClicked(interactionProvider);
        }

        private EventHandler EditComplexFieldOnClicked(GtkInteractionProvider interactionProvider)
        {
            return (sender, args) =>
            {
                object data = this.ControlStateValue;
                IObjectProxy proxy = (IObjectProxy)ProxyFactory.CreateProxyOrValue(PropertyDefinition.ClrType);

                if (data != null)
                {
                    proxy.Object = (IObject) data;
                }

                if (interactionProvider.RequestInput(PropertyDefinition.ClrType, proxy, out object res))
                {
                    this.ControlStateValue = res;
                }
            };
        }

        public override void SaveControlState()
        {
        }
    }
}