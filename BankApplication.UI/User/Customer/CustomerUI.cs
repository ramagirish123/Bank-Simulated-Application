using System;
using System.Collections.Generic;
using System.Text;
using BankApplication.Services;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections;
using BankApplication.Data;

namespace BankApplication.UI.User.Customer
{
    // Customer UI.
    public class CustomerUI
    {

        private Services.Interfaces.IUserService _userService;
        private Services.Interfaces.IAccountService _accountService;
        private Services.Interfaces.ITransactionService _transactionService;
        private Services.Interfaces.IExchangeRateService _exchangeRateServices;
        private UI.Utility.UIStyling _UIStyling;
        private string _userID;
        private string _bankID;
        int _selectedOperation;
        string _selectedAccountID;
        string _userName;
        UIBuffer _UIBuffer;

        public CustomerUI(string userID,string userName,string bankID,string JsonFilePath, UI.Utility.UIStyling UIStyling,UIBuffer UIBuffer)
        {
            _userID = userID;
            _UIBuffer = UIBuffer;
            _bankID = bankID;
            _userService = new UserService(userID,JsonFilePath);
            _accountService = new AccountService(userID,JsonFilePath);
            _transactionService = new TransactionService(userID,JsonFilePath);
            _exchangeRateServices = new ExchangeRateService(JsonFilePath);
            _UIStyling = UIStyling;
            _userName = userName;
        }

       public void run()
        {
            while (true)
            {
                _UIBuffer.WriteLine("  Welcome " + _userName + "\n");
                DisplayOperations();
                if (int.TryParse(_UIBuffer.ReadLine(), out _selectedOperation))
                {
                    if (_selectedOperation == 5)
                    {
                        break;
                    }
                    switch (_selectedOperation)
                    {
                        case 1:
                            {
                                WithDraw();
                                break;
                            }
                        case 2:
                            {
                                Deposit();
                                break;
                            }
                        case 3:
                            {
                                Transfer();
                                break;
                            }
                        case 4:
                            {
                                ViewHistoryofAccount();
                                break;
                            }
                        default:
                            {
                                _UIBuffer.WriteLine("press only numbers from 1-4");
                                break;
                            }

                    }
                }
                else
                {
                    _UIBuffer.WriteLine("Press only numbers from 1-4");
                }
            }
        }
       private void DisplayOperations()
        {
            _UIBuffer.WriteLine("  Press a number corresponding to that operation");
            _UIBuffer.WriteLine("  1.Withdraw");
            _UIBuffer.WriteLine("  2.Deposit");
            _UIBuffer.WriteLine("  3.Transfer");
            _UIBuffer.WriteLine("  4.View History");
            _UIBuffer.WriteLine("  5.Logout\n");
            
        }

        // WithDraw amount from an account.
        private void WithDraw()
        {
            string message;

            _selectedAccountID = DisplayAndSelectAccountOfUser();
            if(_selectedAccountID==null)
            {
                return;
            }
            float amount;
            while (true)
            {
                _UIBuffer.Write("  Enter the amount in INR:");
                if (float.TryParse(_UIBuffer.ReadLine(), out amount))
                {
                    if (amount <= 0)
                    {
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("  Enter only possitive values");
                        _UIStyling.RestoreForegroundColor();
                        continue;
                    }
                    else
                    {
                        message=_accountService.WithDraw(_selectedAccountID, amount);
                        _UIBuffer.WriteLine(message);
                        break;
                    }
                }
                else
                {
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Enter only numeric values");
                    _UIStyling.RestoreForegroundColor();
                }
            }
        }


        // Deposit amount into an account.
        private void Deposit()
        {
            string message;

            float amount;
            float exchangeRate;

            _selectedAccountID = DisplayAndSelectAccountOfUser();
            if (_selectedAccountID == null)
            {
                return;
            }
            while (true)
            {
                _UIBuffer.WriteLine("  Enter the amount");
                if (float.TryParse(_UIBuffer.ReadLine(), out amount))
                {
                    if (amount <= 0)
                    {
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("  Enter only possitive values");
                        _UIStyling.RestoreForegroundColor();
                        continue;
                    }
                    else
                    {
                        exchangeRate = SelectExchangeRate();
                        message=_accountService.Deposit(_selectedAccountID, amount * exchangeRate);
                        _UIBuffer.WriteLine(message);
                        break;
                    }
                }
                else
                {
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Enter only numeric values");
                    _UIStyling.RestoreForegroundColor();
                }
            }
           
        }


        // Transfer amount from one account to other account.
        private void Transfer()
        {
            string message;

            float amount;
            string creditingAccount;
            bool IsValidAccount;

            _UIBuffer.WriteLine("  select the account from which you want to transfer\n");
            _selectedAccountID = DisplayAndSelectAccountOfUser();
            if (_selectedAccountID == null)
            {
                return;
            }
            _UIBuffer.Write("  Enter the account number to which you want to transfer: ");
            creditingAccount = _UIBuffer.ReadLine();
            IsValidAccount = _accountService.IsAccountExists(creditingAccount);
            if(IsValidAccount)
            {
                _UIBuffer.Write("  Enter the amount: ");
                while (true)
                {
                    if (float.TryParse(_UIBuffer.ReadLine(), out amount))
                    {
                        if (amount <= 0)
                        {
                            _UIStyling.ChangeForegroundForErrorMessage();
                            _UIBuffer.WriteLine("  Enter only possitive values");
                            _UIStyling.RestoreForegroundColor();
                            continue;
                        }
                        else
                        {
                            message=_accountService.Transfer(_selectedAccountID, creditingAccount,amount);
                            _UIBuffer.WriteLine(message);
                            break;
                        }
                    }
                    else
                    {
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("  Enter only numeric values");
                        _UIStyling.RestoreForegroundColor();
                    }
                }
            }
            else
            {
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("Invalid AccountID");
                _UIStyling.RestoreForegroundColor();

            }
           

        }

