using Guna.UI2.WinForms;

using Guna.UI2.WinForms;

namespace RealtimeChatClient
{
    partial class LoginForm
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
            pnlLoginCard = new Guna2Panel();
            lblAppTitle = new Label();
            lblSubtitle = new Label();
            label1 = new Label();
            txtUsername = new Guna2TextBox();
            label2 = new Label();
            txtPassword = new Guna2TextBox();
            btnLogin = new Guna2Button();
            btnGoToRegister = new Guna2Button();
            btnForgotPassword = new Guna2Button();

            pnlLoginCard.SuspendLayout();
            SuspendLayout();

            // 
            // pnlLoginCard
            // 
            pnlLoginCard.BackColor = Color.Transparent;
            pnlLoginCard.BorderRadius = 18;
            pnlLoginCard.FillColor = Color.White;
            pnlLoginCard.Location = new Point(95, 45);
            pnlLoginCard.Name = "pnlLoginCard";
            pnlLoginCard.ShadowDecoration.Enabled = true;
            pnlLoginCard.ShadowDecoration.Depth = 8;
            pnlLoginCard.Size = new Size(430, 430);
            pnlLoginCard.TabIndex = 0;

            // 
            // lblAppTitle
            // 
            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblAppTitle.Location = new Point(80, 35);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new Size(272, 41);
            lblAppTitle.TabIndex = 1;
            lblAppTitle.Text = "Realtime Chat App";

            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(107, 114, 128);
            lblSubtitle.Location = new Point(122, 80);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(185, 23);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Đăng nhập tài khoản";

            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(75, 85, 99);
            label1.Location = new Point(55, 130);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 3;
            label1.Text = "Tài khoản";

            // 
            // txtUsername
            // 
            txtUsername.BorderColor = Color.FromArgb(209, 213, 219);
            txtUsername.BorderRadius = 10;
            txtUsername.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtUsername.Font = new Font("Segoe UI", 10F);
            txtUsername.Location = new Point(55, 155);
            txtUsername.Margin = new Padding(3, 4, 3, 4);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Nhập tài khoản";
            txtUsername.Size = new Size(320, 38);
            txtUsername.TabIndex = 0;

            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(75, 85, 99);
            label2.Location = new Point(55, 210);
            label2.Name = "label2";
            label2.Size = new Size(75, 20);
            label2.TabIndex = 4;
            label2.Text = "Mật khẩu";

            // 
            // txtPassword
            // 
            txtPassword.BorderColor = Color.FromArgb(209, 213, 219);
            txtPassword.BorderRadius = 10;
            txtPassword.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.Location = new Point(55, 235);
            txtPassword.Margin = new Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.PlaceholderText = "Nhập mật khẩu";
            txtPassword.Size = new Size(320, 38);
            txtPassword.TabIndex = 1;

            // 
            // btnLogin
            // 
            btnLogin.BorderRadius = 10;
            btnLogin.FillColor = Color.FromArgb(37, 99, 235);
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(55, 300);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(320, 40);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Đăng nhập";
            btnLogin.Click += btnLogin_Click;

            // 
            // btnGoToRegister
            // 
            btnGoToRegister.BorderRadius = 8;
            btnGoToRegister.FillColor = Color.FromArgb(239, 246, 255);
            btnGoToRegister.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGoToRegister.ForeColor = Color.FromArgb(37, 99, 235);
            btnGoToRegister.Location = new Point(55, 360);
            btnGoToRegister.Name = "btnGoToRegister";
            btnGoToRegister.Size = new Size(145, 34);
            btnGoToRegister.TabIndex = 3;
            btnGoToRegister.Text = "Đăng ký";
            btnGoToRegister.Click += btnGoToRegister_Click;

            // 
            // btnForgotPassword
            // 
            btnForgotPassword.BorderRadius = 8;
            btnForgotPassword.FillColor = Color.FromArgb(239, 246, 255);
            btnForgotPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnForgotPassword.ForeColor = Color.FromArgb(37, 99, 235);
            btnForgotPassword.Location = new Point(215, 360);
            btnForgotPassword.Name = "btnForgotPassword";
            btnForgotPassword.Size = new Size(160, 34);
            btnForgotPassword.TabIndex = 4;
            btnForgotPassword.Text = "Quên mật khẩu?";
            btnForgotPassword.Click += btnForgotPassword_Click;

            // 
            // add controls to card
            // 
            pnlLoginCard.Controls.Add(lblAppTitle);
            pnlLoginCard.Controls.Add(lblSubtitle);
            pnlLoginCard.Controls.Add(label1);
            pnlLoginCard.Controls.Add(txtUsername);
            pnlLoginCard.Controls.Add(label2);
            pnlLoginCard.Controls.Add(txtPassword);
            pnlLoginCard.Controls.Add(btnLogin);
            pnlLoginCard.Controls.Add(btnGoToRegister);
            pnlLoginCard.Controls.Add(btnForgotPassword);

            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 248);
            ClientSize = new Size(620, 530);
            Controls.Add(pnlLoginCard);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập hệ thống";

            pnlLoginCard.ResumeLayout(false);
            pnlLoginCard.PerformLayout();
            ResumeLayout(false);
        }

        private Guna2Panel pnlLoginCard;
        private Label lblAppTitle;
        private Label lblSubtitle;
        private Guna2TextBox txtUsername;
        private Guna2TextBox txtPassword;
        private Guna2Button btnLogin;
        private Label label1;
        private Label label2;
        private Guna2Button btnGoToRegister;
        private Guna2Button btnForgotPassword;
    }
}