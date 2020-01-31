using System;
using System.Collections.Generic;
using System.Text;
using BankApplication.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using BankApplication.Data;

namespace BankApplication.Services
{
    public class AccountService : Interfaces.IAccountService
    {
        private TransactionService _transactionService;
        private DataProvider _dataProvider;
        private string _userID;

        public AccountService(string userID,string JsonFilePath)
        {
            _userID = userID;
            _transactionService = new TransactionService(userID,JsonFilePath);
            _dataProvider = new DataProvider(JsonFilePath);
        }

        // WithDraw amount from bankaccount.
        public string WithDraw(string accountID, float amount)
        {
            string message;
            Account account;
            Models.Transaction.Transaction newTransaction;

            account = _dataProvider.GetObjectByID<Account>(accountID);
            if (account == null)
            {
                return "Invalid Account ID";
            }
            // Transaction is stopped if amount is 0 or neagative value.
            if (amount <= 0)
            {
                message = "Enter only possitive value for the amount";
                return message;
            }
            bool IsSufficientBalance = IsBalanceSufficient(accountID, amount);
            //Check Wheather sufficient balance in the account.
            if (IsSufficientBalance)
            {
                // Creating Transaction.
                newTransaction = new Models.Transaction.Transaction()
                {
                    CreatedOn = DateTime.Now,
                    IsReverted = false,
                    Type = Models.Transaction.TransactionType.Withdraw,
                    Amount = amount,
                    PayorID = accountID,
                };
                // Adding Transaction
                _transactionService.AddTransaction(newTransaction);
                // Updating balance of the account.
                account.Balance -= amount;
                if (_dataProvider.UpdateObject<Account>(account))
                {
                    message = "  TransactionID:" + newTransaction.ID + " Balance:" + account.Balance;
                }
                else
                    message = "  Transaction Failed";
            }
            else
            {
                message = "  No suffcient balance";
            }
            return message;
        }

        // Depositing amount to account.
        public string Deposit(string accountID,float amount)
        {
            string message;
            Models.Transaction.Transaction newTransaction;
            Account account;

            if (amount <= 0)
            {
                message = "  Enter only possitive value for the amount";
                return message;
            }
            // Creating Transaction.
            newTransaction = new Models.Transaction.Transaction()
            {
                CreatedOn = DateTime.Now,
                IsReverted = false,
                Type = Models.Transaction.TransactionType.Deposit,
                Amount = amount,
                PayeeID = accountID,
            };

            // Adding Transaction
            _transactionService.AddTransaction(newTransaction);
            // Updating balance of the account.
            account = _dataProvider.GetObjectByID<Account>(accountID);
            account.Balance += amount;
            if (_dataProvider.UpdateObject<Account>(account))
                message = "  TransactionID:" + newTransaction.ID + " Balance:" + account.Balance;
            else
                message = "  Transaction Failed";
            return message;

        }

        // Service for tranfering 
        public string Transfer(string debitingAccountID, string creditingAccountID, float amount)
        {
            Models.Transaction.Transaction newTransaction;
            Account creditingAccount, debitingAccount;
            string message;
            bool IsSufficientBalance = IsBalanceSufficient(debitingAccountID, amount);

            creditingAccount = _dataProvider.GetObjectByID<Account>(creditingAccountID);
            if (creditingAccount == null)
                return "creditingAccountID is not valid";
            debitingAccount = _dataProvider.GetObjectByID<Account>(debitingAccountID);
            if (debitingAccount == null)
                return "DebitingAccountID is not valid";
            //Check Wheather sufficient balance in the account.
            if (IsSufficientBalance)
            {
                newTransaction = new Models.Transaction.Transaction()
                {

                    CreatedOn = DateTime.Now,
                    IsReverted = false,
                    Type = Models.Transaction.TransactionType.Transfer,
                    Amount = amount,
                    PayeeID = creditingAccountID,
                    PayorID = debitingAccountID
                };

                // Adding Transaction.
                _transactionService.AddTransaction(newTransaction);
                // Updating balance for the credited account.
                creditingAccount.Balance += amount;
                // Updating balance fot the debited account.
                debitingAccount.Balance -= amount;
                // saving those Updated accounts in JSON.
                if (_dataProvider.UpdateObject<Account>(debitingAccount) && _dataProvider.UpdateObject<Account>(creditingAccount))
                    message = "  TransactionID:" + newTransaction.ID + " Balance:" + debitingAccount.Balance;
                else
                    message = "  Transaction Failed";
            }
            else
            {
                message = "  Unsufficient Balance";
            }
            return message;
        }


        // Check Wheather Sufficient Balance in the Account.
        public bool IsBalanceSufficient(string accountID, float amount)
        {
            Account account = _dataProvider.GetObjectByID<Account>(accountID);
            if (account == null)
                return false;
            return account.Balance >= amount;
        }

        // Returns all accountID's of a user.
        public IList<string> GetAccountsbyUserID(string userID)
        {
            IList<Account> accounts = _dataProvider.GetAllObjects<Account>();

            if (accounts == null)
                return default;
            else
            {
                try
                {
                    return accounts.Where(account => account.UserID == userID && account.IsActive == true).Select(p => p.ID).ToList();
                }
                catch
                {
                    return default;
                }
            }
        }

        // Update balance of the accounts after reverting transaction.
        public string UpdatebalanceAfterReverting(string creditedAccountID, string debitedAccountID, float amount)
        {
            Account creditedAccount, debitedAccount;
            string message;

            creditedAccount = _dataProvider.GetObjectByID<Account>(creditedAccountID);
            if (creditedAccount == null)
                return "Reverting Failed";
            creditedAccount.Balance -= amount;
            // Updating balance fot the debited account.
            debitedAccount = _dataProvider.GetObjectByID<Account>(debitedAccountID);
            if (debitedAccount == null)
                return "Reverting Failed";
            creditedAccount.Balance -= amount;
            // saving those Updated accounts in JSON.
            if (_dataProvider.UpdateObject<Account>(debitedAccount) && _dataProvider.UpdateObject<Account>(creditedAccount))
                message = "  Transaction Reverted";
            else
                message = "  Reverting Failed";
            return message;
        }

        // Adding an account for customer.
        public string AddAccount(string customerID)
        {
            string message;

            Account newAccount = new Account()
            {
                CreatedBy = _userID,
                CreatedOn = DateTime.Now,
                UserID = customerID,
                Balance = 0,
                IsActive = true,
            };
            message = _dataProvider.AddObject<Account>(newAccount);
            return message;
        }

        // Deleting account of the customer.
        public string DeleteAccount(string ID)
        {
            string message;
            Account account;

            try
            {
                account = _dataProvider.GetObjectByID<Account>(ID);
                account.ModifiedBy = _userID;
                account.ModifiedOn = DateTime.Now;
                account.IsActive = false;
                if (!_dataProvider.UpdateObject<Account>(account))
                    message = "  Deletion Failed";
                else
                    message = "  Deletion Successful";
            }
            catch
            {
                message = "  Deletion failed";
            }
            return message;
        }

        // Determine wheather AccountID exists.
        public bool IsAccountExists(string ID)
        {
            Models.Account account;
            account = _dataProvider.GetObjectByID<Models.Account>(ID);
            if (account != null && account.IsActive == true)
            {
                return true;
            }
            else
                return false;
           
        }

    }
}
