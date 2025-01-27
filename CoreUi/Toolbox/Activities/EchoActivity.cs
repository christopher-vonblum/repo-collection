using CoreUi;

namespace Toolbox.Activities
{
    public class EchoActivity : ActivityBase<string, string>
    {
        private readonly IInteractionProvider _interactionProvider;

        public EchoActivity(IInteractionProvider interactionProvider)
        {
            _interactionProvider = interactionProvider;
        }


        public override string Execute(string input)
        {
            _interactionProvider.Messsage(input);
            return input;
        }
    }
}