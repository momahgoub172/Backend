using System.Net.WebSockets;
using System.Text;

ClientWebSocket ws = new ClientWebSocket();

//get user name
string userName;
while (true)
{
    Console.Write("Enter your name : ");
    userName = Console.ReadLine();
    break;
}



Console.WriteLine("Connecting");
await ws.ConnectAsync(new Uri($"ws://localhost:2345/ws?userName={userName}"), CancellationToken.None);
Console.WriteLine("Connected");

var buffer = new byte[1024];
while (true)
{
    var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.Count));
}

