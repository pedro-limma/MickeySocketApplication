using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;

using var ws = new ClientWebSocket();
try
{
    await ws.ConnectAsync(new Uri("ws://localhost:5130"), CancellationToken.None);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

byte[] buffer = new byte[256];

while (ws.State == WebSocketState.Open)
{
    Console.WriteLine("Insira o nome do arquivo: [algumaCoisa.txt]");
    var file = Console.ReadLine();

    Console.WriteLine("Insira o tamanho do arquivo em bytes: ");
    var qtnBytes = Console.ReadLine();
    
    Console.WriteLine(@"Insira o diretório base: [C:\]");
    var baseDir = Console.ReadLine();

    dynamic obj = new
    {
        Filename = file,
        QtnBytes = qtnBytes,
        BaseDir = baseDir
    };

    Console.WriteLine(JsonSerializer.Serialize(obj));

    await ws.SendAsync(
        Encoding.ASCII.GetBytes(JsonSerializer.Serialize(obj)),
        WebSocketMessageType.Text,
        true,
        CancellationToken.None);

    var result = await ws.ReceiveAsync(buffer, CancellationToken.None);

    if (result.MessageType == WebSocketMessageType.Close)
        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
    else
        Console.WriteLine("[ARQUIVO ENCONTRADO]");
        Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, result.Count));
}

Console.ReadLine();
