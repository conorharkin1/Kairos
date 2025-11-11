using System.Security.Cryptography;
using System.Text;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class EncryptService : IEncryptService
    {
        public string Encrypt(string plainText, string key, string iv)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                throw new ArgumentException("Key and IV are required");
            }

            var KEY = Encoding.UTF32.GetBytes(key);
            var IV = Encoding.UTF32.GetBytes(iv);

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = KEY;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            return Encoding.UTF32.GetString(encrypted);
        }

        public string Decrypt(string cipherText, string key, string iv)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                throw new ArgumentException("Key and IV are required");
            }
            
            var KEY = Encoding.UTF32.GetBytes(key);
            var IV = Encoding.UTF32.GetBytes(iv);
            var cipherTextBytes = Encoding.UTF32.GetBytes(cipherText);

            string plaintext;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = KEY;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}