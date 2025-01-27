using System;
using System.Collections.Generic;
using CoreUi.Model;
using CoreUi.Proxy;
using Gtk;

namespace CoreUi.Gtk.Dialog
{
    public class RequestDecisionDialog : CollectionInputDialog
    {
        public PropertyDefinition ChoiceProperty { get; private set; }

        public RequestDecisionDialog(Type elementType, IObject dataObject, GtkInteractionProvider gtkInteractionProvider, Window parentDialog = null) : base(elementType, dataObject, gtkInteractionProvider, parentDialog)
        {
        }

        public override void Init()
        {
            base.Init();
            this.elementBox.SelectionMode = SelectionMode.Single;
            this.elementBox.SelectedRowsChanged += (sender, args) => { };
            this.ShowAll();
            this.ChoiceProperty = new PropertyDefinition
            {
                Name = "$$choice",
                ClrType = typeof(int)
            };
            this.ControlState.CreateProperty(ChoiceProperty);
        }

        protected override Widget RenderField(PropertyDefinition property)
        {
            Label label = new Label();

            label.Text = property.Name;
            
            label.MarginTop = 10;
            label.Halign = Align.Start;
            
            return label;
        }

        protected override void RenderControls()
        {
        }

        protected override void SaveControlState()
        {
            this.ControlState[ChoiceProperty] = this.elementBox.SelectedRow.Index;
        }

        public override void UpdateData()
        {
            this.source.ReplaceWith(ControlState);
        }
    }
}