using Gtk;

namespace CoreUi.Gtk.Base
{
    public class DialogBase : global::Gtk.Dialog
    {
        public DialogBase(string title = "", Window parent = null) 
            : base(title,
            parent,
            DialogFlags.Modal,
            new object[0])
        {
            this.HeightRequest = 600;
            this.WidthRequest = 800;
        }
    }
}