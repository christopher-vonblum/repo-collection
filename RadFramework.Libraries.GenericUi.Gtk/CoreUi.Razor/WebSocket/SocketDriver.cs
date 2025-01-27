using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreUi.Razor.Event.Client;
using CoreUi.Razor.Event.Source;
using Newtonsoft.Json;

namespace CoreUi.Razor.MultiClient
{
    public class SocketDriver : ISocketDriver
    {
        private readonly IEventSource _eventSource;
        private readonly IMultiClientManager _multiClientManager;
        private readonly IAuthService _authService;

        public SocketDriver(IEventSource eventSource, IMultiClientManager multiClientManager, IAuthService authService)
        {
            _eventSource = eventSource;
            _multiClientManager = multiClientManager;
            _authService = authService;
        }
        public async void OpenSocketPipeline(Guid clientId, WebSocket webSocket, TaskCompletionSource<object> completeSocket)
        {
            if (!TryAuth(clientId, webSocket, out string user))
            {
                return;
            }
            
            /*_eventSource.OnClientEvent(clientId, o =>
            {
                Task.Run(() => { SendString(webSocket, o); });
            });*/

            while (true)
            {
                await Receive(webSocket);
            }
        }

        bool TryAuth(Guid clientId, WebSocket webSocket, out string user)
        {
            var authData = ReadString(webSocket);
            
            user = null;
            
            if (!(authData?.Contains(":") ?? false))
            {
                return false;
            }

            string pw;
            int index = authData.IndexOf(":", StringComparison.Ordinal);

            user = authData.Substring(0, index);
            pw = authData.Substring(index + 1);

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pw))
            {
                return false;
            }

            if (_authService.ValidateCredentials(user, pw))
            {
                _multiClientManager.OnClientConnected(user, clientId);
                return true;
            }
            return false;
        }

        private string ReadString(WebSocket webSocket)
        {
            return (string)Receive(webSocket).Result;
        }
        
        
        
        private async Task<object> Receive(WebSocket webSocket)
        {
            var buffer =  WebSocket.CreateClientBuffer(1024*4,1024*4);
            WebSocketReceiveResult result= null;

            using (var ms = new MemoryStream())
            {
                do
                {
                    result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string raw = Encoding.UTF8.GetString(buffer);
                    return raw;
                }
                
                throw new InvalidOperationException();
            }
        }
    }
}