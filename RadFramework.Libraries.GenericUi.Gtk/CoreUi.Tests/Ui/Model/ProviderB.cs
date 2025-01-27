namespace CoreUi.Tests.Ui.Model
{
    class ProviderB : IRuntimeProvider
    {
        private readonly IInteractionProvider _interactionProvider;

        public ProviderB(IInteractionProvider interactionProvider)
        {
            _interactionProvider = interactionProvider;
        }
        public void Test()
        {
            _interactionProvider.Messsage("B in use.");
        }
    }
}