using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using BankApplication.Models;
using System.Linq;
using System.Collections;
using BankApplication.Data;

namespace BankApplication.Services
{
    public class TransactionService : Interfaces.ITransactionService
    {
        private DataProvider _dataProvider;
        private string _userID;

        public TransactionService(string userID,string JsonFilePath)
        {
            _dataProvider = new DataProvider(JsonFilePath);
            _userID = userID;
        }


        // Adding new transaction to JSON FILE.
        public string AddTransaction(Models.Transaction.Transaction newTransaction)
        {
            string ID;

            ID = _dataProvider.AddObject<Models.Transaction.Transaction>( newTransaction);
            return string.IsNullOrEmpty(ID) ? string.Empty : ID;
        }

        // Get all Transactions of an Account.
        public IList<Models.Transaction.Transaction> GetTransactionsByAccountID(string accountID)
        {
            IList<Models.Transaction.Transaction> transactions = _dataProvider.GetAllObjects<Models.Transaction.Transaction>();

            try
            {
                return transactions.Where(p => (p.PayeeID == accountID || p.PayorID == accountID) && p.IsReverted == false).ToList();
            }
            catch
            {
                return default;
            }
        }


        // Retrive all transfer transactions of an account.
        public IList<Models.Transaction.Transaction> GetTransferTransactionsByAccountID(string ID)
        {
            IList<Models.Transaction.Transaction> transactions;

            try
            {
                transactions = _dataProvider.GetAllObjects<Models.Transaction.Transaction>();
            }
            catch
            {
                return default;
            }
            return transactions.Where(p => p.IsReverted == false && p.Type == Models.Transaction.TransactionType.Transfer && (p.PayeeID == ID || p.PayorID == ID)).ToList();
        }

        // Delete a transaction.
        public bool RevertTransactionByID(string ID)
        {
            Models.Transaction.Transaction transaction;
            try
            {
                transaction = _dataProvider.GetObjectByID<Models.Transaction.Transaction>(ID);
                transaction.IsReverted = true;
                transaction.ModifiedOn = DateTime.Now;
                transaction.ModifiedBy = _userID;
                return _dataProvider.UpdateObject<Models.Transaction.Transaction>( transaction);
            }
            catch
            {
                return false;
            }


        }
    }
}
