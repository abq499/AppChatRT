using Guna.UI2.WinForms;

namespace RealtimeChatClient
{
    partial class ProfileForm
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2Panel pnlProfileCard;

        private Label lblTitle;
        private Label lblUsername;

        private Label lblFullName;
        private Guna2TextBox txtFullName;

        private Label lblEmail;
        private Guna2TextBox txtEmail;

        private PictureBox picAvatarPreview;
        private Guna2Button btnChooseAvatar;

        private Label lblPasswordTitle;
        private Label lblOldPassword;
        private Guna2TextBox txtOldPassword;
        private Label lblNewPassword;
        private Guna2TextBox txtNewPassword;
        private Label lblConfirmPassword;
        private Guna2TextBox txtConfirmPassword;
        private Guna2Button btnChangePassword;

        private Label lblEmailChangeTitle;
        private Label lblNewEmail;
        private Guna2TextBox txtNewEmail;
        private Guna2Button btnSendEmailOtp;
        private Label lblEmailOtp;
        private Guna2TextBox txtEmailOtp;
        private Guna2Button btnUpdateEmail;

        private Guna2Button btnSave;
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
            pnlProfileCard = new Guna2Panel();

            lblTitle = new Label();
            lblUsername = new Label();

            lblFullName = new Label();
            txtFullName = new Guna2TextBox();

            lblEmail = new Label();
            txtEmail = new Guna2TextBox();

            picAvatarPreview = new PictureBox();
            btnChooseAvatar = new Guna2Button();

            lblPasswordTitle = new Label();
            lblOldPassword = new Label();
            txtOldPassword = new Guna2TextBox();
            lblNewPassword = new Label();
            txtNewPassword = new Guna2TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new Guna2TextBox();
            btnChangePassword = new Guna2Button();

            lblEmailChangeTitle = new Label();
            lblNewEmail = new Label();
            txtNewEmail = new Guna2TextBox();
            btnSendEmailOtp = new Guna2Button();
            lblEmailOtp = new Label();
            txtEmailOtp = new Guna2TextBox();
            btnUpdateEmail = new Guna2Button();

            btnSave = new Guna2Button();
            btnCancel = new Guna2Button();

            pnlProfileCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatarPreview).BeginInit();
            SuspendLayout();

            // 
            // pnlProfileCard
            // 
            pnlProfileCard.BackColor = Color.Transparent;
            pnlProfileCard.BorderRadius = 18;
            pnlProfileCard.FillColor = Color.White;
            pnlProfileCard.Location = new Point(40, 30);
            pnlProfileCard.Name = "pnlProfileCard";
            pnlProfileCard.ShadowDecoration.Enabled = true;
            pnlProfileCard.ShadowDecoration.Depth = 8;
            pnlProfileCard.Size = new Size(760, 620);
            pnlProfileCard.TabIndex = 0;

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblTitle.Location = new Point(35, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(283, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Thông tin cá nhân";

            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 10F);
            lblUsername.ForeColor = Color.FromArgb(107, 114, 128);
            lblUsername.Location = new Point(38, 72);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(91, 23);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Tài khoản:";

            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFullName.ForeColor = Color.FromArgb(75, 85, 99);
            lblFullName.Location = new Point(38, 125);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(76, 20);
            lblFullName.TabIndex = 2;
            lblFullName.Text = "Họ và tên";

            // 
            // txtFullName
            // 
            txtFullName.BorderColor = Color.FromArgb(209, 213, 219);
            txtFullName.BorderRadius = 10;
            txtFullName.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtFullName.Font = new Font("Segoe UI", 10F);
            txtFullName.Location = new Point(38, 150);
            txtFullName.Margin = new Padding(3, 4, 3, 4);
            txtFullName.Name = "txtFullName";
            txtFullName.PlaceholderText = "Nhập họ và tên";
            txtFullName.Size = new Size(320, 38);
            txtFullName.TabIndex = 3;

            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(75, 85, 99);
            lblEmail.Location = new Point(38, 205);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(46, 20);
            lblEmail.TabIndex = 4;
            lblEmail.Text = "Email";

            // 
            // txtEmail
            // 
            txtEmail.BorderColor = Color.FromArgb(209, 213, 219);
            txtEmail.BorderRadius = 10;
            txtEmail.FillColor = Color.FromArgb(249, 250, 251);
            txtEmail.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.Location = new Point(38, 230);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.ReadOnly = true;
            txtEmail.PlaceholderText = "Email hiện tại";
            txtEmail.Size = new Size(320, 38);
            txtEmail.TabIndex = 5;

            // 
            // picAvatarPreview
            // 
            picAvatarPreview.BackColor = Color.WhiteSmoke;
            picAvatarPreview.BorderStyle = BorderStyle.FixedSingle;
            picAvatarPreview.Location = new Point(132, 300);
            picAvatarPreview.Name = "picAvatarPreview";
            picAvatarPreview.Size = new Size(128, 128);
            picAvatarPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatarPreview.TabIndex = 6;
            picAvatarPreview.TabStop = false;

            // 
            // btnChooseAvatar
            // 
            btnChooseAvatar.BorderRadius = 10;
            btnChooseAvatar.FillColor = Color.FromArgb(239, 246, 255);
            btnChooseAvatar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnChooseAvatar.ForeColor = Color.FromArgb(37, 99, 235);
            btnChooseAvatar.Location = new Point(92, 445);
            btnChooseAvatar.Name = "btnChooseAvatar";
            btnChooseAvatar.Size = new Size(210, 38);
            btnChooseAvatar.TabIndex = 7;
            btnChooseAvatar.Text = "Chọn avatar";
            btnChooseAvatar.Click += btnChooseAvatar_Click;

            // 
            // lblPasswordTitle
            // 
            lblPasswordTitle.AutoSize = true;
            lblPasswordTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblPasswordTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblPasswordTitle.Location = new Point(405, 35);
            lblPasswordTitle.Name = "lblPasswordTitle";
            lblPasswordTitle.Size = new Size(140, 28);
            lblPasswordTitle.TabIndex = 10;
            lblPasswordTitle.Text = "Đổi mật khẩu";

            // 
            // lblOldPassword
            // 
            lblOldPassword.AutoSize = true;
            lblOldPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblOldPassword.ForeColor = Color.FromArgb(75, 85, 99);
            lblOldPassword.Location = new Point(405, 82);
            lblOldPassword.Name = "lblOldPassword";
            lblOldPassword.Size = new Size(91, 20);
            lblOldPassword.TabIndex = 11;
            lblOldPassword.Text = "Mật khẩu cũ";

            // 
            // txtOldPassword
            // 
            txtOldPassword.BorderColor = Color.FromArgb(209, 213, 219);
            txtOldPassword.BorderRadius = 10;
            txtOldPassword.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtOldPassword.Font = new Font("Segoe UI", 10F);
            txtOldPassword.Location = new Point(405, 107);
            txtOldPassword.Margin = new Padding(3, 4, 3, 4);
            txtOldPassword.Name = "txtOldPassword";
            txtOldPassword.PasswordChar = '*';
            txtOldPassword.PlaceholderText = "Nhập mật khẩu cũ";
            txtOldPassword.Size = new Size(310, 38);
            txtOldPassword.TabIndex = 12;

            // 
            // lblNewPassword
            // 
            lblNewPassword.AutoSize = true;
            lblNewPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNewPassword.ForeColor = Color.FromArgb(75, 85, 99);
            lblNewPassword.Location = new Point(405, 157);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(105, 20);
            lblNewPassword.TabIndex = 13;
            lblNewPassword.Text = "Mật khẩu mới";

            // 
            // txtNewPassword
            // 
            txtNewPassword.BorderColor = Color.FromArgb(209, 213, 219);
            txtNewPassword.BorderRadius = 10;
            txtNewPassword.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtNewPassword.Font = new Font("Segoe UI", 10F);
            txtNewPassword.Location = new Point(405, 182);
            txtNewPassword.Margin = new Padding(3, 4, 3, 4);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PasswordChar = '*';
            txtNewPassword.PlaceholderText = "Nhập mật khẩu mới";
            txtNewPassword.Size = new Size(310, 38);
            txtNewPassword.TabIndex = 14;

            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblConfirmPassword.ForeColor = Color.FromArgb(75, 85, 99);
            lblConfirmPassword.Location = new Point(405, 232);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(137, 20);
            lblConfirmPassword.TabIndex = 15;
            lblConfirmPassword.Text = "Xác nhận mật khẩu";

            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.BorderColor = Color.FromArgb(209, 213, 219);
            txtConfirmPassword.BorderRadius = 10;
            txtConfirmPassword.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtConfirmPassword.Font = new Font("Segoe UI", 10F);
            txtConfirmPassword.Location = new Point(405, 257);
            txtConfirmPassword.Margin = new Padding(3, 4, 3, 4);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '*';
            txtConfirmPassword.PlaceholderText = "Nhập lại mật khẩu mới";
            txtConfirmPassword.Size = new Size(310, 38);
            txtConfirmPassword.TabIndex = 16;

            // 
            // btnChangePassword
            // 
            btnChangePassword.BorderRadius = 10;
            btnChangePassword.FillColor = Color.FromArgb(37, 99, 235);
            btnChangePassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnChangePassword.ForeColor = Color.White;
            btnChangePassword.Location = new Point(405, 315);
            btnChangePassword.Name = "btnChangePassword";
            btnChangePassword.Size = new Size(310, 38);
            btnChangePassword.TabIndex = 17;
            btnChangePassword.Text = "Đổi mật khẩu";
            btnChangePassword.Click += btnChangePassword_Click;

            // 
            // lblEmailChangeTitle
            // 
            lblEmailChangeTitle.AutoSize = true;
            lblEmailChangeTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblEmailChangeTitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblEmailChangeTitle.Location = new Point(405, 385);
            lblEmailChangeTitle.Name = "lblEmailChangeTitle";
            lblEmailChangeTitle.Size = new Size(101, 28);
            lblEmailChangeTitle.TabIndex = 18;
            lblEmailChangeTitle.Text = "Đổi email";

            // 
            // lblNewEmail
            // 
            lblNewEmail.AutoSize = true;
            lblNewEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNewEmail.ForeColor = Color.FromArgb(75, 85, 99);
            lblNewEmail.Location = new Point(405, 432);
            lblNewEmail.Name = "lblNewEmail";
            lblNewEmail.Size = new Size(77, 20);
            lblNewEmail.TabIndex = 19;
            lblNewEmail.Text = "Email mới";

            // 
            // txtNewEmail
            // 
            txtNewEmail.BorderColor = Color.FromArgb(209, 213, 219);
            txtNewEmail.BorderRadius = 10;
            txtNewEmail.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtNewEmail.Font = new Font("Segoe UI", 10F);
            txtNewEmail.Location = new Point(405, 457);
            txtNewEmail.Margin = new Padding(3, 4, 3, 4);
            txtNewEmail.Name = "txtNewEmail";
            txtNewEmail.PlaceholderText = "Nhập email mới";
            txtNewEmail.Size = new Size(190, 38);
            txtNewEmail.TabIndex = 20;

            // 
            // btnSendEmailOtp
            // 
            btnSendEmailOtp.BorderRadius = 10;
            btnSendEmailOtp.FillColor = Color.FromArgb(239, 246, 255);
            btnSendEmailOtp.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSendEmailOtp.ForeColor = Color.FromArgb(37, 99, 235);
            btnSendEmailOtp.Location = new Point(605, 457);
            btnSendEmailOtp.Name = "btnSendEmailOtp";
            btnSendEmailOtp.Size = new Size(110, 38);
            btnSendEmailOtp.TabIndex = 21;
            btnSendEmailOtp.Text = "Gửi OTP";
            btnSendEmailOtp.Click += btnSendEmailOtp_Click;

            // 
            // lblEmailOtp
            // 
            lblEmailOtp.AutoSize = true;
            lblEmailOtp.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEmailOtp.ForeColor = Color.FromArgb(75, 85, 99);
            lblEmailOtp.Location = new Point(405, 507);
            lblEmailOtp.Name = "lblEmailOtp";
            lblEmailOtp.Size = new Size(64, 20);
            lblEmailOtp.TabIndex = 22;
            lblEmailOtp.Text = "Mã OTP";

            // 
            // txtEmailOtp
            // 
            txtEmailOtp.BorderColor = Color.FromArgb(209, 213, 219);
            txtEmailOtp.BorderRadius = 10;
            txtEmailOtp.FocusedState.BorderColor = Color.FromArgb(37, 99, 235);
            txtEmailOtp.Font = new Font("Segoe UI", 10F);
            txtEmailOtp.Location = new Point(405, 532);
            txtEmailOtp.Margin = new Padding(3, 4, 3, 4);
            txtEmailOtp.Name = "txtEmailOtp";
            txtEmailOtp.PlaceholderText = "Nhập mã OTP";
            txtEmailOtp.Size = new Size(190, 38);
            txtEmailOtp.TabIndex = 23;

            // 
            // btnUpdateEmail
            // 
            btnUpdateEmail.BorderRadius = 10;
            btnUpdateEmail.FillColor = Color.FromArgb(37, 99, 235);
            btnUpdateEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnUpdateEmail.ForeColor = Color.White;
            btnUpdateEmail.Location = new Point(605, 532);
            btnUpdateEmail.Name = "btnUpdateEmail";
            btnUpdateEmail.Size = new Size(110, 38);
            btnUpdateEmail.TabIndex = 24;
            btnUpdateEmail.Text = "Cập nhật";
            btnUpdateEmail.Click += btnUpdateEmail_Click;

            // 
            // btnSave
            // 
            btnSave.BorderRadius = 10;
            btnSave.FillColor = Color.FromArgb(37, 99, 235);
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(38, 535);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 40);
            btnSave.TabIndex = 8;
            btnSave.Text = "Lưu";
            btnSave.Click += btnSave_Click;

            // 
            // btnCancel
            // 
            btnCancel.BorderRadius = 10;
            btnCancel.FillColor = Color.FromArgb(239, 246, 255);
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.FromArgb(37, 99, 235);
            btnCancel.Location = new Point(208, 535);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(150, 40);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Hủy";
            btnCancel.Click += btnCancel_Click;

            // 
            // Add controls to card
            // 
            pnlProfileCard.Controls.Add(lblTitle);
            pnlProfileCard.Controls.Add(lblUsername);
            pnlProfileCard.Controls.Add(lblFullName);
            pnlProfileCard.Controls.Add(txtFullName);
            pnlProfileCard.Controls.Add(lblEmail);
            pnlProfileCard.Controls.Add(txtEmail);
            pnlProfileCard.Controls.Add(picAvatarPreview);
            pnlProfileCard.Controls.Add(btnChooseAvatar);
            pnlProfileCard.Controls.Add(btnSave);
            pnlProfileCard.Controls.Add(btnCancel);

            pnlProfileCard.Controls.Add(lblPasswordTitle);
            pnlProfileCard.Controls.Add(lblOldPassword);
            pnlProfileCard.Controls.Add(txtOldPassword);
            pnlProfileCard.Controls.Add(lblNewPassword);
            pnlProfileCard.Controls.Add(txtNewPassword);
            pnlProfileCard.Controls.Add(lblConfirmPassword);
            pnlProfileCard.Controls.Add(txtConfirmPassword);
            pnlProfileCard.Controls.Add(btnChangePassword);

            pnlProfileCard.Controls.Add(lblEmailChangeTitle);
            pnlProfileCard.Controls.Add(lblNewEmail);
            pnlProfileCard.Controls.Add(txtNewEmail);
            pnlProfileCard.Controls.Add(btnSendEmailOtp);
            pnlProfileCard.Controls.Add(lblEmailOtp);
            pnlProfileCard.Controls.Add(txtEmailOtp);
            pnlProfileCard.Controls.Add(btnUpdateEmail);

            // 
            // ProfileForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 248);
            ClientSize = new Size(840, 680);
            Controls.Add(pnlProfileCard);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProfileForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thông tin cá nhân";

            ((System.ComponentModel.ISupportInitialize)picAvatarPreview).EndInit();
            pnlProfileCard.ResumeLayout(false);
            pnlProfileCard.PerformLayout();
            ResumeLayout(false);
        }
    }
}