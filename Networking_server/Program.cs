using System.Net;
using System.Net.Sockets;
using System.Text;

var port = 45678;
var ip = IPAddress.Any;
var localEP = new IPEndPoint(ip, port);
var server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
server.Bind(localEP);

Console.WriteLine($"Server ready on {localEP}");

var buffer = new byte[1024];
var clients = new List<EndPoint>();
EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
var pairs = new List<(EndPoint, EndPoint)>();

while (true)
{
    int len = server.ReceiveFrom(buffer, ref remoteEP);
    var msg = Encoding.UTF8.GetString(buffer, 0, len);

    if (msg == "HELLO")
    {
        if (!clients.Contains(remoteEP))
        {
            clients.Add(remoteEP);
            Console.WriteLine($"New client joined: {remoteEP}");
            server.SendTo(Encoding.UTF8.GetBytes("Connected to server!"), remoteEP);
        }

        if (clients.Count % 2 == 0)
        {
            //2 client olmalidir
            var a = clients[^2];
            var b = clients[^1];
            pairs.Add((a, b));

            server.SendTo(Encoding.UTF8.GetBytes("You are paired with another client."), a);
            server.SendTo(Encoding.UTF8.GetBytes("You are paired with another client."), b);

            Console.WriteLine($"Paired {a} ↔ {b}");
        }

        continue;
    }

    var pair = pairs.FirstOrDefault(p => p.Item1.Equals(remoteEP) || p.Item2.Equals(remoteEP));
    if (pair != default)
    {
        var target = pair.Item1.Equals(remoteEP) ? pair.Item2 : pair.Item1;
        server.SendTo(Encoding.UTF8.GetBytes(msg), target);
        Console.WriteLine($"[{remoteEP}] → [{target}] : {msg}");
    }
    else
    {
        server.SendTo(Encoding.UTF8.GetBytes("You are not paired yet."), remoteEP);
    }
}
