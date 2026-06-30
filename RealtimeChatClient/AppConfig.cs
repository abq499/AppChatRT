namespace RealtimeChatClient
{
    public static class AppConfig
    {
        public static string ConnectionString =
            @"Server=LAPTOP-L4MDGNNE;Database=ChatAppDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static string LoadBalancerIp = " 192.168.199.1";
        public static int LoadBalancerPort = 8800;

        // SMTP config dùng để gửi mã xác nhận email
        public static string SmtpHost = "smtp.gmail.com";
        public static int SmtpPort = 587;
        public static string SmtpEmail = "youremail";
        public static string SmtpPassword = "code app";
        public static string SmtpDisplayName = "Realtime Chat App";
    }
}