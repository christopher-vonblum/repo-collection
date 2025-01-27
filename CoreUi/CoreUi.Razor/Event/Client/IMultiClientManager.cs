using System;
using System.Collections.Generic;

namespace CoreUi.Razor.Event.Client
{
    public interface IMultiClientManager
    {
        void OnClientConnected(string user, Guid clientId);
        bool AreClientsOwnedBySameUser(Guid clientId, Guid clientId2);
    }
}