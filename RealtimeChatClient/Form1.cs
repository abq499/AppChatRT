using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;

namespace RealtimeChatClient
{
    public partial class Form1 : Form
    {
        private ChatClientLogic _chatLogic;
        private string _myUsername;

        private string _currentRoomId = "";
        private string _currentRoomName = "";

        private string _currentPrivateUser = "";
        private string _currentPrivateDisplayName = "";

        private string _myAvatarBase64 = "";

        private string _myFullName = "";
        private string _myEmail = "";

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
        private async void LbOnlineUsers_DoubleClick(object sender, EventArgs e)
        {
            if (lbOnlineUsers.SelectedItem == null)
                return;

            string selectedUsername = ExtractUsernameFromOnlineItem(lbOnlineUsers.SelectedItem.ToString());

            if (string.IsNullOrEmpty(selectedUsername))
                return;

            if (selectedUsername == _myUsername)
            {
                MessageBox.Show("Không thể chat riêng với chính mình!");
                return;
            }

            string selectedDisplayName = ExtractDisplayNameFromOnlineItem(lbOnlineUsers.SelectedItem.ToString());

            _currentRoomId = "";
            _currentRoomName = "";

            _currentPrivateUser = selectedUsername;
            _currentPrivateDisplayName = selectedDisplayName;

            lblCurrentRoom.Text = $"Đang ở: Chat riêng với {_currentPrivateDisplayName} ({_currentPrivateUser})";

            rtbChat.Clear();
            AppendToChat("System", $"Bạn đang chat riêng với {_currentPrivateDisplayName}.");

            await _chatLogic.RequestPrivateHistoryAsync(_currentPrivateUser);
        }
        private async void btnCreateRoom_Click(object sender, EventArgs e)
        {
            string roomName = txtRoomName.Text.Trim();

            if (string.IsNullOrWhiteSpace(roomName))
            {
                MessageBox.Show("Vui lòng nhập tên phòng!");
                return;
            }

            await _chatLogic.CreateRoomAsync(roomName);
        }

