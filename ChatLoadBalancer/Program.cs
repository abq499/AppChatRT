using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ChatLoadBalancer
{
    class Program
    {
        // Danh sach cac Chat Server trong cum
        static readonly List<ChatServerNode> ChatServers = new List<ChatServerNode>
        {
            new ChatServerNode("127.0.0.1", 8888),
            new ChatServerNode("127.0.0.1", 8889)
        };

        // Bien dem Round-Robin
        static int nextServerIndex = 0;

        // Lock tranh nhieu client cung luc lam sai index
        static readonly object roundRobinLock = new object();

        static async Task Main(string[] args)
        {
            int lbPort = 8800;

            TcpListener listener = new TcpListener(IPAddress.Any, lbPort);
            listener.Start();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[LOAD BALANCER] Dang chay tai Port {lbPort}...");
            Console.WriteLine("[LOAD BALANCER] Danh sach server:");
            foreach (var server in ChatServers)
            {
                Console.WriteLine($"- {server.Ip}:{server.Port}");
            }
            Console.ResetColor();

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = HandleClientRoutingAsync(client);
            }
        }

        static async Task HandleClientRoutingAsync(TcpClient client)
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
            {
                try
                {
                    ChatServerNode? assignedServer = await GetAvailableServerAsync();

                    if (assignedServer == null)
                    {
                        await writer.WriteLineAsync("ERROR|No available chat server");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[LOI] Khong co Chat Server nao dang hoat dong.");
                        Console.ResetColor();
                        return;
                    }

                    string redirectMsg = $"REDIRECT|{assignedServer.Ip}|{assignedServer.Port}";
                    await writer.WriteLineAsync(redirectMsg);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[DIEU PHOI] Client moi -> {assignedServer.Ip}:{assignedServer.Port}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[LOI ROUTING] {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        static async Task<ChatServerNode?> GetAvailableServerAsync()
        {
            int totalServers = ChatServers.Count;

            for (int i = 0; i < totalServers; i++)
            {
                ChatServerNode candidate;

                lock (roundRobinLock)
                {
                    candidate = ChatServers[nextServerIndex];
                    nextServerIndex = (nextServerIndex + 1) % totalServers;
                }

                bool isAlive = await IsServerAliveAsync(candidate.Ip, candidate.Port, 800);

                if (isAlive)
                {
                    return candidate;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[HEALTH CHECK] Server {candidate.Ip}:{candidate.Port} khong phan hoi, bo qua.");
                Console.ResetColor();
            }

            return null;
        }

        static async Task<bool> IsServerAliveAsync(string ip, int port, int timeoutMs)
        {
            try
            {
                using (TcpClient testClient = new TcpClient())
                {
                    Task connectTask = testClient.ConnectAsync(ip, port);
                    Task timeoutTask = Task.Delay(timeoutMs);

                    Task completedTask = await Task.WhenAny(connectTask, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        return false;
                    }

                    await connectTask;
                    return testClient.Connected;
                }
            }
            catch
            {
                return false;
            }
        }
    }

    class ChatServerNode
    {
        public string Ip { get; set; }
        public int Port { get; set; }

        public ChatServerNode(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }
    }
}