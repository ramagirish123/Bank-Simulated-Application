using System.Collections.Generic;

namespace BankApplication.Services.Interfaces
{
    public interface IAccountService
    {
        string AddAccount(string customerID);
        string DeleteAccount(string ID);
        string Deposit(string accountID, float amount);
        IList<string> GetAccountsbyUserID(string userID);
        bool IsBalanceSufficient(string accountID, float amount);
        string Transfer(string debitingAccountID, string creditingAccountID,float amount);
        string UpdatebalanceAfterReverting(string creditedAccountID, string debitedAccountID, float amount);
        string WithDraw(string accountID, float amount);
        bool IsAccountExists(string ID);
    }
}