        private async void btnJoinRoom_Click(object sender, EventArgs e)
        {
            string roomId = txtRoomId.Text.Trim();

            if (string.IsNullOrWhiteSpace(roomId))
            {
                MessageBox.Show("Vui lòng nhập Room ID!");
                return;
            }

            await _chatLogic.JoinRoomAsync(roomId);
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
            string serverIp = txtServerIP.Text.Trim();
            string portText = txtServerPort.Text.Trim();

            if (string.IsNullOrEmpty(serverIp))
            {
                MessageBox.Show("Vui lòng nhập IP hoặc Host của Load Balancer!");
                btnConnect.Enabled = true;
                return;
            }

            if (!int.TryParse(portText, out int serverPort))
            {
                MessageBox.Show("Port không hợp lệ!");
                btnConnect.Enabled = true;
                return;
            }

            bool success = await _chatLogic.ConnectAsync(serverIp, serverPort, _myUsername);

            if (success)
            {
                AppendToChat("System", "Kết nối thành công!");
                btnSend.Enabled = true;
                btnSendFile.Enabled = true;
                btnCreateRoom.Enabled = true;
                btnJoinRoom.Enabled = true;
                btnLeaveRoom.Enabled = true;
                btnProfile.Enabled = true;

                txtUsername.Enabled = false;
                txtServerIP.Enabled = false;
                txtServerPort.Enabled = false;
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

            if (string.IsNullOrEmpty(msg))
                return;

            // 1. Nếu đang ở trong phòng thì gửi room message
            if (!string.IsNullOrEmpty(_currentRoomId))
            {
                await _chatLogic.SendRoomMessageAsync(_currentRoomId, msg);
                AppendToChat($"[Room: {_currentRoomName}] {GetMyDisplayName()}", msg, isMe: true);
                txtMessage.Clear();
                return;
            }

            // 2. Nếu đang chat riêng thì gửi private message
            if (!string.IsNullOrEmpty(_currentPrivateUser))
            {
                await _chatLogic.SendPrivateMessageAsync(_currentPrivateUser, msg);

                AppendToChat(
                    $"[Riêng cho {_currentPrivateDisplayName}]",
                    msg,
                    isMe: true,
                    isPrivate: true
                );

                txtMessage.Clear();
                return;
            }

            // 3. Nếu không ở room/private thì gửi chat chung
            await _chatLogic.SendMessageAsync(msg);
            AppendToChat(GetMyDisplayName(), msg, isMe: true, isPrivate: false);
            txtMessage.Clear();
        }
        private string GetMyDisplayName()
        {
            if (!string.IsNullOrWhiteSpace(_myFullName))
                return _myFullName;

            return _myUsername;
        }

        private async void btnSendFile_Click(object sender, EventArgs e)
        {
            if (lbOnlineUsers.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một người trong danh sách online để gửi file!");
                return;
            }

            string targetUser = ExtractUsernameFromOnlineItem(lbOnlineUsers.SelectedItem.ToString());

            if (string.IsNullOrEmpty(targetUser))
            {
                MessageBox.Show("Người nhận không hợp lệ!");
                return;
            }

            if (targetUser == _myUsername)
            {
                MessageBox.Show("Không thể gửi file cho chính mình!");
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn file muốn gửi";
                ofd.Filter = "All files (*.*)|*.*";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                FileInfo fi = new FileInfo(ofd.FileName);

                // Giới hạn 500KB để tránh nghẽn socket khi gửi base64 qua WriteLine
                const long maxFileSize = 500 * 1024;

                if (fi.Length > maxFileSize)
                {
                    MessageBox.Show(
                        $"File quá lớn ({fi.Length / 1024} KB). Vui lòng chọn file <= 500 KB để demo ổn định.",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                try
                {
                    AppendToChat("System", $"Đang gửi file '{ofd.SafeFileName}' cho {targetUser}...");

                    byte[] fileBytes = File.ReadAllBytes(ofd.FileName);
                    string base64String = Convert.ToBase64String(fileBytes);

                    await _chatLogic.SendFileBase64Async(targetUser, ofd.SafeFileName, base64String);

                    AppendToChat("System", $"Đã gửi file '{ofd.SafeFileName}' cho {targetUser}.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi gửi file: " + ex.Message);
                }
            }
        }

        private async void btnProfile_Click(object sender, EventArgs e)
        {
            using (ProfileForm profileForm = new ProfileForm(
                _myUsername,
                _myFullName,
                _myEmail,
                _myAvatarBase64,
                _chatLogic))
            {
                if (profileForm.ShowDialog(this) == DialogResult.OK)
                {
                    string newFullName = profileForm.FullName;
                    string newAvatarBase64 = profileForm.AvatarBase64;

                    await _chatLogic.UpdateProfileAsync(newFullName, newAvatarBase64);
                }
            }
        }
        private void SetMyProfile(string username, string fullName, string email, string avatarBase64)
        {
            _myUsername = username;
            _myFullName = fullName;
            _myEmail = email;

            if (!string.IsNullOrWhiteSpace(_myFullName))
                lblMyFullName.Text = _myFullName;
            else
                lblMyFullName.Text = _myUsername;

            

            if (!string.IsNullOrEmpty(avatarBase64))
            {
                SetMyAvatar(avatarBase64);
            }
        }
        private Image Base64ToImage(string base64)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }

        private void SetMyAvatar(string avatarBase64)
        {
            try
            {
                if (string.IsNullOrEmpty(avatarBase64))
                    return;

                _myAvatarBase64 = avatarBase64;

                Image img = Base64ToImage(avatarBase64);

                if (picMyAvatar.Image != null)
                {
                    picMyAvatar.Image.Dispose();
                }

                picMyAvatar.Image = img;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể hiển thị avatar: " + ex.Message);
            }
        }

        // Đã thêm từ khóa 'async' vào đây
        private void OnMessageReceived(string rawData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(OnMessageReceived), rawData);
                return;
            }

            var parts = rawData.Split(new[] { '|' }, 6);

            if (parts[0] == "MY_PROFILE" && parts.Length >= 5)
            {
                string username = parts[1];
                string fullName = parts[2];
                string email = parts[3];
                string avatarBase64 = parts[4];

                SetMyProfile(username, fullName, email, avatarBase64);
            }
            else if (parts[0] == "PROFILE_UPDATED" && parts.Length >= 5)
            {
                string username = parts[1];
                string fullName = parts[2];
                string email = parts[3];
                string avatarBase64 = parts[4];

                SetMyProfile(username, fullName, email, avatarBase64);

                MessageBox.Show("Cập nhật thông tin thành công!");
            }

            else if (parts[0] == "PROFILE_ERROR" && parts.Length >= 2)
            {
                MessageBox.Show(parts[1], "Lỗi cập nhật thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (parts[0] == "PASSWORD_CHANGED")
            {
                string message = parts.Length >= 2
                    ? parts[1]
                    : "Đổi mật khẩu thành công.";

                MessageBox.Show(message, "Đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (parts[0] == "PASSWORD_ERROR")
            {
                string message = parts.Length >= 2
                    ? parts[1]
                    : "Đổi mật khẩu thất bại.";

                MessageBox.Show(message, "Lỗi đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (parts[0] == "EMAIL_UPDATED")
            {
                string newEmail = parts.Length >= 2 ? parts[1] : "";

                if (!string.IsNullOrWhiteSpace(newEmail))
                {
                    _myEmail = newEmail;
                }

                MessageBox.Show("Cập nhật email thành công!", "Email", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (parts[0] == "EMAIL_ERROR")
            {
                string message = parts.Length >= 2
                    ? parts[1]
                    : "Cập nhật email thất bại.";

                MessageBox.Show(message, "Lỗi email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (parts[0] == "MY_AVATAR" && parts.Length >= 2)
            {
                // Giữ tương thích nếu server cũ còn gửi MY_AVATAR
                SetMyAvatar(parts[1]);
            }
            else if (parts[0] == "AVATAR_UPDATED" && parts.Length >= 2)
            {
                // Giữ tương thích nếu còn dùng update avatar riêng
                SetMyAvatar(parts[1]);
                MessageBox.Show("Cập nhật avatar thành công!");
            }
            else if (parts[0] == "AVATAR_ERROR" && parts.Length >= 2)
            {
                MessageBox.Show(parts[1], "Lỗi avatar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (parts[0] == "ROOM_CREATED" && parts.Length >= 3)
            {
                _currentRoomId = parts[1];
                _currentRoomName = parts[2];

                _currentPrivateUser = "";
                _currentPrivateDisplayName = "";

                txtRoomId.Text = _currentRoomId;
                lblCurrentRoom.Text = $"Đang ở: Phòng {_currentRoomName} ({_currentRoomId})";
                rtbChat.Clear();

                AppendToChat("System", $"Đã tạo phòng '{_currentRoomName}' với ID: {_currentRoomId}");
                MessageBox.Show($"Tạo phòng thành công!\nRoom ID: {_currentRoomId}", "Room Created");
            }
            else if (parts[0] == "ROOM_JOINED" && parts.Length >= 3)
            {
                _currentRoomId = parts[1];
                _currentRoomName = parts[2];

                _currentPrivateUser = "";
                _currentPrivateDisplayName = "";

                txtRoomId.Text = _currentRoomId;
                lblCurrentRoom.Text = $"Đang ở: Phòng {_currentRoomName} ({_currentRoomId})";
                rtbChat.Clear();

                AppendToChat("System", $"Đã tham gia phòng '{_currentRoomName}' ({_currentRoomId})");
            }
            else if (parts[0] == "ROOM_LEFT" && parts.Length >= 2)
            {
                string oldRoomId = _currentRoomId;
                string oldRoomName = _currentRoomName;

                _currentRoomId = "";
                _currentRoomName = "";

                _currentPrivateUser = "";
                _currentPrivateDisplayName = "";

                lblCurrentRoom.Text = "Đang ở: Chat chung";
                txtRoomId.Clear();

                rtbChat.Clear();
                AppendToChat("System", $"Đã rời phòng '{oldRoomName}' và quay về chat chung.");
            }
            else if (parts[0] == "ROOM_ERROR" && parts.Length >= 2)
            {
                MessageBox.Show(parts[1], "Lỗi phòng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (parts[0] == "ROOM_HISTORY")
            {
                if (parts.Length < 2)
                    return;

                string roomId = parts[1];

                if (roomId != _currentRoomId)
                    return;

                // Format mới: ROOM_HISTORY|roomId|username|fullName|encrypted|time
                if (parts.Length >= 6)
                {
                    string senderUsername = parts[2];
                    string senderFullName = parts[3];
                    string encryptedMsg = parts[4];
                    string time = parts[5];

                    string displayName = string.IsNullOrWhiteSpace(senderFullName)
                        ? senderUsername
                        : senderFullName;

                    string decryptedMsg = CryptoHelper.Decrypt(encryptedMsg);
                    bool isMyHistory = senderUsername == _myUsername;

                    AppendToChat(
                        $"[🕰️ Room {_currentRoomName} - {time}] {displayName}",
                        decryptedMsg,
                        isMe: isMyHistory
                    );
                }
                // Format cũ: ROOM_HISTORY|roomId|username|encrypted|time
                else if (parts.Length >= 5)
                {
                    string senderName = parts[2];
                    string encryptedMsg = parts[3];
                    string time = parts[4];

                    string decryptedMsg = CryptoHelper.Decrypt(encryptedMsg);
                    bool isMyHistory = senderName == _myUsername;

                    AppendToChat(
                        $"[🕰️ Room {_currentRoomName} - {time}] {senderName}",
                        decryptedMsg,
                        isMe: isMyHistory
                    );
                }
            }
            else if (parts[0] == "ROOM_MSG")
            {
                if (parts.Length < 2)
                    return;

                string roomId = parts[1];

                if (roomId != _currentRoomId)
                    return;

                // Format mới: ROOM_MSG|roomId|username|fullName|encrypted
                if (parts.Length >= 5)
                {
                    string senderUsername = parts[2];
                    string senderFullName = parts[3];
                    string encryptedMsg = parts[4];

                    string displayName = string.IsNullOrWhiteSpace(senderFullName)
                        ? senderUsername
                        : senderFullName;

                    string decryptedMsg = CryptoHelper.Decrypt(encryptedMsg);

                    AppendToChat($"[Room {_currentRoomName}]", $"{displayName}: {decryptedMsg}", isMe: false);
                }
                // Format cũ: ROOM_MSG|roomId|username|encrypted
                else if (parts.Length >= 4)
                {
                    string senderName = parts[2];
                    string encryptedMsg = parts[3];

                    string decryptedMsg = CryptoHelper.Decrypt(encryptedMsg);

                    AppendToChat($"[Room {_currentRoomName}]", $"{senderName}: {decryptedMsg}", isMe: false);
                }
            }
            else if (parts[0] == "MSG")
            {
                if (!string.IsNullOrEmpty(_currentRoomId) || !string.IsNullOrEmpty(_currentPrivateUser))
                    return;

                // Format mới: MSG|username|fullName|encrypted
                if (parts.Length >= 4)
                {
                    string senderUsername = parts[1];
                    string senderFullName = parts[2];
                    string encryptedMsg = parts[3];

                    string displayName = string.IsNullOrWhiteSpace(senderFullName)
                        ? senderUsername
                        : senderFullName;

                    string decryptedMsg = CryptoHelper.Decrypt(encryptedMsg);
                    AppendToChat(displayName, decryptedMsg, isMe: false);
                }
                // Format cũ: MSG|username|encrypted
                else if (parts.Length >= 3)
                {
                    string decryptedMsg = CryptoHelper.Decrypt(parts[2]);
                    AppendToChat(parts[1], decryptedMsg, isMe: false);
                }
            }
            else if (parts.Length >= 2 && parts[0] == "USERS")
            {
                UpdateOnlineUsersList(parts[1]);
            }
            else if (parts[0] == "PRIVATE_HISTORY")
            {
                // Format: PRIVATE_HISTORY|username|fullName|encrypted|time
                if (parts.Length >= 5)
                {
                    string senderUsername = parts[1];
                    string senderFullName = parts[2];
                    string encryptedMsg = parts[3];
                    string time = parts[4];

                    string displayName = string.IsNullOrWhiteSpace(senderFullName)
                        ? senderUsername
                        : senderFullName;

                    string decryptedMsg = CryptoHelper.Decrypt(encryptedMsg);

                    bool isMe = senderUsername == _myUsername;

                    AppendToChat(
                        $"[🕰️ {time}] {displayName}",
                        decryptedMsg,
                        isMe: isMe,
                        isPrivate: true
                    );
                }
            }
            else if (parts[0] == "PRIVATE")
            {
                // Format mới: PRIVATE|username|fullName|encrypted
                if (parts.Length >= 4)
                {
                    string senderUsername = parts[1];
                    string senderFullName = parts[2];
                    string encryptedMsg = parts[3];

                    string displayName = string.IsNullOrWhiteSpace(senderFullName)
                        ? senderUsername
                        : senderFullName;

                    string decryptedMsg = CryptoHelper.Decrypt(encryptedMsg);

                    // Nếu đang chat riêng đúng với người gửi thì hiện trong khung hiện tại
                    if (_currentPrivateUser == senderUsername)
                    {
                        AppendToChat($"[Mật từ {displayName}]", decryptedMsg, isMe: false, isPrivate: true);
                    }
                    else if (string.IsNullOrEmpty(_currentRoomId))
                    {
                        // Nếu đang ở chat chung thì vẫn báo có tin riêng
                        AppendToChat($"[Mật từ {displayName}]", decryptedMsg, isMe: false, isPrivate: true);
                    }
                    else
                    {
                        // Nếu đang ở room, không chen tin vào room. Báo nhẹ bằng MessageBox.
                        MessageBox.Show($"Bạn có tin nhắn riêng từ {displayName}.", "Tin nhắn riêng");
                    }
                }
                // Format cũ: PRIVATE|username|encrypted
                else if (parts.Length >= 3)
                {
                    string decryptedMsg = CryptoHelper.Decrypt(parts[2]);
                    AppendToChat($"[Mật từ {parts[1]}]", decryptedMsg, isMe: false, isPrivate: true);
                }
            }
            else if (parts[0] == "HISTORY")
            {
                if (!string.IsNullOrEmpty(_currentRoomId) || !string.IsNullOrEmpty(_currentPrivateUser))
                    return;

                // Format mới: HISTORY|username|fullName|encrypted|time
                if (parts.Length >= 5)
                {
                    string senderUsername = parts[1];
                    string senderFullName = parts[2];
                    string encryptedMsg = parts[3];
                    string time = parts[4];

                    string displayName = string.IsNullOrWhiteSpace(senderFullName)
                        ? senderUsername
                        : senderFullName;

                    string decryptedMsg = CryptoHelper.Decrypt(encryptedMsg);
                    bool isMyHistory = senderUsername == _myUsername;

                    AppendToChat($"[🕰️ {time}] {displayName}", decryptedMsg, isMe: isMyHistory);
                }
                // Format cũ: HISTORY|username|encrypted|time
                else if (parts.Length >= 4)
                {
                    string senderName = parts[1];
                    string decryptedMsg = CryptoHelper.Decrypt(parts[2]);
                    string time = parts[3];

                    bool isMyHistory = senderName == _myUsername;
                    AppendToChat($"[🕰️ {time}] {senderName}", decryptedMsg, isMe: isMyHistory);
                }
            }
            else if (parts[0] == "FILE_INCOMING" && parts.Length >= 4)
            {
                string senderName = parts[1];
                string fileName = parts[2];
                string base64Data = parts[3];

                var result = MessageBox.Show(
                    $"{senderName} muốn gửi file: {fileName}. Bạn có nhận không?",
                    "Nhận file",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    SaveFileDialog sfd = new SaveFileDialog { FileName = fileName };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        byte[] fileBytes = Convert.FromBase64String(base64Data);
                        File.WriteAllBytes(sfd.FileName, fileBytes);

                        AppendToChat("System", $"Đã nhận file và lưu tại: {sfd.FileName}");
                    }
                }
            }
        }

        // HÀM MỚI: Cập nhật ListBox
        private void UpdateOnlineUsersList(string userListStr)
        {
            string selectedUsername = "";

            if (lbOnlineUsers.SelectedItem != null)
            {
                selectedUsername = ExtractUsernameFromOnlineItem(lbOnlineUsers.SelectedItem.ToString());
            }

            lbOnlineUsers.BeginUpdate();
            lbOnlineUsers.Items.Clear();

            if (!string.IsNullOrEmpty(userListStr))
            {
                string[] users = userListStr.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (string rawUser in users)
                {
                    string item = rawUser.Trim();
                    if (string.IsNullOrEmpty(item)) continue;

                    string username = item;
                    string fullName = item;

                    if (item.Contains("::"))
                    {
                        string[] parts = item.Split(new[] { "::" }, 2, StringSplitOptions.None);
                        username = parts[0].Trim();
                        fullName = parts[1].Trim();

                        if (string.IsNullOrWhiteSpace(fullName))
                            fullName = username;
                    }

                    string displayText;

                    if (username == _myUsername)
                        displayText = $"{fullName} ({username}) (You)";
                    else
                        displayText = $"{fullName} ({username})";

                    lbOnlineUsers.Items.Add(displayText);
                }
            }

            if (!string.IsNullOrEmpty(selectedUsername))
            {
                for (int i = 0; i < lbOnlineUsers.Items.Count; i++)
                {
                    string itemUsername = ExtractUsernameFromOnlineItem(lbOnlineUsers.Items[i].ToString());

                    if (itemUsername == selectedUsername)
                    {
                        lbOnlineUsers.SelectedIndex = i;
                        break;
                    }
                }
            }

            lbOnlineUsers.EndUpdate();
        }

        private string ExtractUsernameFromOnlineItem(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                return "";

            item = item.Replace(" (You)", "").Trim();

            int openIndex = item.LastIndexOf('(');
            int closeIndex = item.LastIndexOf(')');

            if (openIndex >= 0 && closeIndex > openIndex)
            {
                return item.Substring(openIndex + 1, closeIndex - openIndex - 1).Trim();
            }

            return item.Trim();
        }
        private string ExtractDisplayNameFromOnlineItem(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                return "";

            item = item.Replace(" (You)", "").Trim();

            int openIndex = item.LastIndexOf('(');

            if (openIndex > 0)
            {
                return item.Substring(0, openIndex).Trim();
            }

            return item.Trim();
        }

        private async void btnLeaveRoom_Click(object sender, EventArgs e)
        {
            // Nếu đang chat riêng thì quay về chat chung
            if (!string.IsNullOrEmpty(_currentPrivateUser))
            {
                string oldName = _currentPrivateDisplayName;

                _currentPrivateUser = "";
                _currentPrivateDisplayName = "";

                lblCurrentRoom.Text = "Đang ở: Chat chung";
                rtbChat.Clear();

                AppendToChat("System", $"Đã quay về chat chung từ cuộc trò chuyện với {oldName}.");

                await _chatLogic.RequestGeneralHistoryAsync();

                return;
            }

            // Nếu đang ở room thì rời room thật
            if (!string.IsNullOrEmpty(_currentRoomId))
            {
                await _chatLogic.LeaveRoomAsync(_currentRoomId);
                return;
            }

            MessageBox.Show("Bạn hiện chưa ở trong phòng hoặc chat riêng nào!");
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
            btnCreateRoom.Enabled = false;
            btnJoinRoom.Enabled = false;
            btnLeaveRoom.Enabled = false;
            btnProfile.Enabled = false;

            _myAvatarBase64 = "";
            picMyAvatar.Image = null;
            _myFullName = "";
            _myEmail = "";
            lblMyFullName.Text = "";
            

            txtUsername.Enabled = true;
            txtServerIP.Enabled = true;
            txtServerPort.Enabled = true;

            _currentRoomId = "";
            _currentRoomName = "";
            lblCurrentRoom.Text = "Đang ở: Chat chung";
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

        private void lblMyFullName_Click(object sender, EventArgs e)
        {

        }
    }
}