        // Display history of an account.
        private void ViewHistoryofAccount()
        {
            IList<Models.Transaction.Transaction> transactionsList;
            int index = 1;
            bool IsCreditingTransaction;

            _UIBuffer.WriteLine("  select the accountr\n");
            _selectedAccountID = DisplayAndSelectAccountOfUser();
            if (_selectedAccountID == null)
            {
                return;
            }

            transactionsList =_transactionService.GetTransactionsByAccountID(_selectedAccountID);
            _UIBuffer.WriteLine("--------------------------------------------------------------------------------\n");
            _UIBuffer.WriteLine(string.Format("  {0,-7}  {1,-8}  {2,-40}  {3,-8}  {4,-8}", "S.NO", "Date", "Transaction ID", "Amount", "Type"));
            foreach (Models.Transaction.Transaction transaction in transactionsList )
            {
                
                // Determining wheather transaction is Credit or Debit
                if (transaction.Type==Models.Transaction.TransactionType.Transfer)
                {
                    if(transaction.PayeeID== _selectedAccountID)
                    {
                        IsCreditingTransaction = true;
                    }
                    else
                    {
                        IsCreditingTransaction = false;
                    }
                }
                else
                {
                    if (transaction.Type == Models.Transaction.TransactionType.Deposit)
                    {
                        IsCreditingTransaction = true;
                    }
                    else
                    {
                        IsCreditingTransaction = false;
                    }
                }
                if (IsCreditingTransaction)
                {
                    _UIBuffer.Write(String.Format("  {0,-5}  {1,-8}  {2,-40}  {3,-8}", index, transaction.CreatedOn.ToString("dd/MM/yyyy"), transaction.ID, transaction.Amount));
                    _UIBuffer.Write(String.Format("{0,-8}", "Credit\n"));

                }
                else
                {
                    _UIBuffer.Write(String.Format("  {0,-5}  {1,-8}  {2,-40}  {3,-8}", index, transaction.CreatedOn.ToString("dd/MM/yyyy"), transaction.ID, transaction.Amount));
                    _UIBuffer.Write(String.Format("{0,-8}", "debit\n"));
                }
                index++;
            }
            _UIBuffer.WriteLine("--------------------------------------------------------------------------------\n");
        }


        // Prompt User to pick one of his accounts
       private string DisplayAndSelectAccountOfUser()
        {
            int accountsCount;
            int index;
            int selectedAccountIndex;
            IList<String> accountsList = _accountService.GetAccountsbyUserID(_userID);

            accountsCount = accountsList.Count();
            if(accountsCount==0)
            {
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("  You don't have any accounts\n");
                _UIStyling.RestoreForegroundColor();
                return null;
            }
            else
            {
                while(true)
                {
                    index = 0;
                    _UIBuffer.WriteLine("  Press Number corresponding to the account\n");
                    _UIBuffer.WriteLine("-----------------------------------------------");
                    _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-36}\n", "S.No", "AccountID"));
                    while(index<accountsCount)
                    {
                        _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-36}", (index + 1),accountsList[index]));
                        index++;
                    }
                    _UIBuffer.WriteLine("-----------------------------------------------\n");
                    _UIBuffer.WriteLine("  Enter " +(accountsCount+1) +" to go back" );
                    if (int.TryParse(_UIBuffer.ReadLine(), out selectedAccountIndex))
                    {
                        // User selecting go back option.
                        if (selectedAccountIndex == accountsCount + 1)
                        {
                            return default;
                        }
                        if (selectedAccountIndex >= 1 && selectedAccountIndex <= (accountsCount))
                        {
                            return accountsList[selectedAccountIndex - 1];
                        }
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("Enter between 1 and " + accountsCount);
                        _UIStyling.RestoreForegroundColor();
                    }
                    else
                    {
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("Enter Only numeric values");
                        _UIStyling.RestoreForegroundColor();
                    }
                }
            }
        }

        // Displaying exchange rates of bank and selecting one among them.
        private float SelectExchangeRate()
        {
            int exchangeRatesLength;
            int index=0;
            int selectedCurrency;
            IList<BankApplication.Models.ExchangeRate.ExchangeRate> rates = _exchangeRateServices.GetExchangeRatesByBankID(_bankID);
            exchangeRatesLength = rates.Count();
            _UIBuffer.WriteLine("  Select Currency you want to deposit\n");
            while (true)
            {
                index = 0;
                _UIBuffer.WriteLine("-----------------------------------------------");
                _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-12}  {2,-5}\n", "S.No", "Currency Name", "Rate"));
                while (index < exchangeRatesLength)
                {
                    _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-12}  {2,-5}\n",(index + 1),rates[index].CurrencyName, rates[index].Rate));
                    index++;
                }
                _UIBuffer.WriteLine("-----------------------------------------------");
                if (int.TryParse(_UIBuffer.ReadLine(),out selectedCurrency))
                {
                    if(selectedCurrency>=1&& selectedCurrency<=(exchangeRatesLength))
                    {
                        return rates[selectedCurrency-1].Rate;
                    }
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Enter between 1 and " + exchangeRatesLength);
                }
                _UIBuffer.WriteLine("  Enter Only numeric values");
            }
            

        }
    }

}
