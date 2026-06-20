using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RealtimeChatClient
{
    public partial class LoginForm : Form
    {
        // Biến lưu tên người dùng sau khi đăng nhập thành công
        public string LoggedInUsername { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = CryptoHelper.HashPassword(txtPassword.Text.Trim());

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!");
                return;
            }

            // Chuỗi kết nối lấy chính xác từ Server của máy em
            string connectionString = @"Server=LAPTOP-TKFEE1EB\XMSSQLSERVER;Database=ChatAppDB;Trusted_Connection=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Mở cửa kết nối vào SQL Server

                    // Câu lệnh SQL kiểm tra xem có tài khoản nào khớp user/pass không
                    string query = "SELECT COUNT(1) FROM Users WHERE Username = @user AND PasswordHash = @pass";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Chống SQL Injection
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@pass", password);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // Đăng nhập thành công
                            LoggedInUsername = username;
                            this.DialogResult = DialogResult.OK; // Báo hiệu thành công
                            this.Close(); // Đóng form đăng nhập
                        }
                        else
                        {
                            MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGoToRegister_Click(object sender, EventArgs e)
        {
            RegisterForm regForm = new RegisterForm();
            regForm.ShowDialog(); // Mở cửa sổ đăng ký lên
        }
    }
}