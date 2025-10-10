using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UdpBroadcastServer
{
    static void Main()
    {
        int port = 45678;
        using Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        server.EnableBroadcast = true;

        Console.WriteLine($"UDP Broadcast Server started on port {port}.");
        Console.WriteLine("Type a message and press Enter to broadcast. Type 'exit' to quit.");

        while (true)
        {
            string msg = Console.ReadLine() ?? "";
            if (msg.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            byte[] data = Encoding.UTF8.GetBytes(msg);
            IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Broadcast, port);
            server.SendTo(data, broadcastEP);

            Console.WriteLine($"[Server] Broadcasted: {msg}");
        }
    }
}
