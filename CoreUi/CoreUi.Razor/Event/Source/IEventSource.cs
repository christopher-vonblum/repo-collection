using System;
using System.Collections.Generic;

namespace CoreUi.Razor.Event.Source
{
    public interface IEventSource
    {
        void OnClientEvent(Guid clientId, Action<object> onEnqueue);
        void WrapAndEnqueue<T>(T eventModel);
    }
}