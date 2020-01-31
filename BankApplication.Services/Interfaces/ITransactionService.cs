using BankApplication.Models.Transaction;
using System.Collections.Generic;

namespace BankApplication.Services.Interfaces
{
    public interface ITransactionService
    {
        string AddTransaction(Transaction newTransaction);
        bool RevertTransactionByID(string ID);
        IList<Transaction> GetTransactionsByAccountID(string ID);
        IList<Transaction> GetTransferTransactionsByAccountID(string ID);
    }
}