using System.Net.WebSockets;
using System.Threading;

using var ws = new ClientWebSocket();

await ws.ConnectAsync(new Uri("ws://localhost:5130"), CancellationToken.None);

byte[] buffer = new byte[256];

while (ws.State == WebSocketState.Open)
{



}

