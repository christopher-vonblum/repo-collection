namespace CVB.NET.Ui.WindowsForms.Controls
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class EditableList : UserControl
    {
        public object[] Items
        {
            get { return items; }
            set
            {
                items = value;

                ValueListBox.Items.Clear();
                //ValueListBox.Items.AddRange(items.Select(item => UiUtils.GetUiMaskString(item)).ToArray());
                ValuesChanged?.Invoke(value);
            }
        }

        private object[] items;

        public EditableList()
        {
            InitializeComponent();
        }

        public event Action<object[]> ValuesChanged;

        private void EditableList_Load(object sender, EventArgs e)
        {
            UpdateUiAfterSelectedIndexChange();
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUiAfterSelectedIndexChange();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            ValueListBox.SelectedIndex = ValueListBox.Items.Add(AddTextBox.Text);

            AddTextBox.Clear();

            ValuesChanged?.Invoke(ValueListBox.Items.Cast<object>().ToArray());
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            ValueListBox.Items.RemoveAt(ValueListBox.SelectedIndex);

            ValuesChanged?.Invoke(ValueListBox.Items.Cast<object>().ToArray());
        }

        private void RaiseIndexButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LowerIndexButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UpdateUiAfterSelectedIndexChange()
        {
            if (ValueListBox.SelectedIndex == -1)
            {
                RaiseIndexButton.Enabled = false;
                LowerIndexButton.Enabled = false;
                RemoveButton.Enabled = false;

                ItemIndexBox.Clear();

                return;
            }

            ItemIndexBox.Text = ValueListBox.SelectedIndex.ToString();

            RemoveButton.Enabled = true;

            RaiseIndexButton.Enabled = ValueListBox.SelectedIndex != ValueListBox.Items.Count - 1;

            LowerIndexButton.Enabled = ValueListBox.SelectedIndex != 0;
        }
    }
}