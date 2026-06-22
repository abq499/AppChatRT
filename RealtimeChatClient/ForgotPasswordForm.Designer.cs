using Guna.UI2.WinForms;

namespace RealtimeChatClient
{
    partial class ForgotPasswordForm
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2Panel pnlForgotCard;
        private Label lblTitle;
        private Label lblGuide;
        private Label lblUsernameOrEmail;
        private Guna2TextBox txtUsernameOrEmail;
        private Guna2Button btnSendOtp;
        private Label lblOtp;
        private Guna2TextBox txtOtp;
        private Label lblNewPassword;
        private Guna2TextBox txtNewPassword;
        private Label lblConfirmPassword;
        private Guna2TextBox txtConfirmPassword;
        private Guna2Button btnResetPassword;
        private Guna2Button btnCancel;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlForgotCard = new Guna2Panel();
            lblTitle = new Label();
            lblGuide = new Label();
            lblUsernameOrEmail = new Label();
            txtUsernameOrEmail = new Guna2TextBox();
            btnSendOtp = new Guna2Button();
            lblOtp = new Label();
            txtOtp = new Guna2TextBox();
            lblNewPassword = new Label();
            txtNewPassword = new Guna2TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new Guna2TextBox();
            btnResetPassword = new Guna2Button();
            btnCancel = new Guna2Button();
            pnlForgotCard.SuspendLayout();
            SuspendLayout();
            // 
            // pnlForgotCard
            // 
            pnlForgotCard.BackColor = Color.Transparent;
            pnlForgotCard.BorderRadius = 18;
            pnlForgotCard.Controls.Add(lblTitle);
            pnlForgotCard.Controls.Add(lblGuide);
            pnlForgotCard.Controls.Add(lblUsernameOrEmail);
            pnlForgotCard.Controls.Add(txtUsernameOrEmail);
            pnlForgotCard.Controls.Add(btnSendOtp);
            pnlForgotCard.Controls.Add(lblOtp);
            pnlForgotCard.Controls.Add(txtOtp);
            pnlForgotCard.Controls.Add(lblNewPassword);
            pnlForgotCard.Controls.Add(txtNewPassword);
            pnlForgotCard.Controls.Add(lblConfirmPassword);
            pnlForgotCard.Controls.Add(txtConfirmPassword);
            pnlForgotCard.Controls.Add(btnResetPassword);
            pnlForgotCard.Controls.Add(btnCancel);
            pnlForgotCard.CustomizableEdges = customizableEdges15;
            pnlForgotCard.FillColor = Color.White;
            pnlForgotCard.Location = new Point(80, 35);
            pnlForgotCard.Name = "pnlForgotCard";
            pnlForgotCard.ShadowDecoration.CustomizableEdges = customizableEdges16;
            pnlForgotCard.ShadowDecoration.Depth = 8;
            pnlForgotCard.ShadowDecoration.Enabled = true;
            pnlForgotCard.Size = new Size(473, 550);
            pnlForgotCard.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblTitle.Location = new Point(105, 30);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(233, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Quên mật khẩu";
            // 
            // lblGuide
            // 
            lblGuide.Font = new Font("Segoe UI", 10F);
            lblGuide.ForeColor = Color.FromArgb(107, 114, 128);
            lblGuide.Location = new Point(45, 78);
            lblGuide.Name = "lblGuide";
            lblGuide.Size = new Size(370, 45);
            lblGuide.TabIndex = 1;
            lblGuide.Text = "Nhập username hoặc email để nhận mã OTP đặt lại mật khẩu.";
            lblGuide.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUsernameOrEmail
            // 
            lblUsernameOrEmail.AutoSize = true;
            lblUsernameOrEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUsernameOrEmail.ForeColor = Color.FromArgb(75, 85, 99);
            lblUsernameOrEmail.Location = new Point(55, 145);
            lblUsernameOrEmail.Name = "lblUsernameOrEmail";
            lblUsernameOrEmail.Size = new Size(125, 20);
            lblUsernameOrEmail.TabIndex = 2;
            lblUsernameOrEmail.Text = "Username/Email";
            // 
            // txtUsernameOrEmail
            // 
            txtUsernameOrEmail.BorderColor = Color.FromArgb(209, 213, 219);
            txtUsernameOrEmail.BorderRadius = 10;
            txtUsernameOrEmail.CustomizableEdges = customizableEdges1;
            txtUsernameOrEmail.DefaultText = "";
            txtUsernameOrEmail.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtUsernameOrEmail.Font = new Font("Segoe UI", 10F);
            txtUsernameOrEmail.Location = new Point(55, 170);
            txtUsernameOrEmail.Margin = new Padding(3, 4, 3, 4);
            txtUsernameOrEmail.Name = "txtUsernameOrEmail";
            txtUsernameOrEmail.PlaceholderText = "Nhập username hoặc email";
            txtUsernameOrEmail.SelectedText = "";
            txtUsernameOrEmail.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtUsernameOrEmail.Size = new Size(350, 38);
            txtUsernameOrEmail.TabIndex = 3;
            // 
            // btnSendOtp
            // 
            btnSendOtp.BorderRadius = 10;
            btnSendOtp.CustomizableEdges = customizableEdges3;
            btnSendOtp.FillColor = Color.FromArgb(37, 99, 235);
            btnSendOtp.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSendOtp.ForeColor = Color.White;
            btnSendOtp.Location = new Point(55, 220);
            btnSendOtp.Name = "btnSendOtp";
            btnSendOtp.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnSendOtp.Size = new Size(350, 40);
            btnSendOtp.TabIndex = 4;
            btnSendOtp.Text = "Gửi mã OTP";
            btnSendOtp.Click += btnSendOtp_Click;
            // 
            // lblOtp
            // 
            lblOtp.AutoSize = true;
            lblOtp.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblOtp.ForeColor = Color.FromArgb(75, 85, 99);
            lblOtp.Location = new Point(55, 280);
            lblOtp.Name = "lblOtp";
            lblOtp.Size = new Size(63, 20);
            lblOtp.TabIndex = 5;
            lblOtp.Text = "Mã OTP";
            // 
            // txtOtp
            // 
            txtOtp.BorderColor = Color.FromArgb(209, 213, 219);
            txtOtp.BorderRadius = 10;
            txtOtp.CustomizableEdges = customizableEdges5;
            txtOtp.DefaultText = "";
            txtOtp.Enabled = false;
            txtOtp.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtOtp.Font = new Font("Segoe UI", 10F);
            txtOtp.Location = new Point(55, 305);
            txtOtp.Margin = new Padding(3, 4, 3, 4);
            txtOtp.Name = "txtOtp";
            txtOtp.PlaceholderText = "Nhập mã OTP";
            txtOtp.SelectedText = "";
            txtOtp.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtOtp.Size = new Size(350, 38);
            txtOtp.TabIndex = 6;
            // 
            // lblNewPassword
            // 
            lblNewPassword.AutoSize = true;
            lblNewPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNewPassword.ForeColor = Color.FromArgb(75, 85, 99);
            lblNewPassword.Location = new Point(55, 355);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(106, 20);
            lblNewPassword.TabIndex = 7;
            lblNewPassword.Text = "Mật khẩu mới";
            // 
            // txtNewPassword
            // 
            txtNewPassword.BorderColor = Color.FromArgb(209, 213, 219);
            txtNewPassword.BorderRadius = 10;
            txtNewPassword.CustomizableEdges = customizableEdges7;
            txtNewPassword.DefaultText = "";
            txtNewPassword.Enabled = false;
            txtNewPassword.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtNewPassword.Font = new Font("Segoe UI", 10F);
            txtNewPassword.Location = new Point(55, 380);
            txtNewPassword.Margin = new Padding(3, 4, 3, 4);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PasswordChar = '*';
            txtNewPassword.PlaceholderText = "Nhập mật khẩu mới";
            txtNewPassword.SelectedText = "";
            txtNewPassword.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtNewPassword.Size = new Size(350, 38);
            txtNewPassword.TabIndex = 8;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblConfirmPassword.ForeColor = Color.FromArgb(75, 85, 99);
            lblConfirmPassword.Location = new Point(55, 430);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(143, 20);
            lblConfirmPassword.TabIndex = 9;
            lblConfirmPassword.Text = "Xác nhận mật khẩu";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.BorderColor = Color.FromArgb(209, 213, 219);
            txtConfirmPassword.BorderRadius = 10;
            txtConfirmPassword.CustomizableEdges = customizableEdges9;
            txtConfirmPassword.DefaultText = "";
            txtConfirmPassword.Enabled = false;
            txtConfirmPassword.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtConfirmPassword.Font = new Font("Segoe UI", 10F);
            txtConfirmPassword.Location = new Point(55, 455);
            txtConfirmPassword.Margin = new Padding(3, 4, 3, 4);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '*';
            txtConfirmPassword.PlaceholderText = "Nhập lại mật khẩu mới";
            txtConfirmPassword.SelectedText = "";
            txtConfirmPassword.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtConfirmPassword.Size = new Size(350, 38);
            txtConfirmPassword.TabIndex = 10;
            // 
            // btnResetPassword
            // 
            btnResetPassword.BorderRadius = 10;
            btnResetPassword.CustomizableEdges = customizableEdges11;
            btnResetPassword.DisabledState.FillColor = Color.FromArgb(209, 213, 219);
            btnResetPassword.DisabledState.ForeColor = Color.FromArgb(107, 114, 128);
            btnResetPassword.Enabled = false;
            btnResetPassword.FillColor = Color.FromArgb(37, 99, 235);
            btnResetPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnResetPassword.ForeColor = Color.White;
            btnResetPassword.Location = new Point(55, 510);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnResetPassword.Size = new Size(215, 40);
            btnResetPassword.TabIndex = 11;
            btnResetPassword.Text = "Đặt lại mật khẩu";
            btnResetPassword.Click += btnResetPassword_Click;
            // 
            // btnCancel
            // 
            btnCancel.BorderRadius = 10;
            btnCancel.CustomizableEdges = customizableEdges13;
            btnCancel.FillColor = Color.FromArgb(239, 246, 255);
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.FromArgb(37, 99, 235);
            btnCancel.Location = new Point(285, 510);
            btnCancel.Name = "btnCancel";
            btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnCancel.Size = new Size(120, 40);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Hủy";
            btnCancel.Click += btnCancel_Click;
            // 
            // ForgotPasswordForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 248);
            ClientSize = new Size(620, 620);
            Controls.Add(pnlForgotCard);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ForgotPasswordForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quên mật khẩu";
            Load += ForgotPasswordForm_Load;
            pnlForgotCard.ResumeLayout(false);
            pnlForgotCard.PerformLayout();
            ResumeLayout(false);
        }
    }
}