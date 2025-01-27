namespace CVB.NET.Ui.Generic.Base
{
    public interface IInputControlFactory<TInputControl>
    {
        TInputControl CreateTextInputControl(string description, string initialValue = null);
        TInputControl CreateChooseInputControl(string description, object[] possibleValues, object initialValue = null);
        TInputControl CreateBoolInputControl(string description, bool initialValue = false);
        TInputControl CreateListInputControl(string description, object[] initialValues = null);
    }
}