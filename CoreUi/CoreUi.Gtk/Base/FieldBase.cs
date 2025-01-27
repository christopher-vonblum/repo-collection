using System.Linq;
using CoreUi.Attributes;
using CoreUi.Model;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using Gtk;

namespace CoreUi.Gtk.Base
{
    public abstract class FieldBase : Box
    {
        public Label Label { get; private set; }

        protected object ControlStateValue
        {
            get
            {
                return this.ControlState[PropertyDefinition];
            }
            set
            {
                if (this.ControlState[PropertyDefinition.Name] == null)
                {
                    this.ControlState.CreateProperty(PropertyDefinition);
                }
                
                this.ControlState[PropertyDefinition] = value;
            }
        }

        protected readonly IObject ControlState;
        protected readonly PropertyDefinition PropertyDefinition;

        public FieldBase(IObject controlState, PropertyDefinition propertyDefinition) : base(Orientation.Vertical, 0)
        {
            this.Name = propertyDefinition.Name;
            ControlState = controlState;
            PropertyDefinition = propertyDefinition;
            Init();
        }

        protected virtual void Init()
        {
            RenderLabel();
        }
        
        protected virtual void RenderLabel()
        {
            Label label = new Label();

            if (PropertyDefinition.Attributes.OfType<DescriptionAttribute>().FirstOrDefault() is DescriptionAttribute a)
            {
                label.Text = $"{PropertyDefinition.Name} :    ( {a.Description} )";
            }
            else
            {
                label.Text = $"{PropertyDefinition.Name} :";
            }
            
            label.MarginTop = 10;
            label.Halign = Align.Start;
            this.Add(label);
            Label = label;
        }
        
        protected virtual Widget ToFieldControl(Widget e, PropertyDefinition propertyName)
        {
            e.MarginTop = 5;
            e.Hexpand = true;
            e.TooltipText = propertyName.ClrType.IsValueType ? ProxyFactory.CreateProxyOrValue(propertyName.ClrType).ToString() : string.Empty;
            return e;
        }

        public abstract void SaveControlState();
    }
}