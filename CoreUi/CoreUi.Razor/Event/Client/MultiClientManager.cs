using System;
using System.Collections.Concurrent;

namespace CoreUi.Razor.Event.Client
{
    public class MultiClientManager : IMultiClientManager
    {
        private ConcurrentDictionary<Guid, string> knownClients = new ConcurrentDictionary<Guid, string>();
        
        public void OnClientConnected(string user, Guid clientId)
        {
            knownClients.GetOrAdd(clientId, s => user);
        }

        public bool AreClientsOwnedBySameUser(Guid clientId, Guid clientId2)
        {
            if (knownClients.TryGetValue(clientId, out var user1) && knownClients.TryGetValue(clientId2, out var user2))
            {
                return user1 == user2;
            }

            return false;
        }
    }
}