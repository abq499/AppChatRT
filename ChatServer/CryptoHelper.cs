using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace RealtimeChatClient
{
    public class CryptoHelper
    {
        // 1. CHÌA KHÓA AES ĐÃ CHUYỂN THÀNH DYNAMIC (Không còn hardcode nữa)
        // Mặc định để 1 chuỗi 32 byte để tránh lỗi null ban đầu, sau đó sẽ bị ghi đè lúc chạy.
        public static string DynamicAESKey = "uit_network_programming_chat_key";

        // ---------------- CÁC HÀM XỬ LÝ AES (Dùng để mã hóa tin nhắn) ----------------
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                // Dùng khóa động thay vì khóa cứng
                aes.Key = Encoding.UTF8.GetBytes(DynamicAESKey.PadRight(32).Substring(0, 32));
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
            return Convert.ToBase64String(array);
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;
            byte[] iv = new byte[16];
            try
            {
                byte[] buffer = Convert.FromBase64String(cipherText);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(DynamicAESKey.PadRight(32).Substring(0, 32));
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
                return "[Tin nhắn đã bị lỗi mã hóa]";
            }
        }

        // ---------------- CÁC HÀM XỬ LÝ RSA VÀ TẠO KHÓA (MỚI THÊM) ----------------

        // Hàm băm mật khẩu lúc Đăng nhập/Đăng ký
        public static string HashPassword(string rawPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Client dùng hàm này để tạo ra 1 khóa AES ngẫu nhiên (32 ký tự)
        public static string GenerateRandomAESKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            var key = new string(Enumerable.Repeat(chars, 32).Select(s => s[random.Next(s.Length)]).ToArray());
            return key;
        }

        // Server dùng hàm này để tạo 1 cặp khóa RSA
        public static void GenerateRSAKeys(out string publicKey, out string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                publicKey = rsa.ToXmlString(false); // Chỉ xuất Public Key
                privateKey = rsa.ToXmlString(true);  // Xuất cả Private Key
            }
        }

        // Client dùng hàm này để mã hóa cái AES Key bằng RSA Public Key
        public static string RSAEncrypt(string data, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(publicKey);
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] encryptedBytes = rsa.Encrypt(dataBytes, false);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        // Server dùng hàm này để giải mã, lấy lại AES Key bằng RSA Private Key
        public static string RSADecrypt(string encryptedData, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(privateKey);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}