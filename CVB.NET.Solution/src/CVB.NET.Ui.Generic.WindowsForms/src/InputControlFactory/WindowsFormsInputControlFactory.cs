namespace CVB.NET.Ui.Generic.WindowsForms.InputControlFactory
{
    using System.Windows.Forms;
    using Base;

    public class WindowsFormsInputControlFactory : IInputControlFactory<Control>
    {
        public Control CreateTextInputControl(string description, string initialValue = null)
        {
            throw new System.NotImplementedException();
        }

        public Control CreateChooseInputControl(string description, object[] possibleValues, object initialValue = null)
        {
            throw new System.NotImplementedException();
        }

        public Control CreateBoolInputControl(string description, bool initialValue = false)
        {
            throw new System.NotImplementedException();
        }

        public Control CreateListInputControl(string description, object[] initialValues = null)
        {
            throw new System.NotImplementedException();
        }
    }
}