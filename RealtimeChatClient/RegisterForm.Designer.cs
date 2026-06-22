using Guna.UI2.WinForms;

namespace RealtimeChatClient
{
    partial class RegisterForm
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

        private void InitializeComponent()
        {
            pnlRegisterCard = new Guna2Panel();
            lblAppTitle = new Label();
            lblSubtitle = new Label();

            label1 = new Label();
            txtUser = new Guna2TextBox();

            label2 = new Label();
            txtPass = new Guna2TextBox();

            label3 = new Label();
            txtFullName = new Guna2TextBox();

            label4 = new Label();
            txtEmail = new Guna2TextBox();
            btnSendOtp = new Guna2Button();

            label5 = new Label();
            txtOtp = new Guna2TextBox();

            btnSubmit = new Guna2Button();

            pnlRegisterCard.SuspendLayout();
            SuspendLayout();

            // 
            // pnlRegisterCard
            // 
            pnlRegisterCard.BackColor = Color.Transparent;
            pnlRegisterCard.BorderRadius = 18;
            pnlRegisterCard.FillColor = Color.White;
            pnlRegisterCard.Location = new Point(85, 35);
            pnlRegisterCard.Name = "pnlRegisterCard";
            pnlRegisterCard.ShadowDecoration.Enabled = true;
            pnlRegisterCard.ShadowDecoration.Depth = 8;
            pnlRegisterCard.Size = new Size(500, 580);
            pnlRegisterCard.TabIndex = 0;

            // 
            // lblAppTitle
            // 
            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblAppTitle.Location = new Point(115, 28);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new Size(272, 41);
            lblAppTitle.TabIndex = 20;
            lblAppTitle.Text = "Realtime Chat App";

            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(107, 114, 128);
            lblSubtitle.Location = new Point(160, 72);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(178, 23);
            lblSubtitle.TabIndex = 21;
            lblSubtitle.Text = "Đăng ký tài khoản mới";

            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(75, 85, 99);
            label1.Location = new Point(55, 120);
            label1.Name = "label1";
            label1.Size = new Size(113, 20);
            label1.TabIndex = 0;
            label1.Text = "Tên đăng nhập";

            // 
            // txtUser
            // 
            txtUser.BorderColor = Color.FromArgb(209, 213, 219);
            txtUser.BorderRadius = 10;
            txtUser.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtUser.Font = new Font("Segoe UI", 10F);
            txtUser.Location = new Point(55, 145);
            txtUser.Margin = new Padding(3, 4, 3, 4);
            txtUser.Name = "txtUser";
            txtUser.PlaceholderText = "Nhập tên đăng nhập";
            txtUser.Size = new Size(390, 38);
            txtUser.TabIndex = 1;

            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(75, 85, 99);
            label2.Location = new Point(55, 195);
            label2.Name = "label2";
            label2.Size = new Size(75, 20);
            label2.TabIndex = 2;
            label2.Text = "Mật khẩu";

            // 
            // txtPass
            // 
            txtPass.BorderColor = Color.FromArgb(209, 213, 219);
            txtPass.BorderRadius = 10;
            txtPass.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtPass.Font = new Font("Segoe UI", 10F);
            txtPass.Location = new Point(55, 220);
            txtPass.Margin = new Padding(3, 4, 3, 4);
            txtPass.Name = "txtPass";
            txtPass.PasswordChar = '*';
            txtPass.PlaceholderText = "Nhập mật khẩu";
            txtPass.Size = new Size(390, 38);
            txtPass.TabIndex = 3;

            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(75, 85, 99);
            label3.Location = new Point(55, 270);
            label3.Name = "label3";
            label3.Size = new Size(76, 20);
            label3.TabIndex = 4;
            label3.Text = "Họ và tên";

            // 
            // txtFullName
            // 
            txtFullName.BorderColor = Color.FromArgb(209, 213, 219);
            txtFullName.BorderRadius = 10;
            txtFullName.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtFullName.Font = new Font("Segoe UI", 10F);
            txtFullName.Location = new Point(55, 295);
            txtFullName.Margin = new Padding(3, 4, 3, 4);
            txtFullName.Name = "txtFullName";
            txtFullName.PlaceholderText = "Nhập họ và tên";
            txtFullName.Size = new Size(390, 38);
            txtFullName.TabIndex = 5;

            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(75, 85, 99);
            label4.Location = new Point(55, 345);
            label4.Name = "label4";
            label4.Size = new Size(46, 20);
            label4.TabIndex = 6;
            label4.Text = "Email";

            // 
            // txtEmail
            // 
            txtEmail.BorderColor = Color.FromArgb(209, 213, 219);
            txtEmail.BorderRadius = 10;
            txtEmail.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.Location = new Point(55, 370);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Nhập email";
            txtEmail.Size = new Size(270, 38);
            txtEmail.TabIndex = 7;

            // 
            // btnSendOtp
            // 
            btnSendOtp.BorderRadius = 10;
            btnSendOtp.FillColor = Color.FromArgb(37, 99, 235);
            btnSendOtp.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSendOtp.ForeColor = Color.White;
            btnSendOtp.Location = new Point(335, 370);
            btnSendOtp.Name = "btnSendOtp";
            btnSendOtp.Size = new Size(110, 38);
            btnSendOtp.TabIndex = 8;
            btnSendOtp.Text = "Gửi mã";
            btnSendOtp.Click += btnSendOtp_Click;

            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.ForeColor = Color.FromArgb(75, 85, 99);
            label5.Location = new Point(55, 420);
            label5.Name = "label5";
            label5.Size = new Size(94, 20);
            label5.TabIndex = 9;
            label5.Text = "Mã xác nhận";

            // 
            // txtOtp
            // 
            txtOtp.BorderColor = Color.FromArgb(209, 213, 219);
            txtOtp.BorderRadius = 10;
            txtOtp.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtOtp.Font = new Font("Segoe UI", 10F);
            txtOtp.Location = new Point(55, 445);
            txtOtp.Margin = new Padding(3, 4, 3, 4);
            txtOtp.Name = "txtOtp";
            txtOtp.PlaceholderText = "Nhập mã OTP";
            txtOtp.Size = new Size(390, 38);
            txtOtp.TabIndex = 10;

            // 
            // btnSubmit
            // 
            btnSubmit.BorderRadius = 10;
            btnSubmit.FillColor = Color.FromArgb(37, 99, 235);
            btnSubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(55, 510);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(390, 42);
            btnSubmit.TabIndex = 11;
            btnSubmit.Text = "Xác nhận đăng ký";
            btnSubmit.Click += btnSubmit_Click;

            // 
            // Add controls to card
            // 
            pnlRegisterCard.Controls.Add(lblAppTitle);
            pnlRegisterCard.Controls.Add(lblSubtitle);
            pnlRegisterCard.Controls.Add(label1);
            pnlRegisterCard.Controls.Add(txtUser);
            pnlRegisterCard.Controls.Add(label2);
            pnlRegisterCard.Controls.Add(txtPass);
            pnlRegisterCard.Controls.Add(label3);
            pnlRegisterCard.Controls.Add(txtFullName);
            pnlRegisterCard.Controls.Add(label4);
            pnlRegisterCard.Controls.Add(txtEmail);
            pnlRegisterCard.Controls.Add(btnSendOtp);
            pnlRegisterCard.Controls.Add(label5);
            pnlRegisterCard.Controls.Add(txtOtp);
            pnlRegisterCard.Controls.Add(btnSubmit);

            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 248);
            ClientSize = new Size(670, 660);
            Controls.Add(pnlRegisterCard);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đăng ký thành viên";

            pnlRegisterCard.ResumeLayout(false);
            pnlRegisterCard.PerformLayout();
            ResumeLayout(false);
        }

        private Guna2Panel pnlRegisterCard;
        private Label lblAppTitle;
        private Label lblSubtitle;

        private Guna2TextBox txtUser;
        private Guna2TextBox txtPass;
        private Guna2TextBox txtFullName;
        private Guna2TextBox txtEmail;
        private Guna2TextBox txtOtp;

        private Guna2Button btnSendOtp;
        private Guna2Button btnSubmit;

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
    }
}