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
            txtUsername = new TextBox();
            btnConnect = new Button();
            rtbChat = new RichTextBox();
            txtMessage = new TextBox();
            btnSend = new Button();
            lbOnlineUsers = new ListBox();
            btnSendFile = new Button();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(12, 15);
            txtUsername.Margin = new Padding(3, 4, 3, 4);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(200, 27);
            txtUsername.TabIndex = 0;
            txtUsername.Text = "SinhVien_UIT";
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(218, 14);
            btnConnect.Margin = new Padding(3, 4, 3, 4);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(90, 31);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Kết nối";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // rtbChat
            // 
            rtbChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbChat.Location = new Point(12, 56);
            rtbChat.Margin = new Padding(3, 4, 3, 4);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.Size = new Size(460, 374);
            rtbChat.TabIndex = 2;
            rtbChat.Text = "";
            // 
            // txtMessage
            // 
            txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtMessage.Location = new Point(12, 444);
            txtMessage.Margin = new Padding(3, 4, 3, 4);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(280, 27);
            txtMessage.TabIndex = 3;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSend.Enabled = false;
            btnSend.Location = new Point(393, 441);
            btnSend.Margin = new Padding(3, 4, 3, 4);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(79, 31);
            btnSend.TabIndex = 4;
            btnSend.Text = "Gửi";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // lbOnlineUsers
            // 
            lbOnlineUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lbOnlineUsers.FormattingEnabled = true;
            lbOnlineUsers.Location = new Point(281, 56);
            lbOnlineUsers.Name = "lbOnlineUsers";
            lbOnlineUsers.Size = new Size(191, 364);
            lbOnlineUsers.TabIndex = 5;
            // 
            // btnSendFile
            // 
            btnSendFile.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSendFile.Enabled = false;
            btnSendFile.Location = new Point(309, 441);
            btnSendFile.Margin = new Padding(3, 4, 3, 4);
            btnSendFile.Name = "btnSendFile";
            btnSendFile.Size = new Size(78, 31);
            btnSendFile.TabIndex = 6;
            btnSendFile.Text = "Gửi file";
            btnSendFile.UseVisualStyleBackColor = true;
            btnSendFile.Click += btnSendFile_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 489);
            Controls.Add(btnSendFile);
            Controls.Add(lbOnlineUsers);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Controls.Add(rtbChat);
            Controls.Add(btnConnect);
            Controls.Add(txtUsername);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(300, 301);
            Name = "Form1";
            Text = "Chat Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private ListBox lbOnlineUsers;
        private Button btnSendFile;
    }
}