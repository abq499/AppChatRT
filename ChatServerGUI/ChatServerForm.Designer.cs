namespace ChatServerGUI
{
    partial class ChatServerForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblPort;
        private TextBox txtServerPort;
        private Button btnStart;
        private Button btnStop;
        private Label lblStatusTitle;
        private Label lblServerStatus;
        private Label lblCurrentPort;
        private Label lblClientCount;
        private Label lblLocalUsersTitle;
        private ListBox lbLocalUsers;
        private RichTextBox rtbLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblPort = new Label();
            txtServerPort = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            lblStatusTitle = new Label();
            lblServerStatus = new Label();
            lblCurrentPort = new Label();
            lblClientCount = new Label();
            lblLocalUsersTitle = new Label();
            lbLocalUsers = new ListBox();
            rtbLog = new RichTextBox();

            SuspendLayout();

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblTitle.Location = new Point(28, 22);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(326, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Chat Server Dashboard";

            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPort.Location = new Point(32, 88);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(95, 23);
            lblPort.TabIndex = 1;
            lblPort.Text = "Server Port";

            // 
            // txtServerPort
            // 
            txtServerPort.Location = new Point(32, 118);
            txtServerPort.Name = "txtServerPort";
            txtServerPort.Size = new Size(150, 27);
            txtServerPort.TabIndex = 2;
            txtServerPort.Text = "8888";

            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(34, 197, 94);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(210, 112);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(120, 38);
            btnStart.TabIndex = 3;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;

            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(239, 68, 68);
            btnStop.Enabled = false;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(348, 112);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(120, 38);
            btnStop.TabIndex = 4;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;

            // 
            // lblStatusTitle
            // 
            lblStatusTitle.AutoSize = true;
            lblStatusTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatusTitle.Location = new Point(505, 88);
            lblStatusTitle.Name = "lblStatusTitle";
            lblStatusTitle.Size = new Size(86, 23);
            lblStatusTitle.TabIndex = 5;
            lblStatusTitle.Text = "Trạng thái";

            // 
            // lblServerStatus
            // 
            lblServerStatus.AutoSize = true;
            lblServerStatus.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblServerStatus.ForeColor = Color.Red;
            lblServerStatus.Location = new Point(505, 118);
            lblServerStatus.Name = "lblServerStatus";
            lblServerStatus.Size = new Size(85, 25);
            lblServerStatus.TabIndex = 6;
            lblServerStatus.Text = "Stopped";

            // 
            // lblCurrentPort
            // 
            lblCurrentPort.AutoSize = true;
            lblCurrentPort.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCurrentPort.ForeColor = Color.FromArgb(37, 99, 235);
            lblCurrentPort.Location = new Point(32, 175);
            lblCurrentPort.Name = "lblCurrentPort";
            lblCurrentPort.Size = new Size(93, 23);
            lblCurrentPort.TabIndex = 7;
            lblCurrentPort.Text = "Port: None";

            // 
            // lblClientCount
            // 
            lblClientCount.AutoSize = true;
            lblClientCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblClientCount.Location = new Point(210, 175);
            lblClientCount.Name = "lblClientCount";
            lblClientCount.Size = new Size(123, 23);
            lblClientCount.TabIndex = 8;
            lblClientCount.Text = "Local clients: 0";

            // 
            // lblLocalUsersTitle
            // 
            lblLocalUsersTitle.AutoSize = true;
            lblLocalUsersTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblLocalUsersTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblLocalUsersTitle.Location = new Point(32, 225);
            lblLocalUsersTitle.Name = "lblLocalUsersTitle";
            lblLocalUsersTitle.Size = new Size(174, 28);
            lblLocalUsersTitle.TabIndex = 9;
            lblLocalUsersTitle.Text = "Local Online Users";

            // 
            // lbLocalUsers
            // 
            lbLocalUsers.Font = new Font("Segoe UI", 10F);
            lbLocalUsers.FormattingEnabled = true;
            lbLocalUsers.ItemHeight = 23;
            lbLocalUsers.Location = new Point(32, 265);
            lbLocalUsers.Name = "lbLocalUsers";
            lbLocalUsers.Size = new Size(210, 326);
            lbLocalUsers.TabIndex = 10;

            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.FromArgb(17, 24, 39);
            rtbLog.BorderStyle = BorderStyle.None;
            rtbLog.Font = new Font("Consolas", 10F);
            rtbLog.ForeColor = Color.White;
            rtbLog.Location = new Point(270, 225);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(520, 365);
            rtbLog.TabIndex = 11;
            rtbLog.Text = "";

            // 
            // ChatServerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 248);
            ClientSize = new Size(825, 625);
            Controls.Add(rtbLog);
            Controls.Add(lbLocalUsers);
            Controls.Add(lblLocalUsersTitle);
            Controls.Add(lblClientCount);
            Controls.Add(lblCurrentPort);
            Controls.Add(lblServerStatus);
            Controls.Add(lblStatusTitle);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(txtServerPort);
            Controls.Add(lblPort);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ChatServerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chat Server Dashboard";
            FormClosing += ChatServerForm_FormClosing;

            ResumeLayout(false);
            PerformLayout();
        }
    }
}