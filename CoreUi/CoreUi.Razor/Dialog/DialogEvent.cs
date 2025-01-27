using System.Linq.Expressions;
using CoreUi.Razor.Event.Base;

namespace CoreUi.Razor.Dialog
{
    public class DialogEvent
    {
        public TypedResultInteractionEvent SourceEvent { get; set; }
        public Expression<AsyncContinuationDelegate> ContinuationExpression { get; set; }
    }
}
