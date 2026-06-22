using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealtimeChatClient
{
    public partial class ForgotPasswordForm : Form
    {
        private string _currentOtp = "";
        private DateTime _otpExpiredAt;
        private string _verifiedUsername = "";
        private string _verifiedEmail = "";

        public ForgotPasswordForm()
        {
            InitializeComponent();
        }

        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task<(bool Success, string Username, string Email, string Error)> FindUserAsync(string input)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(AppConfig.ConnectionString))
                {
                    await conn.OpenAsync();

                    string query = @"
                        SELECT Username, Email
                        FROM Users
                        WHERE Username = @input OR Email = @input";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@input", input);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                string username = reader["Username"]?.ToString() ?? "";
                                string email = reader["Email"]?.ToString() ?? "";

                                if (string.IsNullOrWhiteSpace(email))
                                    return (false, "", "", "Tài khoản này chưa có email để khôi phục mật khẩu.");

                                return (true, username, email, "");
                            }
                        }
                    }
                }

                return (false, "", "", "Không tìm thấy tài khoản hoặc email.");
            }
            catch (Exception ex)
            {
                return (false, "", "", ex.Message);
            }
        }

        private async Task UpdatePasswordAsync(string username, string newPasswordHash)
        {
            using (SqlConnection conn = new SqlConnection(AppConfig.ConnectionString))
            {
                await conn.OpenAsync();

                string query = @"
                    UPDATE Users
                    SET PasswordHash = @passwordHash
                    WHERE Username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@passwordHash", newPasswordHash);
                    cmd.Parameters.AddWithValue("@username", username);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private async void btnSendOtp_Click(object sender, EventArgs e)
        {
            string input = txtUsernameOrEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Vui lòng nhập username hoặc email!");
                return;
            }

            try
            {
                btnSendOtp.Enabled = false;
                btnSendOtp.Text = "Đang gửi...";

                var result = await FindUserAsync(input);

                if (!result.Success)
                {
                    MessageBox.Show(result.Error);
                    return;
                }

                _verifiedUsername = result.Username;
                _verifiedEmail = result.Email;

                _currentOtp = GenerateOtp();
                _otpExpiredAt = DateTime.Now.AddMinutes(5);

                await EmailHelper.SendVerificationCodeAsync(_verifiedEmail, _currentOtp);

                MessageBox.Show($"Mã OTP đã được gửi đến email: {_verifiedEmail}\nMã có hiệu lực trong 5 phút.");

                txtOtp.Enabled = true;
                txtNewPassword.Enabled = true;
                txtConfirmPassword.Enabled = true;
                btnResetPassword.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi OTP: " + ex.Message);
            }
            finally
            {
                btnSendOtp.Enabled = true;
                btnSendOtp.Text = "Gửi mã OTP";
            }
        }

        private async void btnResetPassword_Click(object sender, EventArgs e)
        {
            string otp = txtOtp.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(_verifiedUsername))
            {
                MessageBox.Show("Vui lòng gửi OTP trước!");
                return;
            }

            if (string.IsNullOrWhiteSpace(otp))
            {
                MessageBox.Show("Vui lòng nhập mã OTP!");
                return;
            }

            if (DateTime.Now > _otpExpiredAt)
            {
                MessageBox.Show("Mã OTP đã hết hạn. Vui lòng gửi lại mã mới!");
                return;
            }

            if (otp != _currentOtp)
            {
                MessageBox.Show("Mã OTP không đúng!");
                return;
            }

            if (string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới và xác nhận mật khẩu!");
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

            try
            {
                btnResetPassword.Enabled = false;
                btnResetPassword.Text = "Đang đổi...";

                string newPasswordHash = CryptoHelper.HashPassword(newPassword);

                await UpdatePasswordAsync(_verifiedUsername, newPasswordHash);

                MessageBox.Show("Đặt lại mật khẩu thành công! Bạn có thể đăng nhập bằng mật khẩu mới.");

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đặt lại mật khẩu: " + ex.Message);
            }
            finally
            {
                btnResetPassword.Enabled = true;
                btnResetPassword.Text = "Đặt lại mật khẩu";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ForgotPasswordForm_Load(object sender, EventArgs e)
        {

        }
    }
}