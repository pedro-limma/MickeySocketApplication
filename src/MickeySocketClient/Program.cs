using System.Net.WebSockets;
using System.Diagnostics;
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

var timer = new Stopwatch();

while (ws.State == WebSocketState.Open)
{
    Console.WriteLine("Insira o nome do arquivo: [algumaCoisa.txt]");
    var file = Console.ReadLine();

    Console.WriteLine("Insira o tamanho do arquivo em bytes: ");
    var qtnBytes = Console.ReadLine();
    
    Console.WriteLine(@"Insira o diretório base: [C:\]");
    var baseDir = Console.ReadLine();


    await ws.SendAsync(
        Encoding.UTF8.GetBytes($"{file},{qtnBytes},{baseDir}"),
        WebSocketMessageType.Text,
        true,
        CancellationToken.None);
    timer.Start();
    var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
    timer.Stop();

    if (result.MessageType == WebSocketMessageType.Close)
        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
    else
    {
        Console.WriteLine($"[ARQUIVO ENCONTRADO EM ({Encoding.ASCII.GetString(buffer, 0, result.Count)})]");
        Console.WriteLine("Elapsed time: {0:hh\\:mm\\:ss\\.fff}", timer.Elapsed);
    }
}

Console.ReadLine();
