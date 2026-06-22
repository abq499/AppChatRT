namespace ChatLoadBalancerGUI
{
    partial class LoadBalancerForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblPort;
        private TextBox txtLbPort;
        private Button btnStart;
        private Button btnStop;
        private Label lblLbStatusTitle;
        private Label lblLbStatus;
        private Label lblServerTitle;
        private Label lblServer8888;
        private Label lblServer8889;
        private Label lblRouteCount;
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
            txtLbPort = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            lblLbStatusTitle = new Label();
            lblLbStatus = new Label();
            lblServerTitle = new Label();
            lblServer8888 = new Label();
            lblServer8889 = new Label();
            lblRouteCount = new Label();
            rtbLog = new RichTextBox();

            SuspendLayout();

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(147, 51, 234);
            lblTitle.Location = new Point(24, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(358, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Load Balancer Dashboard";

            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPort.Location = new Point(28, 85);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(156, 23);
            lblPort.TabIndex = 1;
            lblPort.Text = "Load Balancer Port";

            // 
            // txtLbPort
            // 
            txtLbPort.Location = new Point(28, 115);
            txtLbPort.Name = "txtLbPort";
            txtLbPort.Size = new Size(150, 27);
            txtLbPort.TabIndex = 2;
            txtLbPort.Text = "8800";

            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(34, 197, 94);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(205, 110);
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
            btnStop.Location = new Point(340, 110);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(120, 38);
            btnStop.TabIndex = 4;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;

            // 
            // lblLbStatusTitle
            // 
            lblLbStatusTitle.AutoSize = true;
            lblLbStatusTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLbStatusTitle.Location = new Point(500, 85);
            lblLbStatusTitle.Name = "lblLbStatusTitle";
            lblLbStatusTitle.Size = new Size(86, 23);
            lblLbStatusTitle.TabIndex = 5;
            lblLbStatusTitle.Text = "Trạng thái";

            // 
            // lblLbStatus
            // 
            lblLbStatus.AutoSize = true;
            lblLbStatus.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblLbStatus.ForeColor = Color.Red;
            lblLbStatus.Location = new Point(500, 115);
            lblLbStatus.Name = "lblLbStatus";
            lblLbStatus.Size = new Size(85, 25);
            lblLbStatus.TabIndex = 6;
            lblLbStatus.Text = "Stopped";

            // 
            // lblServerTitle
            // 
            lblServerTitle.AutoSize = true;
            lblServerTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblServerTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblServerTitle.Location = new Point(28, 175);
            lblServerTitle.Name = "lblServerTitle";
            lblServerTitle.Size = new Size(173, 28);
            lblServerTitle.TabIndex = 7;
            lblServerTitle.Text = "Chat Server Nodes";

            // 
            // lblServer8888
            // 
            lblServer8888.AutoSize = true;
            lblServer8888.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblServer8888.ForeColor = Color.Red;
            lblServer8888.Location = new Point(32, 215);
            lblServer8888.Name = "lblServer8888";
            lblServer8888.Size = new Size(174, 25);
            lblServer8888.TabIndex = 8;
            lblServer8888.Text = "Server 8888: Down";

            // 
            // lblServer8889
            // 
            lblServer8889.AutoSize = true;
            lblServer8889.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblServer8889.ForeColor = Color.Red;
            lblServer8889.Location = new Point(300, 215);
            lblServer8889.Name = "lblServer8889";
            lblServer8889.Size = new Size(174, 25);
            lblServer8889.TabIndex = 9;
            lblServer8889.Text = "Server 8889: Down";

            // 
            // lblRouteCount
            // 
            lblRouteCount.AutoSize = true;
            lblRouteCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRouteCount.Location = new Point(32, 260);
            lblRouteCount.Name = "lblRouteCount";
            lblRouteCount.Size = new Size(144, 23);
            lblRouteCount.TabIndex = 10;
            lblRouteCount.Text = "Client đã route: 0";

            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.FromArgb(17, 24, 39);
            rtbLog.BorderStyle = BorderStyle.None;
            rtbLog.Font = new Font("Consolas", 10F);
            rtbLog.ForeColor = Color.White;
            rtbLog.Location = new Point(28, 305);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(680, 320);
            rtbLog.TabIndex = 11;
            rtbLog.Text = "";

            // 
            // LoadBalancerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 248);
            ClientSize = new Size(740, 655);
            Controls.Add(rtbLog);
            Controls.Add(lblRouteCount);
            Controls.Add(lblServer8889);
            Controls.Add(lblServer8888);
            Controls.Add(lblServerTitle);
            Controls.Add(lblLbStatus);
            Controls.Add(lblLbStatusTitle);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(txtLbPort);
            Controls.Add(lblPort);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "LoadBalancerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Load Balancer Dashboard";
            FormClosing += LoadBalancerForm_FormClosing;

            ResumeLayout(false);
            PerformLayout();
        }
    }
}