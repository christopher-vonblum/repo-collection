using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using CoreUi.Model;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using CoreUi.Razor.Dialog;
using CoreUi.Razor.Event;
using CoreUi.Razor.Event.Source;

namespace CoreUi.Razor
{
    public class WebInteractionProvider : IWebInteractionProvider
    {
        private ConcurrentDictionary<Guid, (ManualResetEvent releaseResult, object result)> responsePool = new ConcurrentDictionary<Guid, (ManualResetEvent releaseResult, object result)>();
        private readonly IEventSource _eventSource;
        private readonly IDialogManager _dialogManager;

        public WebInteractionProvider(IEventSource eventSource, IDialogManager dialogManager)
        {
            _eventSource = eventSource;
            _dialogManager = dialogManager;
        }
        
        public T CreateInputModel<T>()
        {
            return ProxyFactory.CreateInputModel<T>();
        }

        public object CreateInputModel(Type t)
        {
            return ProxyFactory.CreateProxyOrValue(t);
        }

        public bool RequestInput(Type tInput, object template, out object output)
        {
            DialogRequestedEvent e = OpenDialog(tInput, null);

            // wait for the client to post the input model
            output = AwaitOpenRequest(e.ResponseToken);

            return true;
        }

        private DialogRequestedEvent OpenDialog(Type tInput, Expression<AsyncContinuationDelegate> continueWith)
        {
            var e = new DialogRequestedEvent();

            e.ResponseToken = Guid.NewGuid();
            e.ModelType = tInput.AssemblyQualifiedName;
            e.DialogTitle = tInput.FullName;
            
            if (ProxyFactory.IsSimpleField(tInput))
            {
                e.Properties = new[]
                {
                    new PropertyDefinition
                    {
                        Name = "Element",
                        ClrType = tInput
                    }
                };
                
                CreateDialog(e, continueWith);
                return e;
            }
            
            IObjectProxy p = (IObjectProxy) ProxyFactory.CreateProxyOrValue(tInput);

            if (p != null)
            {
                e.Properties = p.Object.PropertyDefinitions;
            }

            CreateDialog(e, continueWith);
            return e;
        }

        private void CreateDialog(DialogRequestedEvent e, Expression<AsyncContinuationDelegate> continueWith)
        {
            responsePool[e.ResponseToken] = (new ManualResetEvent(false), null);
            
            // register dialog
            _dialogManager.CreateDialog(e, continueWith);
            
            /*Thread waiter = new Thread(o =>
            {
                var res = AwaitOpenRequest(e.ResponseToken);
                continueWith(res);
            });*/
            
            //waiter.Start();
            
            // flush info about the event to the client
            _eventSource.WrapAndEnqueue(e);
        }

        public bool RequestInput<TInput>(TInput template, out TInput value)
        {
            var ret = RequestInput(typeof(TInput), template,out var val);
            value = (TInput) val;
            return ret;
        }

        public bool RequestInput<TInput>(out TInput value)
        {
            var res = RequestInput(typeof(TInput), default, out var v);

            value = (TInput) v;
            
            return res;
        }

        public bool RequestInput<TInput>(TInput template)
        {
            return RequestInput(template, out var value);
        }

        public TElement RequestDecision<TSource, TElement>(TSource source, Func<TElement, string> getDisplayString) where TSource : IEnumerable<TElement>
        {

            /*Thread waiter = new Thread(o =>
            {
                var res = AwaitOpenRequest(e.ResponseToken);
                continueWith(res);
            });
            
            waiter.Start();*/
            throw new NotImplementedException();
        }

        public void Messsage(string message)
        {
            var messageEvent = new ShowMessageDialogEvent
            {
                ResponseToken = Guid.NewGuid(),
                DialogTitle = "Message", Message = message
            };
            
            _dialogManager.CreateDialog(messageEvent, 
                (provider, data) => 
                    ((IDialogManager)provider.GetService(typeof(IDialogManager))).Destroy(messageEvent.ResponseToken));
            
            _eventSource.WrapAndEnqueue(messageEvent);
        }

        public void InvokeRequestInput(Type tInput, object template, Expression<AsyncContinuationDelegate> continueWith)
        {
            OpenDialog(tInput, continueWith);
        }

        public void InvokeRequestDecision(Type tInput, object template, Expression<AsyncContinuationDelegate> continueWith)
        {
            throw new NotImplementedException();
        }

        public void InvokeRequestDecision(Type tInput, object template, Func<object, string> getDisplayString, Expression<AsyncContinuationDelegate> continueWith)
        {
            getDisplayString = getDisplayString ?? (element => element.ToString()); 
            int i = 0;
            
            Dictionary<int, object> choices = new Dictionary<int, object>();

            List<PropertyDefinition> properties = new List<PropertyDefinition>();
            
            foreach (object element in (IEnumerable)template)
            {
                properties.Add(new PropertyDefinition
                {
                    Name = getDisplayString(element),
                    ClrType = typeof(bool),
                });

                choices[i] = element;
                i++;
            }

            var responseToken = Guid.NewGuid();
            
            var e = new DecisionRequestedEvent
            {
                DialogTitle = tInput.FullName,
                Properties = properties,
                ResponseToken = responseToken
            };
                
            _eventSource.WrapAndEnqueue(e);
        }

        public object AwaitOpenRequest(Guid responseToken)
        {
            responsePool[responseToken] = (new ManualResetEvent(false), null);

            responsePool[responseToken].releaseResult.WaitOne();

            responsePool.TryRemove(responseToken, out var res);
            
            return res.result;
        }

        public void RespondTo(Guid responseToken, object o)
        {
            var valueTuple = responsePool[responseToken];
            valueTuple.result = o;
            responsePool[responseToken] = valueTuple;
            valueTuple.releaseResult.Set();
        }

        public void CancelResponse(Guid responseToken)
        {
            responsePool.TryRemove(responseToken, out var valueTuple);
            valueTuple.releaseResult?.Close();
        }
    }
}