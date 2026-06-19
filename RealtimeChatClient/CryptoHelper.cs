using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace RealtimeChatClient
{
    public class CryptoHelper
    {
        // Chìa khóa bí mật (Phải đủ 32 ký tự cho chuẩn AES-256). 
        // Cả 2 máy Client đều ngầm dùng chung khóa này.
        private static readonly string SecretKey = "uit_network_programming_chat_key";

        // Hàm Khóa tin nhắn
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;
            byte[] iv = new byte[16]; // Vector khởi tạo (để mặc định là 0)
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(SecretKey);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array); // Chuyển byte thành chuỗi chữ để gửi qua mạng
        }

        // Hàm Mở khóa tin nhắn
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;
            byte[] iv = new byte[16];
            try
            {
                byte[] buffer = Convert.FromBase64String(cipherText);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(SecretKey);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                // Nếu ai đó cố tình gửi chuỗi rác không mở khóa được, báo lỗi nhẹ nhàng
                return "[Tin nhắn đã bị lỗi mã hóa]";
            }
        }
    }
}