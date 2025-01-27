using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoreUi.Razor.Event.Base;

namespace CoreUi.Razor.Dialog
{
    public interface IDialogManager
    {
        IEnumerable<Guid> GetActiveDialogIds();
        void Destroy(Guid dialogId);
        void CreateDialog(TypedResultInteractionEvent eventDefinition, Expression<AsyncContinuationDelegate> continuationExpression);
        void SyncDialogViewState(DialogViewState dialogViewState);
        DialogEvent GetDialogSourceEvent(Guid dialogid);
        DialogViewState GetDialogViewState(Guid dialogId);
        DialogControlState GetDialogControlState(Guid dialogId);
    }
}