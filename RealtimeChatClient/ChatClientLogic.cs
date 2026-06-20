using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeChatClient
{
    public class ChatClientLogic
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;

        // Event để bắn dữ liệu nhận được lên UI
        public event Action<string> MessageReceived;
        public event Action Disconnected;

        public async Task<bool> ConnectAsync(string loadBalancerIp, int loadBalancerPort, string username)
        {
            try
            {
                // --- NHỊP 1: HỎI ĐƯỜNG LOAD BALANCER ---
                string targetIp = "";
                int targetPort = 0;

                using (TcpClient lbClient = new TcpClient())
                {
                    // Kết nối tới Điều phối viên
                    await lbClient.ConnectAsync(loadBalancerIp, loadBalancerPort);
                    using (NetworkStream lbStream = lbClient.GetStream())
                    using (StreamReader lbReader = new StreamReader(lbStream, Encoding.UTF8))
                    {
                        // Đợi Điều phối viên trả về địa chỉ Server rảnh
                        string response = await lbReader.ReadLineAsync();
                        if (response != null && response.StartsWith("REDIRECT|"))
                        {
                            var parts = response.Split('|');
                            targetIp = parts[1];
                            targetPort = int.Parse(parts[2]);
                        }
                        else
                        {
                            return false; // Load balancer trả về sai định dạng
                        }
                    }
                } // Tự động đóng kết nối với Load Balancer tại đây

                // --- NHỊP 2: KẾT NỐI VÀO CHAT SERVER THỰC SỰ ---
                _client = new TcpClient();
                await _client.ConnectAsync(targetIp, targetPort);
                _stream = _client.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };

                // ================= CHÈN ĐOẠN BẮT TAY VÀO ĐÂY =================

                // 1. Tạo cặp khóa RSA
                CryptoHelper.GenerateRSAKeys(out string pubKey, out string privKey);

                // 2. Gửi Public Key cho Server
                await _writer.WriteLineAsync($"RSA_PUB|{pubKey}");

                // 3. Nhận AES Key đã được Server mã hóa
                string aesMsg = await _reader.ReadLineAsync();

                if (aesMsg != null && aesMsg.StartsWith("AES_KEY|"))
                {
                    string encryptedAES = aesMsg.Substring(8);

                    // 4. Giải mã AES bằng Private Key
                    string decryptedAES = CryptoHelper.RSADecrypt(encryptedAES, privKey);

                    // 5. Cập nhật khóa AES dùng để mã hóa chat
                    CryptoHelper.DynamicAESKey = decryptedAES;
                }
                else
                {
                    return false;
                }

                // =============================================================

                // Gửi username sau khi bắt tay xong
                await _writer.WriteLineAsync($"LOGIN|{username}");

                // Khởi chạy luồng đọc dữ liệu liên tục (Background)
                _ = Task.Run(() => ReceiveMessagesAsync());

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            try
            {
                while (_client.Connected)
                {
                    string message = await _reader.ReadLineAsync();
                    if (message == null) break; // Server đóng kết nối

                    MessageReceived?.Invoke(message);
                }
            }
            catch (Exception)
            {
                // Xử lý lỗi đứt cáp, rớt mạng
            }
            finally
            {
                Disconnect();
                Disconnected?.Invoke();
            }
        }

        // Sửa hàm Chat Chung
        public async Task SendMessageAsync(string message)
        {
            if (_client != null && _client.Connected)
            {
                // Dùng CryptoHelper.Encrypt để khóa chữ lại
                string encryptedMsg = CryptoHelper.Encrypt(message);
                await _writer.WriteLineAsync($"MSG|{encryptedMsg}");
            }
        }

        // Sửa hàm Chat Riêng
        public async Task SendPrivateMessageAsync(string targetUser, string message)
        {
            if (_client != null && _client.Connected)
            {
                // Tương tự, khóa lại trước khi gửi
                string encryptedMsg = CryptoHelper.Encrypt(message);
                await _writer.WriteLineAsync($"PRIVATE|{targetUser}|{encryptedMsg}");
            }
        }
        public async Task SendFileAsync(string targetUser, string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            // Gửi header báo hiệu file
            await _writer.WriteLineAsync($"FILE|{targetUser}|{fi.Name}|{fi.Length}");

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[8192];
                int read;
                while ((read = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await _stream.WriteAsync(buffer, 0, read);
                }
            }
            await _stream.FlushAsync();
        }
        public async Task SendFileBase64Async(string targetUser, string fileName, string base64Data)
        {
            if (_client != null && _client.Connected)
            {
                // Cấu trúc gói tin: FILE | Người_Nhận | Tên_File | Nội_Dung_Base64
                await _writer.WriteLineAsync($"FILE|{targetUser}|{fileName}|{base64Data}");
            }
        }
        public void Disconnect()
        {
            _reader?.Close();
            _writer?.Close();
            _stream?.Close();
            _client?.Close();
        }
    }
}