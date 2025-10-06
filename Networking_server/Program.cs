using System.Net;
using System.Net.Sockets;
using System.Text;
// run server first,then copy client into 2
var port = 45678;
var server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
var localEP = new IPEndPoint(IPAddress.Any, port);
server.Bind(localEP);

Console.WriteLine($"[SERVER] UDP server started on {localEP}...");
Console.WriteLine("Waiting for clients...");

EndPoint? client1 = null;
EndPoint? client2 = null;
var buffer = new byte[1024];

while (true)
{
    EndPoint senderEP = new IPEndPoint(IPAddress.Any, 0);
    int len = server.ReceiveFrom(buffer, ref senderEP);
    var msg = Encoding.UTF8.GetString(buffer, 0, len);

    if (client1 == null)
    {
        client1 = senderEP;
        Console.WriteLine($"Client 1 connected: {client1}");
        server.SendTo(Encoding.UTF8.GetBytes("Connected as Client 1"), client1);
        continue;
    }
    else if (client2 == null && !senderEP.Equals(client1))
    {
        client2 = senderEP;
        Console.WriteLine($"Client 2 connected: {client2}");
        server.SendTo(Encoding.UTF8.GetBytes("Connected as Client 2"), client2);
        server.SendTo(Encoding.UTF8.GetBytes("You are now connected to a chat partner."), client1);
        server.SendTo(Encoding.UTF8.GetBytes("You are now connected to a chat partner."), client2);
        continue;
    }
    if (senderEP.Equals(client1) && client2 != null)
        server.SendTo(Encoding.UTF8.GetBytes($"Client1: {msg}"), client2);
    else if (senderEP.Equals(client2) && client1 != null)
        server.SendTo(Encoding.UTF8.GetBytes($"Client2: {msg}"), client1);

    Console.WriteLine($"[{senderEP}] → {msg}");
}
