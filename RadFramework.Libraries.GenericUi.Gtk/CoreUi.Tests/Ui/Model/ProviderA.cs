namespace CoreUi.Tests.Ui.Model
{
    class ProviderA : IRuntimeProvider
    {
        private readonly IInteractionProvider _interactionProvider;

        public ProviderA(IInteractionProvider interactionProvider)
        {
            _interactionProvider = interactionProvider;
        }
        
        public void Test()
        {
            _interactionProvider.Messsage("A in use.");
        }
    }
}