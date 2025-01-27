using System;
using System.Linq;
using System.Windows.Forms;

namespace CVB.NET.Ui.AutoUi.WindowsForms
{
    public partial class EditableList : UserControl
    {
        public event Action<object[]> ValuesChanged;

        public object[] Items
        {
            get { return ListBox.Items.Cast<object>().ToArray(); }
            set
            {
                ListBox.Items.Clear();
                ListBox.Items.AddRange(value);
                ValuesChanged?.Invoke(value);
            }
        }

        public EditableList()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            ListBox.Items.Add(AddTextBox.Text);

            AddTextBox.Clear();

            ValuesChanged?.Invoke(ListBox.Items.Cast<object>().ToArray());
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (ListBox.SelectedIndex == -1) return;
            Removing = true;
            ListBox.Items.RemoveAt(ListBox.SelectedIndex);
            ValuesChanged?.Invoke(ListBox.Items.Cast<object>().ToArray());
            Removing = false;
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBox.SelectedIndex == -1 || Removing) return;
            Selecting = true;
            ChangeIndexBox.Value = ListBox.SelectedIndex;
            Selecting = false;
        }

        private bool Removing { get; set; }

        private void ChangeIndexBox_ValueChanged(object sender, EventArgs e)
        {
            if (ListBox.SelectedIndex == -1 || Selecting)
            {
                ChangeIndexBox.Value = 0;
                return;
            }

            if (ChangeIndexBox.Value >= ListBox.Items.Count)
            {
                ChangeIndexBox.Value = ListBox.SelectedIndex;
                return;
            }

            string value = (string)ListBox.SelectedItem;

            ListBox.Items.RemoveAt(ListBox.SelectedIndex);

            ListBox.Items.Insert((int)ChangeIndexBox.Value, value);

            ListBox.SetSelected((int)ChangeIndexBox.Value, true);
        }

        private bool Selecting { get; set; }
    }
}
