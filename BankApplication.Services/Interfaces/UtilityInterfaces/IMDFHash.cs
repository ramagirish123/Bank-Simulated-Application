namespace BankApplication.Services.Interfaces.Utility
{
    public interface IMdfHash
    {
        string GetMD5Hash(string password);
    }
}