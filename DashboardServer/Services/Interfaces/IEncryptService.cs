namespace DashboardServer.Services.Interfaces
{
    public interface IEncryptService
    {
        string Encrypt(string plainText, Guid key);
        string Decrypt(string cipherText, Guid key);
    }
}