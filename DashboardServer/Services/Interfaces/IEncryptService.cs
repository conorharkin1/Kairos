namespace DashboardServer.Services.Interfaces
{
    public interface IEncryptService
    {
        string Encrypt(string plainText, string key, string iv);
        string Decrypt(string cipherText, string key, string iv);
    }
}