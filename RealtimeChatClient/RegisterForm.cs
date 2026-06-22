using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace RealtimeChatClient
{
    public partial class RegisterForm : Form
    {
        private string _currentOtp = "";
        private DateTime _otpExpiredAt;
        public RegisterForm()
        {
            InitializeComponent();
        }
        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        private async void btnSendOtp_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Vui lòng nhập email trước khi gửi mã xác nhận!");
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Email không hợp lệ!");
                return;
            }

            try
            {
                btnSendOtp.Enabled = false;
                btnSendOtp.Text = "Đang gửi...";

                _currentOtp = GenerateOtp();
                _otpExpiredAt = DateTime.Now.AddMinutes(5);

                await EmailHelper.SendVerificationCodeAsync(email, _currentOtp);

                MessageBox.Show("Mã xác nhận đã được gửi đến email của bạn. Mã có hiệu lực trong 5 phút.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi email: " + ex.Message);
            }
            finally
            {
                btnSendOtp.Enabled = true;
                btnSendOtp.Text = "Gửi mã";
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string rawPass = txtPass.Text.Trim();
            string pass = CryptoHelper.HashPassword(rawPass);
            string name = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string inputOtp = txtOtp.Text.Trim();

            if (user == "" || rawPass == "" || name == "" || email == "" || inputOtp == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và mã xác nhận!");
                return;
            }

            if (string.IsNullOrEmpty(_currentOtp))
            {
                MessageBox.Show("Vui lòng bấm 'Gửi mã' để nhận mã xác nhận email!");
                return;
            }

            if (DateTime.Now > _otpExpiredAt)
            {
                MessageBox.Show("Mã xác nhận đã hết hạn. Vui lòng gửi lại mã mới!");
                return;
            }

            if (inputOtp != _currentOtp)
            {
                MessageBox.Show("Mã xác nhận không đúng!");
                return;
            }

            string connStr = AppConfig.ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(1) FROM Users WHERE Username = @user";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@user", user);

                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Tên đăng nhập này đã có người sử dụng!");
                        return;
                    }

                    string checkEmailQuery = "SELECT COUNT(1) FROM Users WHERE Email = @mail";
                    SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, conn);
                    checkEmailCmd.Parameters.AddWithValue("@mail", email);

                    if ((int)checkEmailCmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Email này đã được sử dụng!");
                        return;
                    }

                    string insertQuery = @"
                INSERT INTO Users (Username, PasswordHash, FullName, Email, IsEmailVerified)
                VALUES (@user, @pass, @name, @mail, 1)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", user);
                        cmd.Parameters.AddWithValue("@pass", pass);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@mail", email);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Đăng ký thành công! Email đã được xác nhận.");
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}