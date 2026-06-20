using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatLoadBalancer
{
    class Program
    {
        // Khai báo danh sách các Chat Server đang chạy (Em có thể thêm nhiều port hơn nếu muốn)
        static readonly int[] ChatServerPorts = { 8888, 8889 };

        // Biến đếm xoay vòng (Round-Robin)
        static int nextServerIndex = 0;

        static async Task Main(string[] args)
        {
            int lbPort = 8800;
            TcpListener listener = new TcpListener(IPAddress.Any, lbPort);
            listener.Start();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[LOAD BALANCER] Đang chạy tại Port {lbPort}...");
            Console.WriteLine($"[LOAD BALANCER] Sẵn sàng điều phối tới cụm Server: {string.Join(", ", ChatServerPorts)}");
            Console.ResetColor();

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = HandleClientRoutingAsync(client);
            }
        }

        static async Task HandleClientRoutingAsync(TcpClient client)
        {
            using (client) // Đảm bảo đóng kết nối ngay sau khi điều phối xong
            using (NetworkStream stream = client.GetStream())
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
            {
                try
                {
                    // 1. Thuật toán Round-Robin: Chọn port tiếp theo
                    int assignedPort = ChatServerPorts[nextServerIndex];

                    // Tăng index, nếu vượt qua số lượng server thì quay lại 0
                    nextServerIndex = (nextServerIndex + 1) % ChatServerPorts.Length;

                    // 2. Gửi lệnh chuyển hướng về cho Client
                    // Cấu trúc: REDIRECT|IP|Port
                    string redirectMsg = $"REDIRECT|127.0.0.1|{assignedPort}";
                    await writer.WriteLineAsync(redirectMsg);

                    Console.WriteLine($"[ĐIỀU PHỐI] Đã chỉ đường 1 Client mới tới Server Port {assignedPort}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[LỖI] {ex.Message}");
                }
            }
        }
    }
}