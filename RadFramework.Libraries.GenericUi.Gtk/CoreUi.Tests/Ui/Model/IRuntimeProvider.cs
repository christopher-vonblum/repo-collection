using CoreUi.Attributes;

namespace CoreUi.Tests.Ui.Model
{
    public interface IRuntimeProvider
    {
        [UiAction]
        void Test();
    }
}