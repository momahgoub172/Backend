using System.Net.WebSockets;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:2345");


var app = builder.Build();



// Configure the HTTP request pipeline.
app.UseWebSockets();


var connections = new List<WebSocket>();
app.Map("ws", async context =>{
    if (context.WebSockets.IsWebSocketRequest)
    {
        //get username
        var userName = context.Request.Query["userName"];


        using var websocket = await context.WebSockets.AcceptWebSocketAsync();
        connections.Add(websocket);
        await Broadcast("Hello From Server");
        await Broadcast($"{userName} joined");
        while (true)
        {
            await Broadcast($"Time now => {DateTime.Now.ToString()}");
            Thread.Sleep(20000);
        }
    }
    else
    {
        //bad request
        context.Response.StatusCode = 400;
    }

});



async Task Broadcast(string message)
{
    var bytes = Encoding.UTF8.GetBytes(message);
    foreach (var websocket in connections)
    {
        if (websocket.State == WebSocketState.Open)
        {
            var arrayToBeSent = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await websocket.SendAsync(arrayToBeSent, WebSocketMessageType.Text, true, CancellationToken.None);

        }
    }
}





app.Run();
