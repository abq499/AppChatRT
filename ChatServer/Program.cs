using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient; // THÊM THƯ VIỆN NÀY
using RealtimeChatClient;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection;

class Program
{
    static ConcurrentDictionary<string, StreamWriter> connectedClients = new ConcurrentDictionary<string, StreamWriter>();
    static string MasterAESKey = "UIT_Cluster_Master_Key_123456789";

    static int lastSyncedMessageId = 0;
    static readonly object syncLock = new object();

    static int lastSyncedFileId = 0;
    static readonly object fileSyncLock = new object();

    static int lastSyncedRoomMessageId = 0;
    static readonly object roomMsgSyncLock = new object();

    static readonly HashSet<int> processedRoomMessageIds = new HashSet<int>();
    static readonly object processedRoomMsgLock = new object();

    // Chuỗi kết nối Database của em
    static string connString = AppConfig.ConnectionString;
    // Thêm biến lưu Port của Server này
    static int currentPort = 8888;

    static string GenerateRoomId()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    static async Task<bool> IsUserInRoomAsync(string username, string roomId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                SELECT COUNT(1)
                FROM RoomMembers
                WHERE RoomId = @roomId AND Username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    cmd.Parameters.AddWithValue("@username", username);

                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ROOM ERROR] IsUserInRoom: " + ex.Message);
            return false;
        }
    }

    static async Task<int> GetLatestRoomMessageIdAsync()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = "SELECT ISNULL(MAX(Id), 0) FROM RoomMessages";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi lấy RoomMessageId mới nhất: " + ex.Message);
            return 0;
        }
    }

    static async Task<int> SaveRoomMessageToDB(string roomId, string sender, string content)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                INSERT INTO RoomMessages (RoomId, Sender, Content)
                OUTPUT INSERTED.Id
                VALUES (@roomId, @sender, @content)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    cmd.Parameters.AddWithValue("@sender", sender);
                    cmd.Parameters.AddWithValue("@content", content);

                    object result = await cmd.ExecuteScalarAsync();
                    int insertedId = Convert.ToInt32(result);

                    Console.WriteLine($"[ROOM MSG DB] Room {roomId}: {sender} - Id {insertedId}");

                    return insertedId;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ROOM MSG DB ERROR] " + ex.Message);
            return 0;
        }
    }

    static async Task DispatchRoomMessageAsync(DbRoomMessage msg)
    {
        foreach (var kvp in connectedClients)
        {
            string localUsername = kvp.Key;
            StreamWriter writer = kvp.Value;

            if (localUsername == msg.Sender)
                continue;

            bool isMember = await IsUserInRoomAsync(localUsername, msg.RoomId);

            if (isMember)
            {
                try
                {
                    string senderFullName = await GetFullNameFromDB(msg.Sender);

                    await writer.WriteLineAsync(
                        $"ROOM_MSG|{msg.RoomId}|{msg.Sender}|{senderFullName}|{msg.Content}"
                    );
                    Console.WriteLine($"[ROOM MSG DISPATCH] Room {msg.RoomId}: {msg.Sender} -> {localUsername}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[ROOM MSG DISPATCH ERROR] " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"[ROOM MSG SKIP] {localUsername} không thuộc room {msg.RoomId}");
            }
        }
    }
    static async Task SyncRoomMessagesFromDatabaseAsync()
    {
        while (true)
        {
            try
            {
                await Task.Delay(1000);

                List<DbRoomMessage> recentMessages = new List<DbRoomMessage>();

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string query = @"
                    SELECT *
                    FROM (
                        SELECT TOP 100 Id, RoomId, Sender, Content, SendTime
                        FROM RoomMessages
                        ORDER BY Id DESC
                    ) AS RecentMessages
                    ORDER BY Id ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            recentMessages.Add(new DbRoomMessage
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                RoomId = reader["RoomId"].ToString(),
                                Sender = reader["Sender"].ToString(),
                                Content = reader["Content"].ToString(),
                                SendTime = Convert.ToDateTime(reader["SendTime"])
                            });
                        }
                    }
                }

                foreach (var msg in recentMessages)
                {
                    // Nếu message này server đã xử lý rồi thì bỏ qua.
                    if (!TryMarkRoomMessageProcessed(msg.Id))
                        continue;

                    // Nếu sender đang nằm trên server này thì tin đó đã được phát ngay lúc gửi.
                    if (connectedClients.ContainsKey(msg.Sender))
                        continue;

                    await DispatchRoomMessageAsync(msg);

                    Console.WriteLine($"[ROOM MSG SYNC] Room {msg.RoomId}: {msg.Sender} - Id {msg.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ROOM MSG SYNC ERROR] " + ex.Message);
            }
        }
    }

    static bool TryMarkRoomMessageProcessed(int messageId)
    {
        lock (processedRoomMsgLock)
        {
            if (processedRoomMessageIds.Contains(messageId))
                return false;

            processedRoomMessageIds.Add(messageId);
            return true;
        }
    }

    static async Task MarkExistingRoomMessagesAsProcessedAsync()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                SELECT TOP 200 Id
                FROM RoomMessages
                ORDER BY Id DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int id = Convert.ToInt32(reader["Id"]);

                        lock (processedRoomMsgLock)
                        {
                            processedRoomMessageIds.Add(id);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ROOM MSG INIT ERROR] " + ex.Message);
        }
    }
    static async Task<bool> RoomExistsAsync(string roomId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = "SELECT COUNT(1) FROM Rooms WHERE RoomId = @roomId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roomId", roomId);

                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ROOM ERROR] RoomExists: " + ex.Message);
            return false;
        }
    }

    static async Task<string> GetRoomNameAsync(string roomId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = "SELECT RoomName FROM Rooms WHERE RoomId = @roomId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roomId", roomId);

                    object result = await cmd.ExecuteScalarAsync();

                    if (result == null || result == DBNull.Value)
                        return "";

                    return result.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ROOM ERROR] GetRoomName: " + ex.Message);
            return "";
        }
    }

    static async Task<(bool Success, string RoomId, string RoomName, string Error)> CreateRoomAsync(string ownerUsername, string roomName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(roomName))
                return (false, "", "", "Tên phòng không được để trống.");

            string roomId;

            // Sinh RoomId cho tới khi không bị trùng
            do
            {
                roomId = GenerateRoomId();
            }
            while (await RoomExistsAsync(roomId));

            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        string insertRoomQuery = @"
                        INSERT INTO Rooms (RoomId, RoomName, OwnerUsername)
                        VALUES (@roomId, @roomName, @owner)";

                        using (SqlCommand cmd = new SqlCommand(insertRoomQuery, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@roomId", roomId);
                            cmd.Parameters.AddWithValue("@roomName", roomName);
                            cmd.Parameters.AddWithValue("@owner", ownerUsername);

                            await cmd.ExecuteNonQueryAsync();
                        }

                        string insertMemberQuery = @"
                        INSERT INTO RoomMembers (RoomId, Username)
                        VALUES (@roomId, @username)";

                        using (SqlCommand cmd = new SqlCommand(insertMemberQuery, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@roomId", roomId);
                            cmd.Parameters.AddWithValue("@username", ownerUsername);

                            await cmd.ExecuteNonQueryAsync();
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }

            Console.WriteLine($"[ROOM] {ownerUsername} created room {roomId} - {roomName}");

            return (true, roomId, roomName, "");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ROOM ERROR] CreateRoom: " + ex.Message);
            return (false, "", "", ex.Message);
        }
    }

    static async Task<(bool Success, string RoomName, string Error)> JoinRoomAsync(string username, string roomId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(roomId))
                return (false, "", "Room ID không được để trống.");

            roomId = roomId.Trim();

            if (!await RoomExistsAsync(roomId))
                return (false, "", "Room ID không tồn tại.");

            string roomName = await GetRoomNameAsync(roomId);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                IF NOT EXISTS (
                    SELECT 1 FROM RoomMembers 
                    WHERE RoomId = @roomId AND Username = @username
                )
                BEGIN
                    INSERT INTO RoomMembers (RoomId, Username)
                    VALUES (@roomId, @username)
                END";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    cmd.Parameters.AddWithValue("@username", username);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            Console.WriteLine($"[ROOM] {username} joined room {roomId}");

            return (true, roomName, "");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ROOM ERROR] JoinRoom: " + ex.Message);
            return (false, "", ex.Message);
        }
    }
    static async Task<(bool Success, string Error)> LeaveRoomAsync(string username, string roomId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(roomId))
                return (false, "Room ID không được để trống.");

            roomId = roomId.Trim();

            if (!await RoomExistsAsync(roomId))
                return (false, "Room ID không tồn tại.");

            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                DELETE FROM RoomMembers
                WHERE RoomId = @roomId AND Username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    cmd.Parameters.AddWithValue("@username", username);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            Console.WriteLine($"[ROOM] {username} left room {roomId}");

            return (true, "");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ROOM ERROR] LeaveRoom: " + ex.Message);
            return (false, ex.Message);
        }
    }
    static async Task Main(string[] args)
    {
        Console.WriteLine("[CHAT SERVER] Đang chờ nhập port...");
        string inputPort = Console.ReadLine();
        if (int.TryParse(inputPort, out int parsedPort))
        {
            currentPort = parsedPort;
        }

        TcpListener listener = new TcpListener(IPAddress.Any, currentPort);
        listener.Start();
        Console.WriteLine($"[CHAT SERVER] Đang chạy tại Port {currentPort}...");

        lastSyncedMessageId = await GetLatestMessageIdAsync();
        lastSyncedFileId = await GetLatestFileIdAsync();
        lastSyncedRoomMessageId = await GetLatestRoomMessageIdAsync();
        await MarkExistingRoomMessagesAsProcessedAsync();

        _ = Task.Run(() => SyncMessagesFromDatabaseAsync());
        _ = Task.Run(() => SyncFilesFromDatabaseAsync());
        _ = Task.Run(() => SyncRoomMessagesFromDatabaseAsync());
        _ = Task.Run(() => SyncOnlineUsersAsync());
        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    static async Task<int> GetLatestFileIdAsync()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = "SELECT ISNULL(MAX(Id), 0) FROM FileTransfers";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi lấy FileId mới nhất: " + ex.Message);
            return 0;
        }
    }

    static async Task SaveFileToDB(string sender, string receiver, string fileName, string base64Data)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                INSERT INTO FileTransfers (Sender, Receiver, FileName, Base64Data)
                VALUES (@s, @r, @f, @b)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@s", sender);
                    cmd.Parameters.AddWithValue("@r", receiver);
                    cmd.Parameters.AddWithValue("@f", fileName);
                    cmd.Parameters.AddWithValue("@b", base64Data);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            Console.WriteLine($"[FILE DB] {sender} -> {receiver}: {fileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[FILE DB ERROR] " + ex.Message);
        }
    }

    static async Task SyncFilesFromDatabaseAsync()
    {
        while (true)
        {
            try
            {
                await Task.Delay(1000);

                List<DbFileTransfer> newFiles = new List<DbFileTransfer>();

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string query = @"
                    SELECT Id, Sender, Receiver, FileName, Base64Data, SendTime
                    FROM FileTransfers
                    WHERE Id > @lastId
                    ORDER BY Id ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@lastId", lastSyncedFileId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                newFiles.Add(new DbFileTransfer
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Sender = reader["Sender"].ToString(),
                                    Receiver = reader["Receiver"].ToString(),
                                    FileName = reader["FileName"].ToString(),
                                    Base64Data = reader["Base64Data"].ToString(),
                                    SendTime = Convert.ToDateTime(reader["SendTime"])
                                });
                            }
                        }
                    }
                }

                foreach (var file in newFiles)
                {
                    await DispatchSyncedFileAsync(file);

                    lock (fileSyncLock)
                    {
                        if (file.Id > lastSyncedFileId)
                            lastSyncedFileId = file.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FILE SYNC ERROR] " + ex.Message);
            }
        }
    }

    static async Task DispatchSyncedFileAsync(DbFileTransfer file)
    {
        if (connectedClients.TryGetValue(file.Receiver, out StreamWriter targetWriter))
        {
            await targetWriter.WriteLineAsync(
                $"FILE_INCOMING|{file.Sender}|{file.FileName}|{file.Base64Data}"
            );

            Console.WriteLine($"[FILE SYNC] {file.Sender} -> {file.Receiver}: {file.FileName}");
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

                // Chỉ lấy lịch sử chat chung
                string query = @"
                SELECT Sender, Content, SendTime
                FROM Messages
                WHERE Receiver IS NULL
                ORDER BY SendTime ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string senderUsername = reader["Sender"].ToString();
                        string content = reader["Content"].ToString();
                        DateTime sendTime = Convert.ToDateTime(reader["SendTime"]);

                        string senderFullName = await GetFullNameFromDB(senderUsername);

                        await writer.WriteLineAsync(
                            $"HISTORY|{senderUsername}|{senderFullName}|{content}|{sendTime:HH:mm}"
                        );
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi đọc lịch sử chat chung: " + ex.Message);
        }
    }
    static async Task LoadRoomHistoryForClient(string roomId, StreamWriter writer)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                SELECT Sender, Content, SendTime
                FROM RoomMessages
                WHERE RoomId = @roomId
                ORDER BY SendTime ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roomId", roomId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string senderUsername = reader["Sender"].ToString();
                            string content = reader["Content"].ToString();
                            DateTime sendTime = Convert.ToDateTime(reader["SendTime"]);

                            string senderFullName = await GetFullNameFromDB(senderUsername);

                            await writer.WriteLineAsync(
                                $"ROOM_HISTORY|{roomId}|{senderUsername}|{senderFullName}|{content}|{sendTime:HH:mm}"
                            );
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ROOM HISTORY ERROR] " + ex.Message);
        }
    }

    static async Task<int> GetLatestMessageIdAsync()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = "SELECT ISNULL(MAX(Id), 0) FROM Messages";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi lấy MessageId mới nhất: " + ex.Message);
            return 0;
        }
    }

    static async Task SyncMessagesFromDatabaseAsync()
    {
        while (true)
        {
            try
            {
                await Task.Delay(1000);

                List<DbMessage> newMessages = new List<DbMessage>();

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string query = @"
                    SELECT Id, Sender, Receiver, Content, SendTime
                    FROM Messages
                    WHERE Id > @lastId
                    ORDER BY Id ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@lastId", lastSyncedMessageId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                newMessages.Add(new DbMessage
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Sender = reader["Sender"].ToString(),
                                    Receiver = reader["Receiver"] == DBNull.Value ? null : reader["Receiver"].ToString(),
                                    Content = reader["Content"].ToString(),
                                    SendTime = Convert.ToDateTime(reader["SendTime"])
                                });
                            }
                        }
                    }
                }

                foreach (var msg in newMessages)
                {
                    await DispatchSyncedMessageAsync(msg);

                    lock (syncLock)
                    {
                        if (msg.Id > lastSyncedMessageId)
                            lastSyncedMessageId = msg.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[SYNC DB ERROR] " + ex.Message);
            }
        }
    }

    static async Task DispatchSyncedMessageAsync(DbMessage msg)
    {
        // Nếu sender đang nằm trên chính server này thì bỏ qua,
        // vì server này đã xử lý tin nhắn lúc user gửi rồi.
        if (connectedClients.ContainsKey(msg.Sender))
            return;

        // Chat chung: Receiver NULL
        if (string.IsNullOrEmpty(msg.Receiver))
        {
            string senderFullName = await GetFullNameFromDB(msg.Sender);

            string broadcastMsg = $"MSG|{msg.Sender}|{senderFullName}|{msg.Content}";

            foreach (var kvp in connectedClients)
            {
                try
                {
                    await kvp.Value.WriteLineAsync(broadcastMsg);
                }
                catch { }
            }

            if (connectedClients.Count > 0)
            {
                Console.WriteLine($"[SYNC] Nhận tin chat chung từ DB: {msg.Sender}");
            }
        }
        else
        {
            if (connectedClients.TryGetValue(msg.Receiver, out StreamWriter targetWriter))
            {
                string senderFullName =
                    await GetFullNameFromDB(msg.Sender);

                string privateMsg =
                    $"PRIVATE|{msg.Sender}|{senderFullName}|{msg.Content}";

                await targetWriter.WriteLineAsync(privateMsg);

                Console.WriteLine($"[SYNC] Nhận tin riêng từ DB: {msg.Sender} -> {msg.Receiver}");
            }
        }
    }

    static async Task SyncOnlineUsersAsync()
    {
        while (true)
        {
            try
            {
                await Task.Delay(5000);

                // Cập nhật LastSeen cho các user đang nối vào server hiện tại
                await RefreshLocalOnlineUsersAsync();

                // Broadcast danh sách online toàn cụm cho client đang nối vào server này
                await BroadcastOnlineUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ONLINE SYNC ERROR] " + ex.Message);
            }
        }
    }

    static async Task<string> GetFullNameFromDB(string username)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = "SELECT FullName FROM Users WHERE Username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    object result = await cmd.ExecuteScalarAsync();

                    if (result == null || result == DBNull.Value)
                        return username;

                    string fullName = result.ToString();

                    if (string.IsNullOrWhiteSpace(fullName))
                        return username;

                    return fullName;
                }
            }
        }
        catch
        {
            return username;
        }
    }

    static async Task UpsertOnlineUserAsync(string username)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                IF EXISTS (SELECT 1 FROM OnlineUsers WHERE Username = @u)
                BEGIN
                    UPDATE OnlineUsers
                    SET ServerPort = @p, LastSeen = GETDATE()
                    WHERE Username = @u
                END
                ELSE
                BEGIN
                    INSERT INTO OnlineUsers (Username, ServerPort, LastSeen)
                    VALUES (@u, @p, GETDATE())
                END";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", currentPort);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ONLINE DB ERROR] Upsert: " + ex.Message);
        }
    }

    static async Task RemoveOnlineUserAsync(string username)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = "DELETE FROM OnlineUsers WHERE Username = @u";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ONLINE DB ERROR] Remove: " + ex.Message);
        }
    }

    static async Task RefreshLocalOnlineUsersAsync()
    {
        foreach (string username in connectedClients.Keys)
        {
            if (!string.IsNullOrWhiteSpace(username) && username != "Unknown")
            {
                await UpsertOnlineUserAsync(username);
            }
        }
    }
    static async Task<List<string>> GetAllOnlineUsersAsync()
    {
        List<string> users = new List<string>();

        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                SELECT Username
                FROM OnlineUsers
                WHERE LastSeen >= DATEADD(SECOND, -30, GETDATE())
                ORDER BY Username ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(reader["Username"].ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ONLINE DB ERROR] GetAll: " + ex.Message);
        }

        return users;
    }
    static async Task SaveAvatarToDB(string username, string avatarBase64)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                UPDATE Users
                SET AvatarBase64 = @avatar
                WHERE Username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@avatar", avatarBase64);
                    cmd.Parameters.AddWithValue("@username", username);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            Console.WriteLine($"[AVATAR] Updated avatar for {username}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[AVATAR ERROR] SaveAvatar: " + ex.Message);
        }
    }
    static async Task<UserProfile> GetUserProfileFromDB(string username)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                SELECT Username, FullName, Email, AvatarBase64
                FROM Users
                WHERE Username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new UserProfile
                            {
                                Username = reader["Username"]?.ToString() ?? "",
                                FullName = reader["FullName"]?.ToString() ?? "",
                                Email = reader["Email"]?.ToString() ?? "",
                                AvatarBase64 = reader["AvatarBase64"] == DBNull.Value
                                    ? ""
                                    : reader["AvatarBase64"].ToString()
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[PROFILE ERROR] GetUserProfile: " + ex.Message);
        }

        return new UserProfile
        {
            Username = username,
            FullName = "",
            Email = "",
            AvatarBase64 = ""
        };
    }

    static async Task UpdateUserProfileToDB(string username, string fullName, string avatarBase64)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                UPDATE Users
                SET FullName = @fullName,
                    AvatarBase64 = @avatar
                WHERE Username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fullName", fullName);
                    cmd.Parameters.AddWithValue("@avatar", string.IsNullOrEmpty(avatarBase64) ? DBNull.Value : avatarBase64);
                    cmd.Parameters.AddWithValue("@username", username);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            Console.WriteLine($"[PROFILE] Updated profile for {username}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[PROFILE ERROR] UpdateUserProfile: " + ex.Message);
        }
    }
    static async Task<(bool Success, string Error)> UpdateEmailAsync(string username, string newEmail)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
            {
                return (false, "Email không hợp lệ.");
            }

            newEmail = newEmail.Trim();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                // Kiểm tra email đã được tài khoản khác dùng chưa
                string checkQuery = @"
                SELECT COUNT(1)
                FROM Users
                WHERE Email = @email AND Username <> @username";

                using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@email", newEmail);
                    cmd.Parameters.AddWithValue("@username", username);

                    int count = (int)await cmd.ExecuteScalarAsync();

                    if (count > 0)
                    {
                        return (false, "Email này đã được tài khoản khác sử dụng.");
                    }
                }

                string updateQuery = @"
                UPDATE Users
                SET Email = @email,
                    IsEmailVerified = 1
                WHERE Username = @username";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@email", newEmail);
                    cmd.Parameters.AddWithValue("@username", username);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            Console.WriteLine($"[EMAIL] {username} updated email to {newEmail}");

            return (true, "");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[EMAIL ERROR] " + ex.Message);
            return (false, ex.Message);
        }
    }
    static async Task<(bool Success, string Error)> ChangePasswordAsync(
    string username,
    string oldPasswordHash,
    string newPasswordHash)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(oldPasswordHash) ||
                string.IsNullOrWhiteSpace(newPasswordHash))
            {
                return (false, "Dữ liệu mật khẩu không hợp lệ.");
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string checkQuery = @"
                SELECT PasswordHash
                FROM Users
                WHERE Username = @username";

                string currentHash = "";

                using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    object result = await cmd.ExecuteScalarAsync();

                    if (result == null || result == DBNull.Value)
                        return (false, "Không tìm thấy tài khoản.");

                    currentHash = result.ToString();
                }

                if (currentHash != oldPasswordHash)
                {
                    return (false, "Mật khẩu cũ không đúng.");
                }

                string updateQuery = @"
                UPDATE Users
                SET PasswordHash = @newPasswordHash
                WHERE Username = @username";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@newPasswordHash", newPasswordHash);
                    cmd.Parameters.AddWithValue("@username", username);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            Console.WriteLine($"[PASSWORD] {username} changed password.");

            return (true, "");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[PASSWORD ERROR] " + ex.Message);
            return (false, ex.Message);
        }
    }
    static async Task BroadcastOnlineUsers()
    {
        try
        {
            List<string> users = await GetAllOnlineUsersAsync();

            List<string> displayUsers = new List<string>();

            foreach (string username in users)
            {
                string fullName = await GetFullNameFromDB(username);

                // Format: username::FullName
                displayUsers.Add($"{username}::{fullName}");
            }

            string userList = string.Join(",", displayUsers);

            foreach (var client in connectedClients)
            {
                try
                {
                    await client.Value.WriteLineAsync("USERS|" + userList);
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ONLINE BROADCAST ERROR] " + ex.Message);
        }
    }

    static async Task<string> GetAvatarFromDB(string username)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = "SELECT AvatarBase64 FROM Users WHERE Username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    object result = await cmd.ExecuteScalarAsync();

                    if (result == null || result == DBNull.Value)
                        return "";

                    return result.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[AVATAR ERROR] GetAvatar: " + ex.Message);
            return "";
        }
    }
    static async Task LoadPrivateHistoryForClient(string username, string targetUser, StreamWriter writer)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                string query = @"
                SELECT Sender, Receiver, Content, SendTime
                FROM Messages
                WHERE 
                    (Sender = @me AND Receiver = @target)
                    OR
                    (Sender = @target AND Receiver = @me)
                ORDER BY SendTime ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@me", username);
                    cmd.Parameters.AddWithValue("@target", targetUser);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string senderUsername = reader["Sender"].ToString();
                            string content = reader["Content"].ToString();
                            DateTime sendTime = Convert.ToDateTime(reader["SendTime"]);

                            string senderFullName = await GetFullNameFromDB(senderUsername);

                            await writer.WriteLineAsync(
                                $"PRIVATE_HISTORY|{senderUsername}|{senderFullName}|{content}|{sendTime:HH:mm}"
                            );
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[PRIVATE HISTORY ERROR] " + ex.Message);
        }
    }

    static async Task BroadcastTypingAsync(string senderUsername, string rawMsg)
    {
        try
        {
            string senderFullName = await GetFullNameFromDB(senderUsername);
            var parts = rawMsg.Split('|');

            if (parts.Length < 2)
                return;

            string mode = parts[1].Trim().ToUpper();

            // Chat chung: TYPING|GENERAL
            if (mode == "GENERAL")
            {
                string typingMsg = $"TYPING|GENERAL|{senderUsername}|{senderFullName}";

                foreach (var kvp in connectedClients)
                {
                    if (kvp.Key == senderUsername)
                        continue;

                    try
                    {
                        await kvp.Value.WriteLineAsync(typingMsg);
                    }
                    catch { }
                }
            }
            // Chat riêng: TYPING|PRIVATE|targetUser
            else if (mode == "PRIVATE" && parts.Length >= 3)
            {
                string targetUser = parts[2].Trim();

                if (connectedClients.TryGetValue(targetUser, out StreamWriter targetWriter))
                {
                    await targetWriter.WriteLineAsync($"TYPING|PRIVATE|{senderUsername}|{senderFullName}");
                }
            }
            // Chat room: TYPING|ROOM|roomId
            else if (mode == "ROOM" && parts.Length >= 3)
            {
                string roomId = parts[2].Trim();
                string typingMsg = $"TYPING|ROOM|{roomId}|{senderUsername}|{senderFullName}";

                foreach (var kvp in connectedClients)
                {
                    string localUsername = kvp.Key;

                    if (localUsername == senderUsername)
                        continue;

                    bool isMember = await IsUserInRoomAsync(localUsername, roomId);

                    if (isMember)
                    {
                        try
                        {
                            await kvp.Value.WriteLineAsync(typingMsg);
                        }
                        catch { }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[TYPING ERROR] " + ex.Message);
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
            string rsaMsg = await reader.ReadLineAsync();

            // Load Balancer health check chỉ connect rồi ngắt, không gửi RSA_PUB.
            // Vì vậy bỏ qua im lặng, không xem là lỗi client.
            if (string.IsNullOrEmpty(rsaMsg))
            {
                return;
            }

            if (rsaMsg.StartsWith("RSA_PUB|"))
            {
                string clientPubKey = rsaMsg.Substring(8);

                string encryptedAES = CryptoHelper.RSAEncrypt(MasterAESKey, clientPubKey);

                await writer.WriteLineAsync($"AES_KEY|{encryptedAES}");
            }
            else
            {
                return;
            }

            string initMsg = await reader.ReadLineAsync();
            if (initMsg != null && initMsg.StartsWith("LOGIN|"))
            {
                username = initMsg.Substring(6);
                connectedClients.TryAdd(username, writer);

                await UpsertOnlineUserAsync(username);

                Console.WriteLine($"{username} connected on server {currentPort}.");

                await BroadcastOnlineUsers();

                // Gửi avatar hiện tại của user về client
                UserProfile profile = await GetUserProfileFromDB(username);

                await writer.WriteLineAsync(
                    $"MY_PROFILE|{profile.Username}|{profile.FullName}|{profile.Email}|{profile.AvatarBase64}"
                );


                await LoadHistoryForClient(username, writer);
            }

            while (client.Connected)
            {
                string rawMsg = await reader.ReadLineAsync();
                if (rawMsg == null) break;

                if (rawMsg.StartsWith("UPDATE_PROFILE|"))
                {
                    var parts = rawMsg.Split(new[] { '|' }, 3);

                    if (parts.Length < 3)
                    {
                        await writer.WriteLineAsync("PROFILE_ERROR|Dữ liệu cập nhật không hợp lệ.");
                        continue;
                    }

                    string fullName = parts[1].Trim();
                    string avatarBase64 = parts[2];

                    if (string.IsNullOrWhiteSpace(fullName))
                    {
                        await writer.WriteLineAsync("PROFILE_ERROR|Họ tên không được để trống.");
                        continue;
                    }

                    await UpdateUserProfileToDB(username, fullName, avatarBase64);

                    UserProfile updatedProfile = await GetUserProfileFromDB(username);

                    await writer.WriteLineAsync(
                        $"PROFILE_UPDATED|{updatedProfile.Username}|{updatedProfile.FullName}|{updatedProfile.Email}|{updatedProfile.AvatarBase64}"
                    );
                    await BroadcastOnlineUsers();
                }
                else if (rawMsg.StartsWith("CHANGE_PASSWORD|"))
                {
                    var parts = rawMsg.Split(new[] { '|' }, 3);

                    if (parts.Length < 3)
                    {
                        await writer.WriteLineAsync("PASSWORD_ERROR|Dữ liệu đổi mật khẩu không hợp lệ.");
                        continue;
                    }

                    string oldPasswordHash = parts[1];
                    string newPasswordHash = parts[2];

                    var result = await ChangePasswordAsync(username, oldPasswordHash, newPasswordHash);

                    if (result.Success)
                    {
                        await writer.WriteLineAsync("PASSWORD_CHANGED|Đổi mật khẩu thành công.");
                    }
                    else
                    {
                        await writer.WriteLineAsync($"PASSWORD_ERROR|{result.Error}");
                    }
                }
                else if (rawMsg.StartsWith("UPDATE_EMAIL|"))
                {
                    string newEmail = rawMsg.Substring("UPDATE_EMAIL|".Length).Trim();

                    var result = await UpdateEmailAsync(username, newEmail);

                    if (result.Success)
                    {
                        UserProfile updatedProfile = await GetUserProfileFromDB(username);

                        await writer.WriteLineAsync($"EMAIL_UPDATED|{updatedProfile.Email}");

                        // Gửi lại profile mới để client đồng bộ _myEmail
                        await writer.WriteLineAsync(
                            $"PROFILE_UPDATED|{updatedProfile.Username}|{updatedProfile.FullName}|{updatedProfile.Email}|{updatedProfile.AvatarBase64}"
                        );
                    }
                    else
                    {
                        await writer.WriteLineAsync($"EMAIL_ERROR|{result.Error}");
                    }
                }
                else if (rawMsg.StartsWith("UPDATE_AVATAR|"))
                {
                    string avatarBase64 = rawMsg.Substring("UPDATE_AVATAR|".Length);

                    if (string.IsNullOrWhiteSpace(avatarBase64))
                    {
                        await writer.WriteLineAsync("AVATAR_ERROR|Dữ liệu avatar không hợp lệ.");
                        continue;
                    }

                    await SaveAvatarToDB(username, avatarBase64);
                    await writer.WriteLineAsync($"AVATAR_UPDATED|{avatarBase64}");
                }
                else if (rawMsg.StartsWith("CREATE_ROOM|"))
                {
                    string roomName = rawMsg.Substring("CREATE_ROOM|".Length).Trim();

                    var result = await CreateRoomAsync(username, roomName);

                    if (result.Success)
                    {
                        await writer.WriteLineAsync($"ROOM_CREATED|{result.RoomId}|{result.RoomName}");
                        await LoadRoomHistoryForClient(result.RoomId, writer);
                    }
                    else
                    {
                        await writer.WriteLineAsync($"ROOM_ERROR|{result.Error}");
                    }
                }
                else if (rawMsg.StartsWith("JOIN_ROOM|"))
                {
                    string roomId = rawMsg.Substring("JOIN_ROOM|".Length).Trim();

                    var result = await JoinRoomAsync(username, roomId);

                    if (result.Success)
                    {
                        await writer.WriteLineAsync($"ROOM_JOINED|{roomId}|{result.RoomName}");
                        await LoadRoomHistoryForClient(roomId, writer);
                    }
                    else
                    {
                        await writer.WriteLineAsync($"ROOM_ERROR|{result.Error}");
                    }
                }
                else if (rawMsg.StartsWith("LEAVE_ROOM|"))
                {
                    string roomId = rawMsg.Substring("LEAVE_ROOM|".Length).Trim();

                    var result = await LeaveRoomAsync(username, roomId);

                    if (result.Success)
                    {
                        await writer.WriteLineAsync($"ROOM_LEFT|{roomId}");

                        // Sau khi rời phòng, gửi lại lịch sử chat chung / chat riêng liên quan user
                        await LoadHistoryForClient(username, writer);
                    }
                    else
                    {
                        await writer.WriteLineAsync($"ROOM_ERROR|{result.Error}");
                    }
                }
                else if (rawMsg.StartsWith("ROOM_MSG|"))
                {
                    var parts = rawMsg.Split(new[] { '|' }, 3);

                    if (parts.Length == 3)
                    {
                        string roomId = parts[1].Trim();
                        string content = parts[2];

                        if (!await RoomExistsAsync(roomId))
                        {
                            await writer.WriteLineAsync("ROOM_ERROR|Phòng không tồn tại.");
                            continue;
                        }

                        if (!await IsUserInRoomAsync(username, roomId))
                        {
                            await writer.WriteLineAsync("ROOM_ERROR|Bạn chưa tham gia phòng này.");
                            continue;
                        }

                        int insertedId = await SaveRoomMessageToDB(roomId, username, content);

                        if (insertedId > 0)
                        {
                            var msg = new DbRoomMessage
                            {
                                Id = insertedId,
                                RoomId = roomId,
                                Sender = username,
                                Content = content,
                                SendTime = DateTime.Now
                            };

                            // Đánh dấu message này server hiện tại đã xử lý.
                            TryMarkRoomMessageProcessed(insertedId);

                            // Phát ngay cho các thành viên đang nằm trên cùng server.
                            await DispatchRoomMessageAsync(msg);
                        }
                    }
                }
                else if (rawMsg.StartsWith("TYPING|"))
                {
                    await BroadcastTypingAsync(username, rawMsg);
                }
                else if (rawMsg == "GENERAL_HISTORY")
                {
                    await LoadHistoryForClient(username, writer);
                }
                else if (rawMsg.StartsWith("MSG|"))
                {
                    string content = rawMsg.Substring(4);

                    await SaveMessageToDB(username, null, content);

                    string senderFullName = await GetFullNameFromDB(username);

                    string broadcastMsg =
                        $"MSG|{username}|{senderFullName}|{content}";

                    foreach (var kvp in connectedClients)
                    {
                        if (kvp.Key != username)
                        {
                            await kvp.Value.WriteLineAsync(broadcastMsg);
                        }
                    }
                }
                else if (rawMsg.StartsWith("PRIVATE_HISTORY|"))
                {
                    string targetUser = rawMsg.Substring("PRIVATE_HISTORY|".Length).Trim();

                    if (!string.IsNullOrWhiteSpace(targetUser))
                    {
                        await LoadPrivateHistoryForClient(username, targetUser, writer);
                    }
                }
                else if (rawMsg.StartsWith("PRIVATE|"))
                {
                    var parts = rawMsg.Split(new[] { '|' }, 3);

                    if (parts.Length == 3)
                    {
                        string targetUser = parts[1];
                        string content = parts[2];

                        await SaveMessageToDB(username, targetUser, content);

                        string senderFullName = await GetFullNameFromDB(username);

                        string privateMsg =
                            $"PRIVATE|{username}|{senderFullName}|{content}";

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

                        if (string.IsNullOrWhiteSpace(targetUser) ||
                            string.IsNullOrWhiteSpace(fileName) ||
                            string.IsNullOrWhiteSpace(base64Data))
                        {
                            continue;
                        }

                        // Không gửi trực tiếp nữa.
                        // Lưu DB để server nào đang giữ người nhận thì server đó tự phát file.
                        await SaveFileToDB(username, targetUser, fileName, base64Data);
                    }
                }
            }
        }
        catch (IOException)
        {
            // Kết nối health-check từ Load Balancer thường connect rồi đóng rất nhanh.
            // Khi username vẫn là Unknown thì không xem đây là lỗi client thật.
            if (username != "Unknown")
            {
                Console.WriteLine($"[INFO] {username} mất kết nối đột ngột.");
            }
        }
        catch (SocketException)
        {
            if (username != "Unknown")
            {
                Console.WriteLine($"[INFO] {username} mất kết nối socket.");
            }
        }
        catch (Exception ex)
        {
            if (username != "Unknown")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[LỖI {username}]: {ex.Message}");
                Console.ResetColor();
            }
        }
        finally
        {
            // Nếu là kết nối health check từ Load Balancer thì username vẫn là Unknown.
            // Không cần remove DB, không cần broadcast, không cần log đỏ.
            if (username != "Unknown")
            {
                connectedClients.TryRemove(username, out _);
                await RemoveOnlineUserAsync(username);

                Console.WriteLine($"{username} disconnected from server {currentPort}.");
                await BroadcastOnlineUsers();
            }
        }
    }
}
class DbMessage
{
    public int Id { get; set; }
    public string Sender { get; set; }
    public string? Receiver { get; set; }
    public string Content { get; set; }
    public DateTime SendTime { get; set; }
}

class DbFileTransfer
{
    public int Id { get; set; }
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public string FileName { get; set; }
    public string Base64Data { get; set; }
    public DateTime SendTime { get; set; }
}
class UserProfile
{
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string AvatarBase64 { get; set; }
}
class DbRoomMessage
{
    public int Id { get; set; }
    public string RoomId { get; set; }
    public string Sender { get; set; }
    public string Content { get; set; }
    public DateTime SendTime { get; set; }
}