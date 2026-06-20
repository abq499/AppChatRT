using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace RealtimeChatClient
{
    public partial class Form1 : Form
    {
        private ChatClientLogic _chatLogic;
        private string _myUsername;

        // CŨ: public Form1()
        // MỚI NHƯ SAU:
        public Form1(string loggedInUser)
        {
            InitializeComponent();
            _chatLogic = new ChatClientLogic();

            _chatLogic.MessageReceived += OnMessageReceived;
            _chatLogic.Disconnected += OnDisconnected;
            lbOnlineUsers.DoubleClick += LbOnlineUsers_DoubleClick;

            // Tự động điền tên người dùng và khóa ô đó lại
            txtUsername.Text = loggedInUser;
            txtUsername.Enabled = false;
        }
        // THÊM HÀM NÀY ĐỂ XỬ LÝ KHI CLICK ĐÚP VÀO TÊN
        private void LbOnlineUsers_DoubleClick(object sender, EventArgs e)
        {
            if (lbOnlineUsers.SelectedItem != null)
            {
                string selectedUser = lbOnlineUsers.SelectedItem.ToString().Replace(" (You)", "");
                if (selectedUser != _myUsername)
                {
                    txtMessage.Text = $"@{selectedUser} ";
                    txtMessage.Focus(); // Đưa con trỏ nhấp nháy xuống ô nhập
                    txtMessage.SelectionStart = txtMessage.Text.Length;
                }
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            _myUsername = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(_myUsername))
            {
                MessageBox.Show("Vui lòng nhập tên!");
                return;
            }

            btnConnect.Enabled = false;
            AppendToChat("System", "Đang kết nối đến server...");

            // Giả định server chạy ở localhost port 8888
            bool success = await _chatLogic.ConnectAsync("127.0.0.1", 8800, _myUsername);

            if (success)
            {
                AppendToChat("System", "Kết nối thành công!");
                btnSend.Enabled = true;
                btnSendFile.Enabled = true;
                txtUsername.Enabled = false;
            }
            else
            {
                AppendToChat("System", "Không thể kết nối. Vui lòng thử lại.");
                btnConnect.Enabled = true;
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string msg = txtMessage.Text.Trim();
            if (!string.IsNullOrEmpty(msg))
            {
                // Kiểm tra xem có phải chat riêng không
                if (msg.StartsWith("@"))
                {
                    int spaceIndex = msg.IndexOf(' ');
                    if (spaceIndex > 1)
                    {
                        string targetUser = msg.Substring(1, spaceIndex - 1);
                        string actualMsg = msg.Substring(spaceIndex + 1);

                        await _chatLogic.SendPrivateMessageAsync(targetUser, actualMsg);
                        AppendToChat($"[Riêng cho {targetUser}]", actualMsg, isMe: true, isPrivate: true);
                        txtMessage.Clear();
                        return; // Dừng tại đây, không gửi broadcast nữa
                    }
                }

                // Nếu không có chữ @ thì gửi chat chung (Broadcast)
                await _chatLogic.SendMessageAsync(msg);
                AppendToChat(_myUsername, msg, isMe: true, isPrivate: false);
                txtMessage.Clear();
            }
        }
        private async void btnSendFile_Click(object sender, EventArgs e)
        {
            if (lbOnlineUsers.SelectedItem == null)
            {
                MessageBox.Show("Chọn một người trong danh sách để gửi file!");
                return;
            }
            string target = lbOnlineUsers.SelectedItem.ToString().Replace(" (You)", "");

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // THÊM ĐOẠN KIỂM TRA DUNG LƯỢNG FILE Ở ĐÂY
                System.IO.FileInfo fi = new System.IO.FileInfo(ofd.FileName);
                if (fi.Length > 200 * 1024) // Giới hạn 200 KB (204,800 bytes)
                {
                    MessageBox.Show($"File quá lớn ({fi.Length / 1024} KB)! Để tránh nghẽn Socket, vui lòng chọn file văn bản hoặc ảnh icon < 200KB.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                AppendToChat("System", $"Đang đóng gói file: {ofd.SafeFileName}...");

                byte[] fileBytes = System.IO.File.ReadAllBytes(ofd.FileName);
                string base64String = Convert.ToBase64String(fileBytes);

                await _chatLogic.SendFileBase64Async(target, ofd.SafeFileName, base64String);
                AppendToChat("System", "Gửi file hoàn tất!");
            }
        }

        // Đã thêm từ khóa 'async' vào đây
        private async void OnMessageReceived(string rawData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(OnMessageReceived), rawData);
                return;
            }

            // Sửa lại thành Split tối đa 4 phần (vì có thêm Base64 data)
            var parts = rawData.Split(new[] { '|' }, 4);

            if (parts.Length >= 3 && parts[0] == "MSG")
            {
                // Dùng Decrypt để dịch lại parts[2]
                string decryptedMsg = CryptoHelper.Decrypt(parts[2]);
                AppendToChat(parts[1], decryptedMsg, isMe: false);
            }
            else if (parts.Length >= 2 && parts[0] == "USERS")
            {
                UpdateOnlineUsersList(parts[1]);
            }
            else if (parts.Length >= 3 && parts[0] == "PRIVATE")
            {
                string decryptedMsg = CryptoHelper.Decrypt(parts[2]);
                AppendToChat($"[Mật từ {parts[1]}]", decryptedMsg, isMe: false, isPrivate: true);
            }
            else if (parts[0] == "HISTORY" && parts.Length >= 4)
            {
                string senderName = parts[1];
                string decryptedMsg = CryptoHelper.Decrypt(parts[2]); // Dịch lịch sử từ DB
                string time = parts[3];

                bool isMyHistory = (senderName == _myUsername);
                AppendToChat($"[🕰️ {time}] {senderName}", decryptedMsg, isMe: isMyHistory);
            }
            // NHÁNH XỬ LÝ NHẬN FILE (Đã fix)
            else if (parts[0] == "FILE_INCOMING" && parts.Length == 4)
            {
                string senderName = parts[1];
                string fileName = parts[2];
                string base64Data = parts[3];

                var result = MessageBox.Show($"{senderName} muốn gửi file: {fileName}. Bạn có nhận không?",
                                            "Nhận file", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    SaveFileDialog sfd = new SaveFileDialog { FileName = fileName };
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // Giải mã chuỗi Base64 ngược lại thành mảng byte và lưu xuống ổ cứng
                        byte[] fileBytes = Convert.FromBase64String(base64Data);
                        System.IO.File.WriteAllBytes(sfd.FileName, fileBytes);

                        AppendToChat("System", $"Đã nhận file và lưu tại: {sfd.FileName}");
                    }
                }
            }
        }

        // HÀM MỚI: Cập nhật ListBox
        private void UpdateOnlineUsersList(string userListStr)
        {
            lbOnlineUsers.Items.Clear();
            if (string.IsNullOrEmpty(userListStr)) return;

            string[] users = userListStr.Split(',');
            foreach (string u in users)
            {
                // Thêm (You) vào cạnh tên của mình cho dễ nhìn
                if (u == _myUsername)
                    lbOnlineUsers.Items.Add($"{u} (You)");
                else
                    lbOnlineUsers.Items.Add(u);
            }
        }

        private void OnDisconnected()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(OnDisconnected));
                return;
            }
            AppendToChat("System", "Đã mất kết nối với Server.");
            btnConnect.Enabled = true;
            btnSend.Enabled = false;
            btnSendFile.Enabled = false;
            txtUsername.Enabled = true;
        }

        // Thay chữ (bool isMe = false) thành thế này:
        private void AppendToChat(string sender, string message, bool isMe = false, bool isPrivate = false)
        {
            string time = DateTime.Now.ToString("HH:mm");

            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;

            // Chat riêng màu tím, hệ thống xám, mình xanh dương, người khác xanh lá
            if (isPrivate) rtbChat.SelectionColor = Color.Purple;
            else if (sender == "System") rtbChat.SelectionColor = Color.Gray;
            else if (isMe) rtbChat.SelectionColor = Color.Blue;
            else rtbChat.SelectionColor = Color.Green;

            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Bold);
            rtbChat.AppendText($"[{time}] {sender}: ");

            rtbChat.SelectionColor = isPrivate ? Color.Purple : Color.Black; // Chữ tin nhắn riêng cũng màu tím
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Regular);
            rtbChat.AppendText($"{message}\n");

            rtbChat.ScrollToCaret();
        }

        
    }
}