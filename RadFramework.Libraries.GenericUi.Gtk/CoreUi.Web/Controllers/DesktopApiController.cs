using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using CoreUi.Razor;
using CoreUi.Razor.Dialog;
using CoreUi.Razor.Event;
using CoreUi.Razor.Event.Source;
using CoreUi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Toolbox;

namespace CoreUi.Web.Controllers
{
    [Authorize]
    public class DesktopApiController : ServiceProxyControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ToolBox _toolbox;
        private readonly IWebInteractionProvider _webInteractionProvider;
        private readonly IDialogManager _dialogManager;
        private readonly IEventSource _eventSource;

        public DesktopApiController(IServiceProvider serviceProvider, ToolBox toolbox, IWebInteractionProvider webInteractionProvider, IDialogManager dialogManager, IEventSource eventSource)
        {
            _serviceProvider = serviceProvider;
            _toolbox = toolbox;
            _webInteractionProvider = webInteractionProvider;
            _dialogManager = dialogManager;
            _eventSource = eventSource;
        }
        
        [HttpPost]
        public IActionResult ActivityOverview()
        {
            return Json(
                _toolbox
                    .Activities
                    .Select(a => new ActivityModel
                    {
                         Type = a.AssemblyQualifiedName,
                         Description = a.FullName
                    }));
        }
        
        [HttpPost]
        public IActionResult GetMinimumBodySize()
        {
            var activeIds = _dialogManager.GetActiveDialogIds().ToList();

            if (!activeIds.Any())
            {
                return Json(new BodySizeModel { Width = 0, Height = 0 });
            }
            
            double maxWidth = activeIds
                .Max(id =>
                {
                    DialogViewState viewState = _dialogManager.GetDialogViewState(id);
                    double maxX = viewState.Width + viewState.X;
                    return maxX;
                });

            double maxHeight = activeIds
                .Max(id =>
                {
                    DialogViewState viewState = _dialogManager.GetDialogViewState(id);
                    double maxY = viewState.Height + viewState.Y;
                    return maxY;
                });
            
            return Json(new BodySizeModel { Width  = maxWidth, Height = maxHeight });
        }

        [HttpPost]
        public IActionResult GetOpenDialogs()
        {
            return Json(
                _dialogManager
                    .GetActiveDialogIds()
                    .Select(dialogId =>
                    {
                        var model = new DialogModel
                        {
                            DialogId = dialogId,
                            Properties = _dialogManager.GetDialogSourceEvent(dialogId).SourceEvent.Properties,
                            ViewState = _dialogManager.GetDialogViewState(dialogId),
                            ControlState = _dialogManager.GetDialogControlState(dialogId)
                        };
                        
                        return model;
                    }));
        }

        [HttpPost]
        public IActionResult FocusSelector([FromBody]string selector)
        {
            _eventSource.WrapAndEnqueue(new FocusSelectorEvent {Selector = selector});
            return Json(Ok());
        }
        
        [HttpPost]
        public IActionResult GetPrimitiveTypes()
        {
            return Json(new[]{ typeof(string), typeof(int), typeof(bool), typeof(Guid)}.Select(t => t.AssemblyQualifiedName));
        }
        
        [HttpPost]
        public IActionResult InvokeActivityInput([FromBody]TypeModel activityType)
        {
            Type activity = Type.GetType(activityType.Type);

            Type inputModel = activity.GetInterface("IActivity`2").GetGenericArguments()[0];

            _webInteractionProvider.InvokeRequestInput(inputModel, null, 
                (provider, data) => ((ToolBox)provider.GetService(typeof(ToolBox))).Run(activity, data));
            
            return Json(Ok());
        }

        [HttpPost]
        public IActionResult SyncDialog([FromBody]DialogViewState dialogViewState)
        {
            _dialogManager.SyncDialogViewState(dialogViewState);
            return Json(Ok());
        }

        [HttpPost]
        public IActionResult SyncDialogControlState([FromBody]DialogControlState dialogModel)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public IActionResult ConfirmResponse([FromBody]DialogResultModel result)
        {
            // distribute the destroy event to all js clients
            _dialogManager.Destroy(result.ResponseToken);
            
            // get source event from db
            DialogEvent @event = _dialogManager.GetDialogSourceEvent(result.ResponseToken);
            
            // convert the result model of the dialog to a strongly typed model for the activity
            object model = null;

            if (result.Data != null)
            {
                model = GetTypedObject(Type.GetType(@event.SourceEvent.ModelType), result);
            }
            
            // compile continuation expression of the source event
            AsyncContinuationDelegate lambda = @event.ContinuationExpression.Compile();

            try
            {
                // now invoke the lambda with the current service provider.
                // this calls into the actual buisiness logic.
                lambda(_serviceProvider, model);
            }
            catch (Exception e)
            {
                _webInteractionProvider.Messsage(e.ToString());
            }
            
            return Json(Ok());
        }

        private object GetTypedObject(Type modelType, DialogResultModel model)
        {
            if (ProxyFactory.IsSimpleField(modelType))
            {
                return model.Data["Element"].ToObject(modelType);
            }
            
            return model.Data.ToObject(modelType,
                JsonSerializer.CreateDefault(new JsonSerializerSettings
                    {Converters = new List<JsonConverter> {new ProxyNewtonsoftJsonSerializationConverter()}}));
        }
        
        [HttpPost]
        public IActionResult CancelResponse([FromBody]Guid responseToken)
        {
            _dialogManager.Destroy(responseToken);
            _webInteractionProvider.CancelResponse(responseToken);
            return Json(Ok());
        }
    }
}