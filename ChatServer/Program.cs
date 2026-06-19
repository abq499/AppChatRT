using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient; // THÊM THƯ VIỆN NÀY

class Program
{
    static ConcurrentDictionary<string, StreamWriter> connectedClients = new ConcurrentDictionary<string, StreamWriter>();

    // Chuỗi kết nối Database của em
    static string connString = @"Server=LAPTOP-TKFEE1EB\XMSSQLSERVER;Database=ChatAppDB;Trusted_Connection=True;";

    static async Task Main(string[] args)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 8888);
        listener.Start();
        Console.WriteLine("Server started on port 8888...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    // --- HÀM MỚI 1: LƯU TIN NHẮN VÀO DB ---
    static async Task SaveMessageToDB(string sender, string receiver, string content)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                string query = "INSERT INTO Messages (Sender, Receiver, Content) VALUES (@s, @r, @c)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@s", sender);
                    cmd.Parameters.AddWithValue("@r", string.IsNullOrEmpty(receiver) ? (object)DBNull.Value : receiver);
                    cmd.Parameters.AddWithValue("@c", content);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex) { Console.WriteLine("Lỗi ghi DB: " + ex.Message); }
    }

    // --- HÀM MỚI 2: TẢI LỊCH SỬ CHO NGƯỜI VỪA VÀO ---
    static async Task LoadHistoryForClient(string username, StreamWriter writer)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                // Lấy chat chung (Receiver IS NULL) HOẶC chat riêng liên quan đến mình
                string query = @"SELECT Sender, Content, SendTime FROM Messages 
                                 WHERE Receiver IS NULL OR Receiver = @u OR Sender = @u 
                                 ORDER BY SendTime ASC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string s = reader["Sender"].ToString();
                            string c = reader["Content"].ToString();
                            DateTime t = Convert.ToDateTime(reader["SendTime"]);

                            // Bắn gói tin dạng: HISTORY|NguoiGui|NoiDung|ThoiGian
                            await writer.WriteLineAsync($"HISTORY|{s}|{c}|{t:HH:mm}");
                        }
                    }
                }
            }
        }
        catch (Exception ex) { Console.WriteLine("Lỗi đọc DB: " + ex.Message); }
    }

    static async Task BroadcastOnlineUsers()
    {
        string userList = "USERS|" + string.Join(",", connectedClients.Keys);
        foreach (var kvp in connectedClients)
        {
            try { await kvp.Value.WriteLineAsync(userList); }
            catch { }
        }
    }

    static async Task HandleClientAsync(TcpClient client)
    {
        using NetworkStream stream = client.GetStream();
        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

        string username = "Unknown";
        try
        {
            string initMsg = await reader.ReadLineAsync();
            if (initMsg != null && initMsg.StartsWith("LOGIN|"))
            {
                username = initMsg.Substring(6);
                connectedClients.TryAdd(username, writer);
                Console.WriteLine($"{username} connected.");

                await BroadcastOnlineUsers();

                // GỌI HÀM: Tải lịch sử ngay khi đăng nhập xong
                await LoadHistoryForClient(username, writer);
            }

            while (client.Connected)
            {
                string rawMsg = await reader.ReadLineAsync();
                if (rawMsg == null) break;

                if (rawMsg.StartsWith("MSG|"))
                {
                    string content = rawMsg.Substring(4);

                    // GỌI HÀM LƯU DB: Lưu chat chung
                    await SaveMessageToDB(username, null, content);

                    string broadcastMsg = $"MSG|{username}|{content}";
                    foreach (var kvp in connectedClients)
                    {
                        if (kvp.Key != username)
                        {
                            await kvp.Value.WriteLineAsync(broadcastMsg);
                        }
                    }
                }
                else if (rawMsg.StartsWith("PRIVATE|"))
                {
                    var parts = rawMsg.Split(new[] { '|' }, 3);
                    if (parts.Length == 3)
                    {
                        string targetUser = parts[1];
                        string content = parts[2];

                        // GỌI HÀM LƯU DB: Lưu chat riêng
                        await SaveMessageToDB(username, targetUser, content);

                        string privateMsg = $"PRIVATE|{username}|{content}";
                        if (connectedClients.TryGetValue(targetUser, out StreamWriter targetWriter))
                        {
                            await targetWriter.WriteLineAsync(privateMsg);
                        }
                    }
                }
                else if (rawMsg.StartsWith("FILE|"))
                {
                    var parts = rawMsg.Split(new[] { '|' }, 4);
                    if (parts.Length == 4)
                    {
                        string targetUser = parts[1];
                        string fileName = parts[2];
                        string base64Data = parts[3];

                        if (connectedClients.TryGetValue(targetUser, out StreamWriter targetWriter))
                        {
                            await targetWriter.WriteLineAsync($"FILE_INCOMING|{username}|{fileName}|{base64Data}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[LỖI {username}]: {ex.Message}");
            Console.ResetColor();
        }
        finally
        {
            connectedClients.TryRemove(username, out _);
            Console.WriteLine($"{username} disconnected.");
            _ = BroadcastOnlineUsers();
        }
    }
}