using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace CoreUi.Razor.MultiClient
{
    public interface ISocketDriver
    {
        void OpenSocketPipeline(Guid clientId, WebSocket webSocket, TaskCompletionSource<object> completeSocket);
    }
}