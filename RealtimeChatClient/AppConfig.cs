namespace RealtimeChatClient
{
    public static class AppConfig
    {
        public static string ConnectionString =
            @"Server=YOUR_SERVER_NAME;Database=ChatAppDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static string LoadBalancerIp = "127.0.0.1";
        public static int LoadBalancerPort = 8800;

        // SMTP config dùng để gửi mã xác nhận email
        public static string SmtpHost = "smtp.gmail.com";
        public static int SmtpPort = 587;
        public static string SmtpEmail = "your_email@gmail.com";
        public static string SmtpPassword = "your_app_password";
        public static string SmtpDisplayName = "Realtime Chat App";
    }
}