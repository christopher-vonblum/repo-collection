using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CoreUi.Razor.MultiClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CoreUi.Web
{
    public class WsMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                         
                    ISocketDriver socketDriver = context.RequestServices.GetService<ISocketDriver>();
                         
                    TaskCompletionSource<object> close = new TaskCompletionSource<object>();
     
                    var clientId = Guid.Parse(context.Request.Headers["Sec-WebSocket-Protocol"]);

                    bool requiresAuth = true;
                    int tries = 0;
                    while (requiresAuth)
                    {
                        requiresAuth = !TryAuth(context.RequestServices.GetService<IAuthService>(), webSocket, out string user);
                        tries++;
                        if (tries > 2)
                        {
                            return;
                        }
                    }
                    
                    while (!webSocket.CloseStatus.HasValue)
                    {
                        byte[] res = await ReceivePackage(webSocket);
                    }
                    
                    //socketDriver.OpenSocketPipeline(clientId, webSocket, close);
                         
                    return;
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await next(context);
            }
        }
        private static void SendString(WebSocket webSocket, string o)
        {
            var bytes = Encoding.UTF8.GetBytes(o);
            var arraySegment = new ArraySegment<byte>(bytes);
            webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private string ReceiveString(WebSocket webSocket)
        {
            var msg = ReceivePackage(webSocket);
            return Encoding.UTF8.GetString(msg.Result);
        }
        
        bool TryAuth(IAuthService authService, WebSocket webSocket, out string user)
        {
            var authData = ReceiveString(webSocket);
            
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

            if (authService.ValidateCredentials(user, pw))
            {
                //_multiClientManager.OnClientConnected(user, clientId);
                return true;
            }
            return false;
        }
        
        private static  async Task<byte[]> ReceivePackage(WebSocket webSocket)
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
                
                return ms.ToArray();
            }
        }
    }
}