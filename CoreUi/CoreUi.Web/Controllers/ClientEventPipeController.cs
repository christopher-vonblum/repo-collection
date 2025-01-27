using System;
using System.Collections.Generic;
using CoreUi.Razor.Event.Base;
using CoreUi.Razor.Event.Source;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreUi.Web.Controllers
{
    [Authorize]
    public class ClientEventPipeController : Controller
    {
        private readonly IEventSource _eventSource;

        public ClientEventPipeController(IEventSource eventSource)
        {
            _eventSource = eventSource;
        }
        
        // replaced with websockets...
        /*[HttpPost]
        public IActionResult PollEvents()
        {
            List<EventModel> eventBlock = new List<EventModel>();

            int i = 0;
            int maxAmount = 20;

            var clientId = Guid.Parse(HttpContext.Request.Headers["clientId"]);
            
            foreach (var @event in _eventSource.Process(clientId))
            {
                eventBlock.Add(@event);
                i++;

                if (i >= maxAmount)
                {
                    break;
                }
            }

            return Json(eventBlock);
        }*/
    }
}