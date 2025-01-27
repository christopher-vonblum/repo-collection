using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CoreUi.Proxy;
using CoreUi.Razor.Event.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreUi.Razor.Event.Source
{
    public class EventSource : IEventSource
    {
        private Action<EventModel> onEnqueue;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ConcurrentQueue<EventModel> events = new ConcurrentQueue<EventModel>();

        public EventSource(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            WrapAndEnqueue(new ReloadClientEvent());
        }
        
        public void OnClientEvent(Guid clientId, Action<object> onEnqueue)
        {
            this.onEnqueue += (e) =>
            {
                if (e.SourceClient == clientId || (e.ReceivedBy.ContainsKey(clientId) && e.ReceivedBy[clientId]))
                {
                    return;
                }
                
                e.ReceivedBy[clientId] = true;
                onEnqueue(e);
            };
        }

        public void WrapAndEnqueue<T>(T eventModel)
        {
            // create wrapper
            var eventDefinition = new EventModel();

            if (eventModel is IFilterForSourceClient)
            {
                eventDefinition.SourceClient = Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers["clientId"]);
            }
            
            eventDefinition.Data = JToken.Parse(JsonConvert.SerializeObject(eventModel, typeof(T),
                new JsonSerializerSettings()
                    {Converters = new List<JsonConverter> {new ProxyNewtonsoftJsonSerializationConverter()}}));
            eventDefinition.Type = typeof(T).AssemblyQualifiedName;

            eventDefinition.ReceivedBy = new ConcurrentDictionary<Guid, bool>();
            
            events.Enqueue(eventDefinition);

            onEnqueue?.Invoke(eventDefinition);
        }
    }
}