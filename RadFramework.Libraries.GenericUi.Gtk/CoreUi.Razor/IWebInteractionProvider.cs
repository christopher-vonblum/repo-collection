using System;
using System.Linq.Expressions;
using CoreUi.Razor.Dialog;

namespace CoreUi.Razor
{
    public interface IWebInteractionProvider : IInteractionProvider
    {
        object AwaitOpenRequest(Guid responseToken);
        void InvokeRequestInput(Type tInput, object template, Expression<AsyncContinuationDelegate> continueWith);
        void InvokeRequestDecision(Type tInput, object template, Expression<AsyncContinuationDelegate> continueWith);
        void RespondTo(Guid responseToken, object o);
        void CancelResponse(Guid responseToken);
        
    }
}