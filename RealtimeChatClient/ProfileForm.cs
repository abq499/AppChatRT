using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace RealtimeChatClient
{
    public partial class ProfileForm : Form
    {
        public string AvatarBase64 { get; private set; } = "";
        public string FullName { get; private set; } = "";

        private string _username;
        private string _email;
        private string _currentAvatarBase64;
        private string _currentFullName;
        private ChatClientLogic _chatLogic;
        private string _emailOtp = "";
        private DateTime _emailOtpExpiredAt;
        private string _pendingNewEmail = "";
        public ProfileForm(
            string username,
            string fullName,
            string email,
            string currentAvatarBase64,
            ChatClientLogic chatLogic)
        {
            InitializeComponent();

            _username = username;
            _currentFullName = fullName;
            _email = email;
            _currentAvatarBase64 = currentAvatarBase64;
            _chatLogic = chatLogic;

            lblUsername.Text = $"Tài khoản: {_username}";
            txtFullName.Text = _currentFullName;
            txtEmail.Text = _email;

            FullName = _currentFullName;

            if (!string.IsNullOrEmpty(_currentAvatarBase64))
            {
                AvatarBase64 = _currentAvatarBase64;
                picAvatarPreview.Image = Base64ToImage(_currentAvatarBase64);
            }
        }
        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async void btnSendEmailOtp_Click(object sender, EventArgs e)
        {
            string newEmail = txtNewEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
            {
                MessageBox.Show("Vui lòng nhập email mới hợp lệ!");
                return;
            }

            if (newEmail == _email)
            {
                MessageBox.Show("Email mới không được trùng với email hiện tại!");
                return;
            }

            try
            {
                btnSendEmailOtp.Enabled = false;
                btnSendEmailOtp.Text = "Đang gửi...";

                _pendingNewEmail = newEmail;
                _emailOtp = GenerateOtp();
                _emailOtpExpiredAt = DateTime.Now.AddMinutes(5);

                await EmailHelper.SendVerificationCodeAsync(_pendingNewEmail, _emailOtp);

                MessageBox.Show($"Mã OTP đã được gửi đến email mới: {_pendingNewEmail}\nMã có hiệu lực trong 5 phút.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi OTP email: " + ex.Message);
            }
            finally
            {
                btnSendEmailOtp.Enabled = true;
                btnSendEmailOtp.Text = "Gửi OTP email";
            }
        }
        private async void btnUpdateEmail_Click(object sender, EventArgs e)
        {
            string inputOtp = txtEmailOtp.Text.Trim();
            string newEmail = txtNewEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(_pendingNewEmail) || string.IsNullOrWhiteSpace(_emailOtp))
            {
                MessageBox.Show("Vui lòng gửi OTP trước khi cập nhật email!");
                return;
            }

            if (newEmail != _pendingNewEmail)
            {
                MessageBox.Show("Email mới đã thay đổi so với email nhận OTP. Vui lòng gửi lại OTP!");
                return;
            }

            if (DateTime.Now > _emailOtpExpiredAt)
            {
                MessageBox.Show("Mã OTP đã hết hạn. Vui lòng gửi lại mã mới!");
                return;
            }

            if (inputOtp != _emailOtp)
            {
                MessageBox.Show("Mã OTP không đúng!");
                return;
            }

            try
            {
                btnUpdateEmail.Enabled = false;
                btnUpdateEmail.Text = "Đang cập nhật...";

                await _chatLogic.UpdateEmailAsync(newEmail);

                // Cập nhật tạm trên form, server sẽ gửi EMAIL_UPDATED/PROFILE_UPDATED về Form1
                _email = newEmail;
                txtEmail.Text = newEmail;

                txtNewEmail.Clear();
                txtEmailOtp.Clear();
                _pendingNewEmail = "";
                _emailOtp = "";

                MessageBox.Show("Đã gửi yêu cầu cập nhật email lên server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật email: " + ex.Message);
            }
            finally
            {
                btnUpdateEmail.Enabled = true;
                btnUpdateEmail.Text = "Cập nhật email";
            }
        }
        private async void btnChangePassword_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(oldPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin đổi mật khẩu!");
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới nên có ít nhất 6 ký tự!");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không khớp!");
                return;
            }

            if (oldPassword == newPassword)
            {
                MessageBox.Show("Mật khẩu mới không nên trùng với mật khẩu cũ!");
                return;
            }

            try
            {
                btnChangePassword.Enabled = false;
                btnChangePassword.Text = "Đang đổi...";

                string oldPasswordHash = CryptoHelper.HashPassword(oldPassword);
                string newPasswordHash = CryptoHelper.HashPassword(newPassword);

                await _chatLogic.ChangePasswordAsync(oldPasswordHash, newPasswordHash);

                txtOldPassword.Clear();
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi yêu cầu đổi mật khẩu: " + ex.Message);
            }
            finally
            {
                btnChangePassword.Enabled = true;
                btnChangePassword.Text = "Đổi mật khẩu";
            }
        }
        private void btnChooseAvatar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh đại diện";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (Image originalImage = Image.FromFile(ofd.FileName))
                        using (Image resizedImage = ResizeImage(originalImage, 128, 128))
                        {
                            AvatarBase64 = ImageToBase64(resizedImage);
                            picAvatarPreview.Image = Base64ToImage(AvatarBase64);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể chọn ảnh: " + ex.Message);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Họ tên không được để trống!");
                return;
            }

            FullName = fullName;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(Color.White);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height);
            }

            return result;
        }

        private string ImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
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
    }
}