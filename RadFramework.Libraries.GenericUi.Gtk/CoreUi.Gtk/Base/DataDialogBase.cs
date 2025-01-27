using System;
using System.Collections.Generic;
using System.Linq;
using CoreUi.Gtk.Widgets.Field;
using CoreUi.Model;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using Gdk;
using Gtk;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Window = Gtk.Window;

namespace CoreUi.Gtk.Base
{
    public class DataDialogBase : DialogBase
    {
        public Type Contract;
        
        protected IObject source;
        
        protected IObject ControlState;
        protected HBox controlBox;
        protected GtkInteractionProvider InteractionProvider;
        protected VBox VBox;
        protected Dictionary<string, PropertyDefinition> Fields;
        protected Container dynamicBox;

        public DataDialogBase(
            IObject dataObject,
            IEnumerable<PropertyDefinition> fields,
            GtkInteractionProvider interactionProvider,
            Window parentDialog = null)
        {
            this.source = dataObject;
            this.ControlState = dataObject.Clone();
            
            this.TransientFor = parentDialog;
            this.Fields = new Dictionary<string, PropertyDefinition>(fields.ToDictionary(k => k.Name, v => v));
            InteractionProvider = interactionProvider;
            
            ScrolledWindow scroller = new ScrolledWindow();
            scroller.Expand = true;
            this.ContentArea.Add(scroller);

            Button saveButton = (Button)this.AddButton("Save", ResponseType.Ok);
            saveButton.Clicked += (sender, args) => SaveControlState();
            
            this.AddButton("Cancel", ResponseType.Cancel);
            this.TypeHint = WindowTypeHint.Normal;

            this.VBox = new VBox();
            scroller.Add(VBox);
            
            this.dynamicBox = new Box(Orientation.Vertical, 0);
            this.VBox.Add(dynamicBox);
            
            this.controlBox = new HBox();
            this.ContentArea.Add(controlBox);
        }

        public virtual void Init()
        {
            RenderBoxEntries();
            RenderControls();
            ShowAll();
        }

        protected virtual void RenderControls()
        {
            if (Contract != null)
            {
                Button exportControlState = new Button();
                this.controlBox.Add(exportControlState);

                exportControlState.Label = "Export Control State";
                exportControlState.Clicked += OnExportControlStateOnClicked;
            }
        }

        private void OnExportControlStateOnClicked(object sender, EventArgs args)
        {
            SaveControlState();

            IObjectProxy p = (IObjectProxy)ProxyFactory.CreateProxyOrValue(Contract);

            p.Object = ControlState;

            InteractionProvider.Messsage(JsonConvert.SerializeObject(p, Contract, new JsonSerializerSettings { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore }));
        }

        protected virtual void RenderBoxEntries()
        {
            CreateDynamicContainer();
            
            foreach (PropertyDefinition field in Fields.Values)
            {
                this.dynamicBox.Add(RenderField(field));
            }
        }

        protected virtual Container CreateDynamicContainer()
        {
            Box b = new Box(Orientation.Vertical, 0);
            VBox.Remove(dynamicBox);
            VBox.Add(b);
            dynamicBox = b;
            return b;
        }
        
        protected virtual Widget RenderField(PropertyDefinition property)
        {
            if (InteractionProvider.IsSimpleData(property.ClrType))
            {
                return new DynamicEntryField(this.ControlState, property);
            }
            else
            {
                return new ComplexReference(this.ControlState, property, InteractionProvider);
            }
        }
        
        public virtual void UpdateData()
        {
            source.ReplaceWith(ControlState);
        }

        protected virtual void SaveControlState()
        {
            foreach (Widget widget in this.dynamicBox.Children)
            {
                if (widget is FieldBase f)
                {
                    f.SaveControlState();
                }
            }
        }
    }
}