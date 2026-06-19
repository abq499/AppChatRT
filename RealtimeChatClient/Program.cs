namespace RealtimeChatClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Chạy Form Đăng nhập trước
            LoginForm login = new LoginForm();

            // Nếu đăng nhập thành công (DialogResult == OK)
            if (login.ShowDialog() == DialogResult.OK)
            {
                // Mở Form1 và truyền tên người dùng đã đăng nhập vào
                Application.Run(new Form1(login.LoggedInUsername));
            }
        }
    }
}