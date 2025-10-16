using System.Net;
using System.Net.Sockets;
using System.Text;

var port = 45678;
var ip = IPAddress.Any;
var localEP = new IPEndPoint(ip, port);

var server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
server.Bind(localEP);

Console.WriteLine($"UDP Server hazir: {localEP}");
Console.WriteLine("Mesaj gцzl?nilir...");

var buffer = new byte[1024];

while (true)
{
    EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
    int len = server.ReceiveFrom(buffer, ref remoteEP);

    var msg = Encoding.UTF8.GetString(buffer, 0, len);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"[{remoteEP}] ? {msg}");
    Console.ResetColor();
}