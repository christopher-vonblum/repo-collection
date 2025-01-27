using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CoreUi.Razor.Data;
using CoreUi.Razor.Event;
using CoreUi.Razor.Event.Base;
using CoreUi.Razor.Event.Source;
using CoreUi.Razor.MultiClient;

namespace CoreUi.Razor.Dialog
{
    public class DialogManager : IDialogManager
    {
        private readonly IEventSource _eventSource;
        private readonly IDataProvider _dataProvider;
        private readonly IAuthService _authService;

        public DialogManager(IEventSource eventSource, IDataProvider dataProvider, IAuthService authService)
        {
            _eventSource = eventSource;
            _dataProvider = dataProvider;
            _authService = authService;
        }

        private string GetDialogBasePath()
        {
            return $"/users/{_authService.CurrentUser}/dialogs/";
        }
        
        private string GetDialogPath(Guid dialogId)
        {
            return GetDialogBasePath() + dialogId.ToString("N") + "/";
        }

        private string GetDialogViewStatePath(Guid dialogId)
        {
            return GetDialogPath(dialogId) + "viewstate";
        }
        
        private string GetDialogControlStatePath(Guid dialogId)
        {
            return GetDialogPath(dialogId) + "controlstate";
        }
        
        private DialogEvent GetDialogEvent(Guid guid)
        {
            return _dataProvider.Load<DialogEvent>(GetDialogPath(guid));
        }
        
        public IEnumerable<Guid> GetActiveDialogIds()
        {
            return _dataProvider.QueryVirtualChildKeys(GetDialogBasePath()).Select(c => Guid.Parse(c)).ToList();
        }

        public Type GetDialogModelType(Guid dialogId)
        {
            return Type.GetType(GetDialogEvent(dialogId).SourceEvent.ModelType) ?? throw new TypeLoadException();
        }

        public void Destroy(Guid responseToken)
        {
            string dialogPath = GetDialogPath(responseToken);
            
            if (_dataProvider.HasData(dialogPath))
            {
                _eventSource.WrapAndEnqueue(new DialogDestroyedEvent { ResponseToken = responseToken });
            }
        }

        public void CreateDialog(TypedResultInteractionEvent eventDefinition, Expression<AsyncContinuationDelegate> continuationExpression)
        {
            _dataProvider.Save(GetDialogPath(eventDefinition.ResponseToken), new DialogEvent
            {
                SourceEvent = eventDefinition,
                ContinuationExpression = continuationExpression
            });
            
            _eventSource.WrapAndEnqueue(eventDefinition);
        }

        public void SyncDialogViewState(DialogViewState dialogViewState)
        {
            var dialogPath = GetDialogViewStatePath(dialogViewState.DialogId);

            if (!_dataProvider.HasData(GetDialogPath(dialogViewState.DialogId)))
            {
                return;
            }
            
            _dataProvider.Save(dialogPath, dialogViewState);
            
            _eventSource.WrapAndEnqueue(new SyncDialogViewEvent
            {
                Dialog = dialogViewState
            });
        }

        public void SyncDialogControlValue(DialogControlValue controlValue)
        {
            var valuePath = GetDialogPath(controlValue.DialogId) + controlValue.Name;
            
            _dataProvider.Save(valuePath, controlValue);
            
            _eventSource.WrapAndEnqueue(controlValue);
        }
        
        public DialogEvent GetDialogSourceEvent(Guid dialogid)
        {
            var dialogpath = GetDialogPath(dialogid);
            DialogEvent viewState = null;
            if (_dataProvider.HasData(dialogpath))
            {
                viewState = _dataProvider.Load<DialogEvent>(dialogpath);
            }

            return viewState;
        }

        public DialogViewState GetDialogViewState(Guid dialogId)
        {
            var viewStatePath = GetDialogViewStatePath(dialogId);
            DialogViewState viewState = null;
            if (_dataProvider.HasData(viewStatePath))
            {
                viewState = _dataProvider.Load<DialogViewState>(viewStatePath);
            }

            return viewState;
        }

        public DialogControlState GetDialogControlState(Guid dialogId)
        {
            var controlStatePath = GetDialogControlStatePath(dialogId);
            DialogControlState controlState = null;
            if (_dataProvider.HasData(controlStatePath))
            {
                controlState = _dataProvider.Load<DialogControlState>(controlStatePath);
            }

            return controlState;
        }
    }
}