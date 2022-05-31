using MickeySocketServer.Models;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

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

            var content = Encoding.UTF8.GetString(buffer, 0, requestSocket.Count);

            Console.WriteLine(content);

            var explorer = new Explorer(content);

            explorer.MatchFile();

            await webSocket.SendAsync(
                Encoding.ASCII.GetBytes(""),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }
});


await app.RunAsync();