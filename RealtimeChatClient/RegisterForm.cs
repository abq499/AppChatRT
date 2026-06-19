using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RealtimeChatClient
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text.Trim();
            string name = txtFullName.Text.Trim();

            if (user == "" || pass == "" || name == "")
            {
                MessageBox.Show("Vui lòng không để trống thông tin!");
                return;
            }

            string connStr = @"Server=LAPTOP-TKFEE1EB\XMSSQLSERVER;Database=ChatAppDB;Trusted_Connection=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Bước 1: Kiểm tra xem tên đăng nhập đã tồn tại chưa
                    string checkQuery = "SELECT COUNT(1) FROM Users WHERE Username = @user";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@user", user);

                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Tên đăng nhập này đã có người sử dụng!");
                        return;
                    }

                    // Bước 2: Thực hiện chèn (Insert) dữ liệu mới
                    string insertQuery = "INSERT INTO Users (Username, PasswordHash, FullName) VALUES (@user, @pass, @name)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", user);
                        cmd.Parameters.AddWithValue("@pass", pass); // Lưu ý: Đồ án thực tế nên Hash Pass nhé
                        cmd.Parameters.AddWithValue("@name", name);

                        int result = cmd.ExecuteNonQuery(); // Trả về số dòng bị tác động

                        if (result > 0)
                        {
                            MessageBox.Show("Đăng ký thành công! Giờ bạn có thể đăng nhập.");
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