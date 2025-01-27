using System;
using System.Collections.Generic;
using System.Linq;
using CoreUi.Gtk.Base;
using CoreUi.Model;
using CoreUi.Objects;
using CoreUi.Proxy;
using GLib;
using Gtk;
using Window = Gtk.Window;

namespace CoreUi.Gtk.Dialog
{
    public class CollectionInputDialog : TypeAwareDataInputDialog
    {
        private readonly Type _elementType;
        protected ListBox elementBox;
        public CollectionInputDialog(
            Type elementType,
            IObject dataObject,
            GtkInteractionProvider gtkInteractionProvider,
            Window parentDialog = null) 
            : base(dataObject, dataObject.PropertyDefinitions.OrderBy(e => e.Name), gtkInteractionProvider, parentDialog)
        {
            _elementType = elementType;
        }

        protected override Container CreateDynamicContainer()
        {
            ListBox b = new ListBox();
            VBox.Remove(dynamicBox);
            VBox.Add(b);
            
            dynamicBox = b;
            elementBox = b;
            
            return b;
        }

        protected override void RenderBoxEntries()
        {
            if (elementBox != null)
            {
                VBox.Remove(elementBox);
            }
            
            ListBox box = new ListBox();
            box.SelectionMode = SelectionMode.Multiple;
            VBox.Add(box);
            this.elementBox = box;
            base.RenderBoxEntries();
            ShowAll();
        }

        protected override void RenderControls()
        {
            Button addEntryButton = new Button();
            controlBox.Add(addEntryButton);

            addEntryButton.Label = "Add";
            addEntryButton.Clicked += AddEntry;
            
            Button moveEntryUpButton = new Button();
            controlBox.Add(moveEntryUpButton);
            
            moveEntryUpButton.Label = "Up";
            moveEntryUpButton.Clicked += MoveEntryUp;
            
            Button moveEntryDownButton = new Button();
            controlBox.Add(moveEntryDownButton);
            
            moveEntryDownButton.Label = "Down";
            moveEntryDownButton.Clicked += MoveEntryDown;
            
            base.RenderControls();

            Button removeEntryButton = new Button();
            controlBox.Add(removeEntryButton);
            
            removeEntryButton.Label = "Remove";
            removeEntryButton.Clicked += RemoveEntry;
        }

        private void MoveEntryDown(object sender, EventArgs args)
        {
            List entries = elementBox.SelectedRows;
            
            foreach (ListBoxRow entry in entries)
            {
                int index = entry.Index;

                index++;

                if (index > elementBox.Children.Length - 1)
                {
                    continue;
                }
            
                MoveEntry(entry, index);
            }
            
            NormalizeEntries();
        }

        private void MoveEntryUp(object sender, EventArgs args) 
        {
            List entries = elementBox.SelectedRows;
            
            foreach (ListBoxRow entry in entries)
            {
                int index = entry.Index;

                index--;

                if (index < 0)
                {
                    continue;
                }

                MoveEntry(entry, index);
            }
            
            NormalizeEntries();
        }

        private void MoveEntry(ListBoxRow entry, int index)
        {
            int oldIndex = entry.Index;
            
            elementBox.Remove(entry);
            
            this.elementBox.Insert(entry, index);

            this.elementBox.SelectRow(entry);
        }

        private void RemoveEntry(object sender, EventArgs args)
        {
            elementBox.SelectedForeach((listBox, row) => { RemoveRow(row); });
            NormalizeEntries();
        }

        private void RemoveRow(ListBoxRow row)
        {
            string key = ((Container)row.Children[0]).Children[1].Name;
            this.Fields.Remove(key);
            elementBox.Remove(row);
        }

        private void AddEntry(object sender, EventArgs args)
        {
            if (InteractionProvider.RequestInputInternal(_elementType, null, out object s, this))
            {
                PropertyDefinition field = new PropertyDefinition {Name = Guid.NewGuid().ToString("N"), ClrType = _elementType};
                
                this.Fields[field.Name] = field;

                this.ControlState.CreateProperty(field);
                
                this.ControlState[field] = s;
                
                this.elementBox.Add( RenderField(field));

                this.elementBox.ShowAll();
            }
            NormalizeEntries();
        }

        public override void UpdateData()
        {
            int i = 0;
            source.ReplaceWith(new O());
            foreach (string fieldsKey in this.Fields.Keys.OrderBy(k => GetRow(k).Index))
            {
                PropertyDefinition prop = this.source.CreateProperty(new PropertyDefinition
                {
                    Name = i.ToString(),
                    ClrType = _elementType
                });
                
                this.source[prop] = this.ControlState[this.ControlState[fieldsKey]];
                i++;
            }
        }

        ListBoxRow GetRow(string fieldKey)
        {
            return (ListBoxRow)this.elementBox.Children.FirstOrDefault(c =>
                ((FieldBase) ((ListBoxRow) c).Children[0]).Name == fieldKey);
        }
        
        private void NormalizeEntries()
        {
            foreach (ListBoxRow e in elementBox.Children)
            {
                if (!(e.Children.FirstOrDefault() is FieldBase b))
                {
                    return;
                }
                
                b.Label.Text = e.Index.ToString();
            }
        }
    }
}