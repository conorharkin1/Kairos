using System.Security.Cryptography;
using System.Text;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-9.0
    // I thought I should reference the resource I used to help implement this.
    public class EncryptService : IEncryptService
    {
        public string Encrypt(string plainText, Guid key)
        {
            string keyBase = key.ToString();
            byte[] derivedKey = DeriveKey(keyBase, 32);
            byte[] iv = DeriveKey(keyBase + "IV", 16);

            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = derivedKey;
                aesAlg.IV = iv;

                using (var encryptor = aesAlg.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    encrypted = ms.ToArray();
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string cipherText, Guid key)
        {
            string keyBase = key.ToString();
            byte[] derivedKey = DeriveKey(keyBase, 32);
            byte[] iv = DeriveKey(keyBase + "IV", 16);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = derivedKey;
                aesAlg.IV = iv;

                using (var decryptor = aesAlg.CreateDecryptor())
                using (var ms = new MemoryStream(cipherBytes))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static byte[] DeriveKey(string input, int keySizeInBytes)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = SHA256.HashData(inputBytes);
            byte[] result = new byte[keySizeInBytes];
            Array.Copy(hash, result, Math.Min(hash.Length, keySizeInBytes));
            return result;
        }
    }
}