namespace RealtimeChatClient
{
    public static class AppConfig
    {
        public static string ConnectionString =
            @"Server=LAPTOP-TKFEE1EB\XMSSQLSERVER;Database=ChatAppDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static string LoadBalancerIp = "127.0.0.1";
        public static int LoadBalancerPort = 8800;
    }
}