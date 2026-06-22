using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RealtimeChatClient
{
    public class CryptoHelper
    {
        // Khoa AES dong bo giua client va server sau buoc bat tay RSA
        public static string DynamicAESKey = "uit_network_programming_chat_key";

        // Tao key AES 32 bytes tu chuoi DynamicAESKey
        private static byte[] GetAESKey()
        {
            return Encoding.UTF8.GetBytes(DynamicAESKey.PadRight(32).Substring(0, 32));
        }

        // Ma hoa AES: moi tin nhan sinh IV ngau nhien
        // Format tra ve: Base64(IV + CipherText)
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = GetAESKey();
                    aes.GenerateIV();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Ghi IV vao dau goi tin de ben nhan lay ra giai ma
                        memoryStream.Write(aes.IV, 0, aes.IV.Length);

                        using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream, Encoding.UTF8))
                        {
                            streamWriter.Write(plainText);
                        }

                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
            catch
            {
                return "[Loi ma hoa]";
            }
        }

        // Giai ma AES: tach 16 byte dau lam IV, phan sau la ciphertext
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                byte[] fullCipher = Convert.FromBase64String(cipherText);

                // AES block size IV = 16 bytes
                if (fullCipher.Length <= 16)
                    return "[Tin nhan da bi loi ma hoa]";

                byte[] iv = new byte[16];
                byte[] cipher = new byte[fullCipher.Length - 16];

                Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                Array.Copy(fullCipher, 16, cipher, 0, cipher.Length);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = GetAESKey();
                    aes.IV = iv;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (MemoryStream memoryStream = new MemoryStream(cipher))
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    using (StreamReader streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return "[Tin nhan da bi loi ma hoa]";
            }
        }

        // Bam mat khau de luu DB
        public static string HashPassword(string rawPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }

        // Tao AES key ngau nhien
        public static string GenerateRandomAESKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                char[] result = new char[32];
                byte[] randomBytes = new byte[32];

                rng.GetBytes(randomBytes);

                for (int i = 0; i < result.Length; i++)
                    result[i] = chars[randomBytes[i] % chars.Length];

                return new string(result);
            }
        }

        // Tao cap khoa RSA
        public static void GenerateRSAKeys(out string publicKey, out string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);
            }
        }

        // Ma hoa AES key bang RSA Public Key
        public static string RSAEncrypt(string data, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(publicKey);
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                // true = OAEP padding
                byte[] encryptedBytes = rsa.Encrypt(dataBytes, true);

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        // Giai ma AES key bang RSA Private Key
        public static string RSADecrypt(string encryptedData, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(privateKey);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

                // Phai dung true giong RSAEncrypt
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, true);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}