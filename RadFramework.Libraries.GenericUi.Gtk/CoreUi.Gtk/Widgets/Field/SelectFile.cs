using CoreUi.Gtk.Base;
using CoreUi.Model;
using CoreUi.Proxy;
using Gtk;

namespace CoreUi.Gtk.Widgets.Field
{
    public class SelectFile : FieldBase
    {
        private FileChooserButton control;

        public SelectFile(IObject controlState, PropertyDefinition propertyDefinition) : base(controlState, propertyDefinition)
        {
            this.control = new FileChooserButton("...", FileChooserAction.Open);

            object state = ControlStateValue;
            
            if (state != null)
            {
                control.SetFilename((string) state);
            }
            this.Add(control);
        }

        public override void SaveControlState()
        {
            if (control.File == null)
            {
                return;
            }

            this.ControlStateValue = control.File.Path;
        }
    }
}