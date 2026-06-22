using Guna.UI2.WinForms;

namespace RealtimeChatClient
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
            
            rtbChat = new RichTextBox();
            txtMessage = new Guna.UI2.WinForms.Guna2TextBox();
            lbOnlineUsers = new ListBox();
            
            labelServerIP = new Label();
            txtServerIP = new Guna.UI2.WinForms.Guna2TextBox();
            labelServerPort = new Label();
            txtServerPort = new Guna.UI2.WinForms.Guna2TextBox();
            labelRoomName = new Label();
            txtRoomName = new Guna.UI2.WinForms.Guna2TextBox();
            
            labelRoomId = new Label();
            txtRoomId = new Guna.UI2.WinForms.Guna2TextBox();
            
            
            lblCurrentRoom = new Label();
            picMyAvatar = new PictureBox();
            btnConnect = new Guna.UI2.WinForms.Guna2Button();
            btnSend = new Guna.UI2.WinForms.Guna2Button();
            btnSendFile = new Guna.UI2.WinForms.Guna2Button();
            btnCreateRoom = new Guna.UI2.WinForms.Guna2Button();
            btnJoinRoom = new Guna.UI2.WinForms.Guna2Button();
            btnLeaveRoom = new Guna.UI2.WinForms.Guna2Button();
            btnProfile = new Guna.UI2.WinForms.Guna2Button();
            lblMyFullName = new Label();
            pnlTopHeader = new Guna2Panel();
            lblAppTitle = new Label();
            pnlLeftSidebar = new Guna2Panel();
            lblOnlineTitle = new Label();
            pnlCenterChat = new Guna2Panel();
            pnlRightTools = new Guna2Panel();
            pnlChatCard = new Guna2Panel();
            lblToolsTitle = new Label();
            lblConnectionTitle = new Label();
            lblRoomTitle = new Label();
            lblFileTitle = new Label();
            lblAccountStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)picMyAvatar).BeginInit();
            pnlTopHeader.SuspendLayout();
            pnlLeftSidebar.SuspendLayout();
            pnlCenterChat.SuspendLayout();
            pnlRightTools.SuspendLayout();
            SuspendLayout();
            // 
            // lblConnectionTitle
            // 
            lblConnectionTitle.AutoSize = true;
            lblConnectionTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblConnectionTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblConnectionTitle.Location = new Point(12, 48);
            lblConnectionTitle.Name = "lblConnectionTitle";
            lblConnectionTitle.Size = new Size(78, 23);
            lblConnectionTitle.TabIndex = 22;
            lblConnectionTitle.Text = "Kết nối";

            // 
            // lblRoomTitle
            // 
            lblRoomTitle.AutoSize = true;
            lblRoomTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRoomTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblRoomTitle.Location = new Point(12, 265);
            lblRoomTitle.Name = "lblRoomTitle";
            lblRoomTitle.Size = new Size(93, 23);
            lblRoomTitle.TabIndex = 23;
            lblRoomTitle.Text = "Phòng chat";

            // 
            // lblFileTitle
            // 
            lblFileTitle.AutoSize = true;
            lblFileTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFileTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblFileTitle.Location = new Point(12, 555);
            lblFileTitle.Name = "lblFileTitle";
            lblFileTitle.Size = new Size(58, 23);
            lblFileTitle.TabIndex = 24;
            lblFileTitle.Text = "Tệp tin";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(12, 80);
            txtUsername.Margin = new Padding(3, 4, 3, 4);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(250, 27);
            txtUsername.TabIndex = 0;
            txtUsername.Text = "SinhVien_UIT";
            txtUsername.BorderRadius = 8;
            txtUsername.BorderColor = Color.FromArgb(209, 213, 219);
            txtUsername.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtUsername.Font = new Font("Segoe UI", 10F);
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(12, 248);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(250, 36);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Kết nối";
            
            btnConnect.Click += btnConnect_Click;
            btnConnect.BorderRadius = 8;
            btnConnect.FillColor = Color.FromArgb(37, 99, 235);
            btnConnect.ForeColor = Color.White;
            btnConnect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConnect.DisabledState.FillColor = Color.FromArgb(209, 213, 219);
            btnConnect.DisabledState.ForeColor = Color.FromArgb(107, 114, 128);
            // 
            // rtbChat
            // 
            rtbChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbChat.BackColor = Color.White;
            rtbChat.BorderStyle = BorderStyle.None;
            rtbChat.Font = new Font("Segoe UI", 10F);
            rtbChat.Location = new Point(10, 10);
            rtbChat.Margin = new Padding(3, 4, 3, 4);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.Size = new Size(621, 500);
            rtbChat.TabIndex = 2;
            rtbChat.Text = "";
            // 
            // txtMessage
            // 
            txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtMessage.Font = new Font("Segoe UI", 10F);
            txtMessage.Location = new Point(12, 590);
            txtMessage.Margin = new Padding(3, 4, 3, 4);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(520, 30);
            txtMessage.TabIndex = 3;
            txtMessage.BorderRadius = 10;
            txtMessage.BorderColor = Color.FromArgb(209, 213, 219);
            txtMessage.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtMessage.PlaceholderText = "Nhập tin nhắn...";
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSend.Enabled = false;
            btnSend.Location = new Point(541, 588);
            btnSend.Margin = new Padding(3, 4, 3, 4);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(112, 36);
            btnSend.TabIndex = 4;
            btnSend.Text = "Gửi";
            
            btnSend.Click += btnSend_Click;
            btnSend.BorderRadius = 8;
            btnSend.FillColor = Color.FromArgb(37, 99, 235);
            btnSend.ForeColor = Color.White;
            btnSend.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSend.DisabledState.FillColor = Color.FromArgb(209, 213, 219);
            btnSend.DisabledState.ForeColor = Color.FromArgb(107, 114, 128);
            // 
            // lbOnlineUsers
            // 
            lbOnlineUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lbOnlineUsers.Font = new Font("Segoe UI", 10F);
            lbOnlineUsers.FormattingEnabled = true;
            lbOnlineUsers.ItemHeight = 23;
            lbOnlineUsers.Location = new Point(12, 55);
            lbOnlineUsers.Name = "lbOnlineUsers";
            lbOnlineUsers.Size = new Size(206, 570);
            lbOnlineUsers.TabIndex = 5;
            lbOnlineUsers.BorderStyle = BorderStyle.None;
            lbOnlineUsers.BackColor = Color.White;
            lbOnlineUsers.ForeColor = Color.FromArgb(17, 24, 39);
            // 
            // btnSendFile
            // 
            btnSendFile.Enabled = false;
            btnSendFile.Location = new Point(12, 585);
            btnSendFile.Name = "btnSendFile";
            btnSendFile.Size = new Size(250, 36);
            btnSendFile.TabIndex = 6;
            btnSendFile.Text = "Gửi file";
            
            btnSendFile.Click += btnSendFile_Click;
            btnSendFile.BorderRadius = 8;
            btnSendFile.FillColor = Color.FromArgb(37, 99, 235);
            btnSendFile.ForeColor = Color.White;
            btnSendFile.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSendFile.DisabledState.FillColor = Color.FromArgb(209, 213, 219);
            btnSendFile.DisabledState.ForeColor = Color.FromArgb(107, 114, 128);
            // 
            // labelServerIP
            // 
            labelServerIP.AutoSize = true;
            labelServerIP.Location = new Point(12, 120);
            labelServerIP.Name = "labelServerIP";
            labelServerIP.Size = new Size(69, 20);
            labelServerIP.TabIndex = 7;
            labelServerIP.Text = "Server IP:";
            labelServerIP.ForeColor = Color.FromArgb(75, 85, 99);
            labelServerIP.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            // 
            // txtServerIP
            // 
            txtServerIP.Location = new Point(12, 144);
            txtServerIP.Name = "txtServerIP";
            txtServerIP.Size = new Size(250, 27);
            txtServerIP.TabIndex = 8;
            txtServerIP.Text = "127.0.0.1";
            txtServerIP.BorderRadius = 8;
            txtServerIP.BorderColor = Color.FromArgb(209, 213, 219);
            txtServerIP.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtServerIP.Font = new Font("Segoe UI", 10F);
            // 
            // labelServerPort
            // 
            labelServerPort.AutoSize = true;
            labelServerPort.Location = new Point(12, 180);
            labelServerPort.Name = "labelServerPort";
            labelServerPort.Size = new Size(38, 20);
            labelServerPort.TabIndex = 9;
            labelServerPort.Text = "Port:";
            labelServerPort.ForeColor = Color.FromArgb(75, 85, 99);
            labelServerPort.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            // 
            // txtServerPort
            // 
            txtServerPort.Location = new Point(12, 204);
            txtServerPort.Name = "txtServerPort";
            txtServerPort.Size = new Size(250, 27);
            txtServerPort.TabIndex = 10;
            txtServerPort.Text = "8800";
            txtServerPort.BorderRadius = 8;
            txtServerPort.BorderColor = Color.FromArgb(209, 213, 219);
            txtServerPort.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtServerPort.Font = new Font("Segoe UI", 10F);
            // 
            // labelRoomName
            // 
            labelRoomName.AutoSize = true;
            labelRoomName.Location = new Point(12, 300);
            labelRoomName.Name = "labelRoomName";
            labelRoomName.Size = new Size(82, 20);
            labelRoomName.TabIndex = 11;
            labelRoomName.Text = "Tên phòng:";
            labelRoomName.ForeColor = Color.FromArgb(75, 85, 99);
            labelRoomName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            // 
            // txtRoomName
            // 
            txtRoomName.Location = new Point(12, 324);
            txtRoomName.Name = "txtRoomName";
            txtRoomName.Size = new Size(250, 27);
            txtRoomName.TabIndex = 12;
            txtRoomName.BorderRadius = 8;
            txtRoomName.BorderColor = Color.FromArgb(209, 213, 219);
            txtRoomName.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtRoomName.Font = new Font("Segoe UI", 10F);
            txtRoomName.PlaceholderText = "Nhập tên phòng";
            // 
            // btnCreateRoom
            // 
            btnCreateRoom.Enabled = false;
            btnCreateRoom.Location = new Point(12, 364);
            btnCreateRoom.Name = "btnCreateRoom";
            btnCreateRoom.Size = new Size(250, 36);
            btnCreateRoom.TabIndex = 13;
            btnCreateRoom.Text = "Tạo phòng";

            btnCreateRoom.Click += btnCreateRoom_Click;
            btnCreateRoom.BorderRadius = 8;
            btnCreateRoom.FillColor = Color.FromArgb(37, 99, 235);
            btnCreateRoom.ForeColor = Color.White;
            btnCreateRoom.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCreateRoom.DisabledState.FillColor = Color.FromArgb(209, 213, 219);
            btnCreateRoom.DisabledState.ForeColor = Color.FromArgb(107, 114, 128);
            // 
            // labelRoomId
            // 
            labelRoomId.AutoSize = true;
            labelRoomId.Location = new Point(12, 419);
            labelRoomId.Name = "labelRoomId";
            labelRoomId.Size = new Size(71, 20);
            labelRoomId.TabIndex = 14;
            labelRoomId.Text = "Room ID:";
            labelRoomId.ForeColor = Color.FromArgb(75, 85, 99);
            labelRoomId.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            // 
            // txtRoomId
            // 
            txtRoomId.Location = new Point(12, 443);
            txtRoomId.Name = "txtRoomId";
            txtRoomId.Size = new Size(250, 27);
            txtRoomId.TabIndex = 15;
            txtRoomId.BorderRadius = 8;
            txtRoomId.BorderColor = Color.FromArgb(209, 213, 219);
            txtRoomId.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtRoomId.Font = new Font("Segoe UI", 10F);
            txtRoomId.PlaceholderText = "Nhập Room ID";
            // 
            // btnJoinRoom
            // 
            btnJoinRoom.Enabled = false;
            btnJoinRoom.Location = new Point(12, 483);
            btnJoinRoom.Name = "btnJoinRoom";
            btnJoinRoom.Size = new Size(250, 36);
            btnJoinRoom.TabIndex = 16;
            btnJoinRoom.Text = "Join";

            btnJoinRoom.Click += btnJoinRoom_Click;

            btnJoinRoom.BorderRadius = 8;
            btnJoinRoom.FillColor = Color.FromArgb(37, 99, 235);
            btnJoinRoom.ForeColor = Color.White;
            btnJoinRoom.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnJoinRoom.DisabledState.FillColor = Color.FromArgb(209, 213, 219);
            btnJoinRoom.DisabledState.ForeColor = Color.FromArgb(107, 114, 128);
            // 
            // btnLeaveRoom
            // 
            btnLeaveRoom.Enabled = false;
            btnLeaveRoom.Location = new Point(12, 528);
            btnLeaveRoom.Name = "btnLeaveRoom";
            btnLeaveRoom.Size = new Size(250, 36);
            btnLeaveRoom.TabIndex = 18;
            btnLeaveRoom.Text = "Quay về chat chung";
            
            btnLeaveRoom.Click += btnLeaveRoom_Click;
            btnLeaveRoom.BorderRadius = 8;
            btnLeaveRoom.FillColor = Color.FromArgb(239, 68, 68);
            btnLeaveRoom.ForeColor = Color.White;
            btnLeaveRoom.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLeaveRoom.DisabledState.FillColor = Color.FromArgb(209, 213, 219);
            btnLeaveRoom.DisabledState.ForeColor = Color.FromArgb(107, 114, 128);
            // 
            // lblCurrentRoom
            // 
            lblCurrentRoom.AutoSize = true;
            lblCurrentRoom.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCurrentRoom.ForeColor = Color.FromArgb(17, 24, 39);
            lblCurrentRoom.Location = new Point(12, 12);
            lblCurrentRoom.Name = "lblCurrentRoom";
            lblCurrentRoom.Size = new Size(188, 25);
            lblCurrentRoom.TabIndex = 17;
            lblCurrentRoom.Text = "Đang ở: Chat chung";
            // 
            // picMyAvatar
            // 
            picMyAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picMyAvatar.BackColor = Color.White;
            picMyAvatar.BorderStyle = BorderStyle.FixedSingle;
            picMyAvatar.Location = new Point(870, 12);
            picMyAvatar.Name = "picMyAvatar";
            picMyAvatar.Size = new Size(46, 46);
            picMyAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picMyAvatar.TabIndex = 19;
            picMyAvatar.TabStop = false; ;
            // 
            // btnProfile
            // 
            btnProfile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnProfile.Enabled = false;
            btnProfile.Location = new Point(1070, 18);
            btnProfile.Name = "btnProfile";
            btnProfile.Size = new Size(95, 34);
            btnProfile.TabIndex = 20;
            btnProfile.Text = "Hồ sơ";
            btnProfile.Click += btnProfile_Click;
            btnProfile.BorderRadius = 8;
            btnProfile.FillColor = Color.White;
            btnProfile.ForeColor = Color.FromArgb(37, 99, 235);
            btnProfile.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnProfile.DisabledState.FillColor = Color.FromArgb(191, 219, 254);
            btnProfile.DisabledState.ForeColor = Color.FromArgb(37, 99, 235);
            // 
            // lblAccountStatus
            // 
            lblAccountStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAccountStatus.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            lblAccountStatus.ForeColor = Color.FromArgb(219, 234, 254);
            lblAccountStatus.Location = new Point(930, 10);
            lblAccountStatus.Name = "lblAccountStatus";
            lblAccountStatus.Size = new Size(130, 20);
            lblAccountStatus.TabIndex = 22;
            lblAccountStatus.Text = "Đang đăng nhập";
            lblAccountStatus.Visible = false;
            // 
            // lblMyFullName
            // 
            lblMyFullName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblMyFullName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMyFullName.ForeColor = Color.White;
            lblMyFullName.Location = new Point(930, 23);
            lblMyFullName.Name = "lblMyFullName";
            lblMyFullName.Size = new Size(130, 25);
            lblMyFullName.TabIndex = 21;
            lblMyFullName.Text = "";
            // 
            // pnlTopHeader
            // 
            pnlTopHeader.BackColor = Color.FromArgb(37, 99, 235);
            pnlTopHeader.FillColor = Color.FromArgb(37, 99, 235);
            pnlTopHeader.Controls.Add(lblAppTitle);
            pnlTopHeader.Controls.Add(picMyAvatar);
            pnlTopHeader.Controls.Add(lblAccountStatus);
            pnlTopHeader.Controls.Add(lblMyFullName);
            pnlTopHeader.Controls.Add(btnProfile);
            pnlTopHeader.Dock = DockStyle.Top;
            pnlTopHeader.Location = new Point(0, 0);
            pnlTopHeader.Name = "pnlTopHeader";
            pnlTopHeader.Size = new Size(1180, 70);
            pnlTopHeader.TabIndex = 3;
            // 
            // lblAppTitle
            // 
            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.Location = new Point(20, 18);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new Size(257, 37);
            lblAppTitle.TabIndex = 0;
            lblAppTitle.Text = "Realtime Chat App";
            // 
            // pnlLeftSidebar
            // 
            pnlLeftSidebar.BackColor = Color.White;
            pnlLeftSidebar.BorderRadius = 12;
            pnlLeftSidebar.FillColor = Color.White;
            pnlLeftSidebar.ShadowDecoration.Enabled = true;
            pnlLeftSidebar.ShadowDecoration.Depth = 4;
            pnlLeftSidebar.Controls.Add(lblOnlineTitle);
            pnlLeftSidebar.Controls.Add(lbOnlineUsers);
            pnlLeftSidebar.Dock = DockStyle.Left;
            pnlLeftSidebar.Location = new Point(0, 70);
            pnlLeftSidebar.Name = "pnlLeftSidebar";
            pnlLeftSidebar.Padding = new Padding(12);
            pnlLeftSidebar.Size = new Size(230, 650);
            pnlLeftSidebar.TabIndex = 2;
            // 
            // lblOnlineTitle
            // 
            lblOnlineTitle.AutoSize = true;
            lblOnlineTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblOnlineTitle.ForeColor = Color.FromArgb(17, 24, 39);
            lblOnlineTitle.Location = new Point(12, 12);
            lblOnlineTitle.Name = "lblOnlineTitle";
            lblOnlineTitle.Size = new Size(123, 25);
            lblOnlineTitle.TabIndex = 0;
            lblOnlineTitle.Text = "Online Users";
            // 
            // pnlChatCard
            // 
            pnlChatCard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlChatCard.BackColor = Color.Transparent;
            pnlChatCard.BorderRadius = 12;
            pnlChatCard.FillColor = Color.White;
            pnlChatCard.Location = new Point(12, 50);
            pnlChatCard.Name = "pnlChatCard";
            pnlChatCard.Padding = new Padding(10);
            pnlChatCard.ShadowDecoration.Enabled = true;
            pnlChatCard.ShadowDecoration.Depth = 4;
            pnlChatCard.Size = new Size(641, 520);
            pnlChatCard.TabIndex = 22;
            pnlChatCard.Controls.Add(rtbChat);
            // 
            // pnlCenterChat
            // 
            pnlCenterChat.BackColor = Color.FromArgb(244, 246, 248);
            pnlCenterChat.FillColor = Color.FromArgb(244, 246, 248);
            pnlCenterChat.Controls.Add(lblCurrentRoom);
            pnlCenterChat.Controls.Add(pnlChatCard);
            pnlCenterChat.Controls.Add(txtMessage);
            pnlCenterChat.Controls.Add(btnSend);
            pnlCenterChat.Dock = DockStyle.Fill;
            pnlCenterChat.Location = new Point(230, 70);
            pnlCenterChat.Name = "pnlCenterChat";
            pnlCenterChat.Padding = new Padding(12);
            pnlCenterChat.Size = new Size(665, 650);
            pnlCenterChat.TabIndex = 0;
            
            // 
            // pnlRightTools
            // 
            pnlRightTools.BackColor = Color.White;
            pnlRightTools.BorderRadius = 12;
            pnlRightTools.FillColor = Color.White;
            pnlRightTools.ShadowDecoration.Enabled = true;
            pnlRightTools.ShadowDecoration.Depth = 4;
            pnlRightTools.Controls.Add(lblToolsTitle);
            pnlRightTools.Controls.Add(txtUsername);
            pnlRightTools.Controls.Add(labelServerIP);
            pnlRightTools.Controls.Add(txtServerIP);
            pnlRightTools.Controls.Add(labelServerPort);
            pnlRightTools.Controls.Add(txtServerPort);
            pnlRightTools.Controls.Add(btnConnect);
            pnlRightTools.Controls.Add(labelRoomName);
            pnlRightTools.Controls.Add(txtRoomName);
            pnlRightTools.Controls.Add(btnCreateRoom);
            pnlRightTools.Controls.Add(labelRoomId);
            pnlRightTools.Controls.Add(txtRoomId);
            pnlRightTools.Controls.Add(btnJoinRoom);
            pnlRightTools.Controls.Add(btnLeaveRoom);
            pnlRightTools.Controls.Add(btnSendFile);
            pnlRightTools.Dock = DockStyle.Right;
            pnlRightTools.Location = new Point(895, 70);
            pnlRightTools.Name = "pnlRightTools";
            pnlRightTools.Padding = new Padding(12);
            pnlRightTools.Size = new Size(285, 650);
            pnlRightTools.TabIndex = 1;
            // 
            // lblToolsTitle
            // 
            lblToolsTitle.AutoSize = true;
            lblToolsTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblToolsTitle.ForeColor = Color.FromArgb(17, 24, 39);
            lblToolsTitle.Location = new Point(12, 12);
            lblToolsTitle.Name = "lblToolsTitle";
            lblToolsTitle.Size = new Size(58, 25);
            lblToolsTitle.TabIndex = 0;
            lblToolsTitle.Text = "Tools";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 248);
            ClientSize = new Size(1180, 720);
            Controls.Add(pnlCenterChat);
            Controls.Add(pnlRightTools);
            Controls.Add(pnlLeftSidebar);
            Controls.Add(pnlTopHeader);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1000, 650);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Realtime Chat App";
            ((System.ComponentModel.ISupportInitialize)picMyAvatar).EndInit();
            pnlTopHeader.ResumeLayout(false);
            pnlTopHeader.PerformLayout();
            pnlLeftSidebar.ResumeLayout(false);
            pnlLeftSidebar.PerformLayout();
            pnlCenterChat.ResumeLayout(false);
            pnlCenterChat.PerformLayout();
            pnlRightTools.ResumeLayout(false);
            pnlRightTools.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtUsername;

        private System.Windows.Forms.RichTextBox rtbChat;
        private Guna.UI2.WinForms.Guna2TextBox txtMessage;
        
        private ListBox lbOnlineUsers;
        private Guna.UI2.WinForms.Guna2Button btnConnect;
        private Guna.UI2.WinForms.Guna2Button btnSend;
        private Guna.UI2.WinForms.Guna2Button btnLeaveRoom;
        private Guna.UI2.WinForms.Guna2Button btnSendFile;
        private Guna.UI2.WinForms.Guna2Button btnCreateRoom;
        private Guna.UI2.WinForms.Guna2Button btnJoinRoom;
        private Guna.UI2.WinForms.Guna2Button btnProfile;

        private Label labelServerIP;
        private Guna.UI2.WinForms.Guna2TextBox txtServerIP;
        private Label labelServerPort;
        private Guna.UI2.WinForms.Guna2TextBox txtServerPort;
        private Label labelRoomName;
        private Guna.UI2.WinForms.Guna2TextBox txtRoomName;
        
        private Label labelRoomId;
        private Guna.UI2.WinForms.Guna2TextBox txtRoomId;
        
        private Label lblCurrentRoom;
        private PictureBox picMyAvatar;
        
        private Label lblMyFullName;
        private Guna2Panel pnlTopHeader;
        private Guna2Panel pnlLeftSidebar;
        private Guna2Panel pnlCenterChat;
        private Guna2Panel pnlRightTools;
        private Guna2Panel pnlChatCard;
        private Label lblAppTitle;
        private Label lblOnlineTitle;
        private Label lblToolsTitle;
        private Label lblConnectionTitle;
        private Label lblRoomTitle;
        private Label lblFileTitle;
        private Label lblAccountStatus;
    }
}