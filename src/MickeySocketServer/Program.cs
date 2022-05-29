using MickeySocketServer.Models;
using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();

byte[] buffer = new byte[256];
app.Map("/", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    else
    {
        using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

        while (true)
        {
            var requestSocket = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

            var content = Encoding.ASCII.GetString(buffer, 0, requestSocket.Count);

            var explorer = new Explorer(content);

            await webSocket.SendAsync(
                Encoding.ASCII.GetBytes(explorer.GetContentFile()),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }
});


await app.RunAsync();