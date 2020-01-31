using System;
using System.Collections.Generic;
using System.Text;
using BankApplication.Services;

namespace BankApplication.UI.User.StaffMember
{
    public class StaffMemberUI
    {
        private Services.Interfaces.IBankService _bankService;
        private Services.Interfaces.IUserService _userService;
        private Services.Interfaces.IAccountService _accountService;
        private Services.Interfaces.ITransactionService _transactionService;
        private Services.Interfaces.IExchangeRateService _exchangeRateService;
        private Utility.UIStyling _UIStyling;
        private string _userID;
        private string _bankID;
        private int _selectedOperation;
        private string _userName;
        private Utility.DataValidations _dataValidations;
        private UIBuffer _UIBuffer;

        public StaffMemberUI(string userID,string userName ,string bankID, string JsonFilepath,Utility.UIStyling UIStyling,UIBuffer UIBuffer)
        {
            this._userID = userID;
            this._bankID = bankID;
            _userName = userName;
            _UIBuffer = UIBuffer;
            _bankService = new BankService(userID,JsonFilepath);
            _userService = new UserService(userID,JsonFilepath);
            _accountService = new AccountService(userID,JsonFilepath);
            _transactionService = new TransactionService(userID,JsonFilepath);
            _exchangeRateService = new ExchangeRateService(JsonFilepath);
            _UIStyling = UIStyling;
            _dataValidations = new Utility.DataValidations();
        }

        // Staff member UI start running
        public void run()
        {

            while (true)
            {
                _UIBuffer.WriteLine("  Welcome " + _userName);
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
                                ChangePendingLoginRequestStatus();
                                _UIBuffer.Clear();
                                break;
                            }
                        case 2:
                            {
                                CustomerOperations();
                                _UIBuffer.Clear();
                                break;
                            }
                        case 3:
                            {
                                BankOperations();
                                _UIBuffer.Clear();
                                break;
                            }
                        case 4:
                            {
                                TransactionOperations();
                                _UIBuffer.Clear();
                                break;
                            }
                     
                        default:
                            {
                                _UIStyling.ChangeForegroundForErrorMessage();
                                _UIBuffer.WriteLine("  press only numbers from 1-5");
                                _UIStyling.RestoreForegroundColor();
                                break;
                            }

                    }
                }
                else
                {
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Press only numeric values.");
                    _UIStyling.RestoreForegroundColor();
                }
            }
        }

        // Accepting or Decline pending user request
        private void ChangePendingLoginRequestStatus()
        {
            string selectedRequestID;
            int selectedOperation;
            string reason;

            selectedRequestID = DisplayAndSelectPendingLoginRequests();
            // If there are no pending requests the exit this function.
            if (selectedRequestID == null)
                return;
            while(true)
            {
                _UIBuffer.WriteLine("  Press number corresponding to the operation");
                _UIBuffer.WriteLine("  1. Accept");
                _UIBuffer.WriteLine("  2. Decline");
                if (int.TryParse(_UIBuffer.ReadLine(), out selectedOperation))
                {
                    if (selectedOperation > 0 && selectedOperation <= 2)
                    {
                        switch (selectedOperation)
                        {
                            case 1:
                                _userService.AcceptUserRequestByID(selectedRequestID);
                                _accountService.AddAccount(selectedRequestID);
                                _UIBuffer.WriteLine("  Successfully accepted the request.");
                                break;
                            case 2:
                                _UIBuffer.WriteLine("  Enter the reason for declining.");
                                reason = _UIBuffer.ReadLine();
                                _userService.DeclineUserRequestByID(selectedRequestID, reason);
                                _UIBuffer.WriteLine("  Successfully declined the request.");
                                break;
                        }
                        break;
                    }

                    else
                    {
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("  Press only numeric values between 1 and 2.");
                        _UIStyling.RestoreForegroundColor();
                    }
                }
                else
                {
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Press only numeric values.");
                    _UIStyling.RestoreForegroundColor();
                }
            }
        }

        // Display Pending user requests
        private string DisplayAndSelectPendingLoginRequests()
        {
            int RequestedUsersCount;
            int index = 0;
            int selectedRequestIndex;
            IList<Models.User.User> requestedUsers = _userService.GetRequestedUsersOfBank(_bankID);
            RequestedUsersCount = requestedUsers.Count;
            if (RequestedUsersCount == 0)
            {
                return null;
            }
            else
            {
                while (true)
                {
                    index = 0;
                    _UIBuffer.WriteLine("  Press Number corresponding to the user");
                    _UIBuffer.WriteLine("  ------------------------------------------------------------------------");
                    _UIBuffer.WriteLine(string.Format("  {0,-4}   {1,-50}  {2,-10}", "S.No","ID","Name"));
                    while (index < RequestedUsersCount)
                    {
                        _UIBuffer.WriteLine(string.Format("  {0,-4}  {1,-50}  {2,-10}", index + 1, requestedUsers[index].ID, requestedUsers[index].Name));
                        index++;
                    }
                    _UIBuffer.WriteLine("  -------------------------------------------------------------------------");
                    _UIBuffer.WriteLine("  Press 0 to go back");
                    if (int.TryParse(_UIBuffer.ReadLine(), out selectedRequestIndex))
                    {
                        if(selectedRequestIndex==0)
                        {
                            return null;
                        }
                        if (selectedRequestIndex > 0 && selectedRequestIndex <= (RequestedUsersCount))
                        {
                            return requestedUsers[selectedRequestIndex - 1].ID;
                        }
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("  Enter between 1 and " + RequestedUsersCount);
                        _UIStyling.RestoreForegroundColor();
                    }
                    else
                    {
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("  Press only numeric values.");
                        _UIStyling.RestoreForegroundColor();
                    }
                }
            }
        }

        // Perform customer related operations for the user.
        private void CustomerOperations()
        {
            int selectedOperation;

            while(true)
            {
                DisplayCustomerOperations();
                if(int.TryParse(_UIBuffer.ReadLine(),out selectedOperation))
                {
                    if(selectedOperation==4)
                    {
                        break;
                    }
                    switch(selectedOperation)
                    {
                        case 1:
                            CreateUser();
                            break;
                        case 2:
                            UpdateUser();
                            break;
                        case 3:
                            DeleteUser();
                            break;
                        default:
                            {
                                _UIStyling.ChangeForegroundForErrorMessage();
                                _UIBuffer.WriteLine("  press only numbers from 1-4");
                                _UIStyling.RestoreForegroundColor();
                                break;
                            }
                    }
                }
                else
                {
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Press only numbers");
                    _UIStyling.RestoreForegroundColor();
                }
                        
            }
        }

        // Display Customer related operations for the user.
        private void DisplayCustomerOperations()
        {
            _UIBuffer.WriteLine("  Press a number corresponding to that operation");
            _UIBuffer.WriteLine("  1. Create Customer");
            _UIBuffer.WriteLine("  2. Update Customer");
            _UIBuffer.WriteLine("  3. Delete Cusotmer");
            _UIBuffer.WriteLine("  4. --Go Back--");
        }

        // Bank related operations that a staff can perform.
        private void BankOperations()
        {
            int selectedOperation;

            while (true)
            {
                DisplayBankOperations();
                if (int.TryParse(_UIBuffer.ReadLine(), out selectedOperation))
                {
                    if (selectedOperation == 6)
                    {
                        break;
                    }
                    switch (selectedOperation)
                    {
                        case 1:
                            ChangeRtgsOfSameBank();
                            break;
                        case 2:
                            ChangeImpsOfSameBank();
                            break;
                        case 3:
                            ChangeRtgsOfotherBank();
                            break;
                        case 4:
                            ChangeImpsOfOtherBank();
                            break;
                        case 5:
                            AddExchangeRateToBank();
                            break;
                        default:
                            {
                                _UIStyling.ChangeForegroundForErrorMessage();
                                _UIBuffer.WriteLine("  press only numbers from 1-6");
                                _UIStyling.RestoreForegroundColor();
                                break;
                            }
                    }
                }
                else
                {
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  press only numbers");
                    _UIStyling.RestoreForegroundColor();
                }

            }
        }

        // Display Bank related operations.
        private void DisplayBankOperations()
        {
            _UIBuffer.WriteLine("  Press a number corresponding to that operation");
            _UIBuffer.WriteLine("  1. Update RTGS charges for the same bank transactions");
            _UIBuffer.WriteLine("  2. Update IMPS charges for the same bank transactions");
            _UIBuffer.WriteLine("  3. Update RTGS charges for the other bank transactions");
            _UIBuffer.WriteLine("  4. Update IMPS charges for the other bank transactions");
            _UIBuffer.WriteLine("  5. Add Exchange rate to the bank");
            _UIBuffer.WriteLine("  6. --Go Back--");
        }
        // Display operations that a staff can perform.
        private void DisplayOperations()
        {
            int numberOfRequests = _userService.GetRequestedUsersCount(_bankID);
            _UIBuffer.WriteLine("  Press a number corresponding to that operation");
            _UIBuffer.WriteLine("  1. Pending User Requests "+numberOfRequests);
            _UIBuffer.WriteLine("  2. Customer Operations");
            _UIBuffer.WriteLine("  3. Bank Operations");
            _UIBuffer.WriteLine("  4. Transaction Operations");
            _UIBuffer.WriteLine("  5. Logout");
        }

        // Transaction related operations that can be performed by staff.
        private void TransactionOperations()
        {
            int selectedOperation;

            while (true)
            {
                DisplayTranactionOperations();
                if (int.TryParse(_UIBuffer.ReadLine(), out selectedOperation))
                {
                    if (selectedOperation == 3)
                    {
                        break;
                    }
                    switch (selectedOperation)
                    {
                        case 1:
                            RevertTransferTranaction();
                            break;
                        case 2:
                            ViewTransactionsofCustomer();
                            break;
                        default:
                            {
                                _UIStyling.ChangeForegroundForErrorMessage();
                                _UIBuffer.WriteLine("  press only numbers from 1-3");
                                _UIStyling.RestoreForegroundColor();
                                break;
                            }
                    }
                }
                else
                {
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  press only numbers");
                    _UIStyling.RestoreForegroundColor();
                }

            }
        }

        // Display transaction related operations.
        private void DisplayTranactionOperations()
        {

            _UIBuffer.WriteLine("  Press a number corresponding to that operation");
            _UIBuffer.WriteLine("  1. Revert Tranaction");
            _UIBuffer.WriteLine("  2. View Transaction history");
            _UIBuffer.WriteLine("  3. --Go Back--");
        }
        // Creating new account.
        private void CreateUser()
        {
            Models.User.User user = new Models.User.User();
            string ID;
            string accountID;
            bool IsEMailAvailable;
            bool IsEMailValid;

            _UIBuffer.Write("  Enter Name of the user: ");
            user.Name = _UIBuffer.ReadLine();
            _UIBuffer.Write("  Enter Password of the user: ");
            user.Password = new Utility.PasswordHider(_UIBuffer).ReadPassword();
            _UIBuffer.Write("  Enter Mobile number of the user: ");
            user.MobileNumber = _UIBuffer.ReadLine();
            while (true)
            {
                do
                {
                    _UIBuffer.Write("  Enter email of the customer: ");
                    user.EMail = _UIBuffer.ReadLine();
                    IsEMailValid = _dataValidations.IsEMailValid(user.EMail);
                    if (IsEMailValid)
                        break;
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Enter a valid EMail");
                    _UIStyling.RestoreForegroundColor();

                } while (true);
                IsEMailAvailable = _userService.CheckEMailAvailbility(user.EMail, _bankID);
                if(IsEMailAvailable)
                {
                    user.BankID = _bankID;
                    ID = _userService.AddUser(user);
                    _UIBuffer.WriteLine("  "+ID + " is new userID\n");
                    accountID=_accountService.AddAccount(ID);
                    _UIBuffer.WriteLine("  "+accountID + " is the default account opened for the customer");
                    break;
                }
                else
                {
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Email is not available.Please enter another EMail ID");
                    _UIStyling.RestoreForegroundColor();
                }
                
            }
            
        }

        // Updating account.
        private void UpdateUser()
        {
            Models.User.User user= new Models.User.User();
            string selectedCustomerID;
            int selectedOperation;
            bool IsEMailValid;
            string createdAccountID;
            string selectedAccountID;

            selectedCustomerID = DisplayAndSelectCustomerID();

            if(selectedCustomerID == String.Empty)
            {
                return;
            }
            if (selectedCustomerID == null)
            {
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("  There are no customers");
                _UIStyling.RestoreForegroundColor();
                return;
            }
            selectedOperation = DisplayCustomerUpdateOperations();
            switch (selectedOperation)
            {
                case 1:
                    _UIBuffer.Write("  Enter new name of the customer: ");
                    user.Name = _UIBuffer.ReadLine();
                    _userService.UpdateCustomerNameByID(selectedCustomerID, user);
                    _UIBuffer.WriteLine("  Successfully Updated");
                    _UIBuffer.ReadKey();
                    break;
                
                case 2:
                    do
                    {
                        _UIBuffer.WriteLine("  Enter new email of the customer");
                        user.Name = _UIBuffer.ReadLine();
                        IsEMailValid = _dataValidations.IsEMailValid(user.EMail);
                        if (IsEMailValid)
                            break;
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("  Enter a valid EMail");
                        _UIStyling.RestoreForegroundColor();

                    } while (true);
                    user.EMail = _UIBuffer.ReadLine();
                    _userService.UpdateCustomerEMailByID(selectedCustomerID, user);
                    _UIBuffer.WriteLine("  Successfully Updated");
                    _UIBuffer.ReadKey();
                    break;
                case 3:
                    _UIBuffer.WriteLine("  Enter new mobileNumber of the customer: ");
                    user.MobileNumber = _UIBuffer.ReadLine();
                    _userService.UpdateCustomerMobileNumberByID(selectedCustomerID, user);
                    _UIBuffer.WriteLine("  Successfully Updated");
                    _UIBuffer.ReadKey();
                    break;
                case 4:
                    createdAccountID=_accountService.AddAccount(selectedCustomerID);
                    _UIBuffer.WriteLine("  New Account Succeddfully\n");
                    _UIBuffer.WriteLine("  AccountID" + createdAccountID);
                    break;
                case 5:
                    selectedAccountID = DisplayAndSelectAccountOfCustomer(selectedCustomerID);
                    _accountService.DeleteAccount(selectedAccountID);
                    break;
            }

        }

        // Deleting Customer.
        private void DeleteUser()
        {
            string selectedUserID;
            string message;

            selectedUserID = DisplayAndSelectCustomerID();
            if (selectedUserID == String.Empty)
                return;
            message=_userService.DeleteUserByID(selectedUserID);
            _UIBuffer.WriteLine("  "+message);
            _UIBuffer.ReadKey();

        }

        // Change RTGS charges of Same Bank Transactions.
        private void ChangeRtgsOfSameBank()
        {
            float newRtgs;

            _UIBuffer.Write("  Enter RTGS charge: ");
            while (true)
            {
                if (float.TryParse(_UIBuffer.ReadLine(), out newRtgs))
                {
                    _bankService.UpdateRtgsOfSameBank(_bankID,newRtgs);
                    _UIBuffer.WriteLine("  Successfully Updated");
                    _UIBuffer.ReadKey();
                    break;
                }
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("  Enter only numerics");
                _UIStyling.RestoreForegroundColor();
            }

        }

        // Change IMPS charges of Same Bank Transactions.
        private void  ChangeImpsOfSameBank()
        {
            float newImps;

            _UIBuffer.Write("  Enter IMPS charge: ");
            while (true)
            {
                if (float.TryParse(_UIBuffer.ReadLine(), out newImps))
                {
                    _bankService.UpdateImpsOfSameBank(_bankID, newImps);
                    _UIBuffer.WriteLine("  Successfully Updated");
                    _UIBuffer.ReadKey();
                    break;
                }
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("  Enter only numerics");
                _UIStyling.RestoreForegroundColor();
            }
        }

        // Change RTGS charges of Other Bank Transactions.
        private void ChangeRtgsOfotherBank()
        {
            float newRtgs;

            _UIBuffer.Write("  Enter RTGS charge: ");
            while (true)
            {
                if (float.TryParse(_UIBuffer.ReadLine(), out newRtgs))
                {
                    _bankService.UpdateRtgsOfOtherBank(_bankID,newRtgs);
                    _UIBuffer.WriteLine("  Successfully Updated");
                    _UIBuffer.ReadKey();
                    break;
                }
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("  Enter only numerics");
                _UIStyling.RestoreForegroundColor();
            }
        }

        // Change IMPS charges of Other Bank Transactions.
        private void ChangeImpsOfOtherBank()
        {
            float newImps;

            _UIBuffer.Write("  Enter IMPS charge: ");
            while (true)
            {
                if (float.TryParse(_UIBuffer.ReadLine(), out newImps))
                {
                    _bankService.UpdateImpsOfOtherBank(_bankID,newImps);
                    _UIBuffer.WriteLine("  Successfully Updated");
                    _UIBuffer.ReadKey();
                    break;
                }
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("  Enter only numerics");
                _UIStyling.RestoreForegroundColor();
            }
        }

        // Reverting transaction.
        private void RevertTransferTranaction()
        {
            IList<Models.Transaction.Transaction> transactions;
            string customerID;
            string accountID;
            int index = 0;
            int transactionsCount;
            int selectedTransactionIndex;

            customerID = DisplayAndSelectCustomerID();
            if (customerID == String.Empty || customerID == null)
                return;
            accountID = DisplayAndSelectAccountOfCustomer(customerID);
            if (accountID == String.Empty || accountID == null)
                return;
            transactions = _transactionService.GetTransferTransactionsByAccountID(accountID);
            transactionsCount = transactions.Count;
            if (transactionsCount == 0)
            {
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("  There are no transactions");
                _UIStyling.RestoreForegroundColor();
            }
            else
            {
                while (true)
                {
                    index = 0;
                    _UIBuffer.WriteLine("  Press Number corresponding to the transactions");
                    _UIBuffer.WriteLine("\n--------------------------------------------------------------");
                    _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-40}", "S.NO", "TransactionID"));
                    while (index < transactionsCount)
                    {
                        _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-40}",(index + 1),transactions[index].ID));
                        index++;
                    }
                    _UIBuffer.WriteLine("-------------------------------------------------------------------\n");
                    _UIBuffer.WriteLine("  Enter 0 to GoBack");
                    if (int.TryParse(_UIBuffer.ReadLine(), out selectedTransactionIndex))
                    {
                        if(selectedTransactionIndex==0)
                        {
                            return;
                        }
                        if (selectedTransactionIndex >= 1 && selectedTransactionIndex <= (transactionsCount))
                        {
                            if (_transactionService.RevertTransactionByID(transactions[selectedTransactionIndex - 1].ID))
                                _UIBuffer.WriteLine("  Successfully Reverted");
                            else
                                _UIBuffer.WriteLine(" Revertion Failed");
                                _UIBuffer.ReadKey();
                            break;
                        }
                        else
                        {
                            _UIStyling.ChangeForegroundForErrorMessage();
                            _UIBuffer.WriteLine("  Enter between 1 and " + transactionsCount);
                            _UIStyling.RestoreForegroundColor();
                        }
                    }
                }
            }
        }

        // Display all transactions of Account
        public void ViewTransactionsofCustomer()
        {
            IList<Models.Transaction.Transaction> transactions;
            string customerID;
            string accountID;
            int index=0;

            customerID = DisplayAndSelectCustomerID();
            if (customerID == String.Empty || customerID == null)
                return;
            accountID = DisplayAndSelectAccountOfCustomer(customerID);
            if (accountID == String.Empty || accountID == null)
                return;
            transactions = _transactionService.GetTransactionsByAccountID(accountID);
            _UIBuffer.WriteLine("\n-------------------------------------------------------------------------------------------------------------------------------");
            _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-40}  {2,-40}  {3,-40}  {4,-7}  {5,-7}", "S.NO", "Transaction ID", "Payor", "Payee", "Amount", "Type"));
            foreach (Models.Transaction.Transaction transaction in transactions)
            {
                _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-40}  {2,-40}  {3,-40}  {4,-7}  {5,-7}",index,transaction.ID,(transaction.PayeeID ?? "------------------------------------"),(transaction.PayorID ?? "-------------------------------------" + " "),transaction.Amount,transaction.Type));
                index++;
            }
            _UIBuffer.WriteLine("---------------------------------------------------------------------------------------------------------------------------------\n");
            _UIBuffer.ReadKey();
        }

        // Displays all UserID's and Prompt user to pick one userID
        private string DisplayAndSelectCustomerID()
        {
            int usersCount;
            int index;
            int selectedUserIndex;
            IList<Models.User.User> customers = _userService.GetAllCustomersOfBank(_userID, _bankID);

            usersCount = customers.Count;
            if (usersCount == 0)
            {
                return null;
            }
            else
            {
                while (true)
                {
                    index = 0;
                    _UIBuffer.WriteLine("  Press Number corresponding to the user");
                    _UIBuffer.WriteLine("\n-------------------------------------------------------------------");
                    _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-40}  {2,-20}", "S.NO", "CustomerName", "CustomerName"));
                    while (index < usersCount)
                    {
                        _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-40}  {2,-20}",(index + 1),customers[index].ID,customers[index].Name));
                        index++;
                    }
                    _UIBuffer.WriteLine("-------------------------------------------------------------------\n");
                    _UIBuffer.WriteLine("  Press 0 to go back");

                    if (int.TryParse(_UIBuffer.ReadLine(), out selectedUserIndex))
                        {
                            if(selectedUserIndex==0)
                            {
                                return string.Empty;
                            }
                            if (selectedUserIndex >= 1 && selectedUserIndex <= (usersCount))
                            {
                                return customers[selectedUserIndex - 1].ID;
                            }
                            _UIStyling.ChangeForegroundForErrorMessage();
                            _UIBuffer.WriteLine("  Enter between 1 and " + usersCount);
                            _UIStyling.RestoreForegroundColor();
                        }
                }
            }
        }

        // Prompt staff to pick one of selected customer account.
        private string DisplayAndSelectAccountOfCustomer(string ID)
        {
            int accountsCount;
            int index = 0;
            int selectedAccountIndex;
            IList<String> accountsList = _accountService.GetAccountsbyUserID(ID);

            accountsCount = accountsList.Count;
            if (accountsCount == 0)
            {
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("Selected Customer don't  have any accounts");
                _UIStyling.RestoreForegroundColor();
                return null;
            }
            else
            {
                while (true)
                {
                    index = 0;
                    _UIBuffer.WriteLine("  Press Number corresponding to the account");
                    _UIBuffer.WriteLine("\n-------------------------------------------------------------------");
                    _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-40}", "S.NO", "Account ID"));
                    while (index < accountsCount)
                    {
                        _UIBuffer.WriteLine(String.Format("  {0,-5}  {1,-40}",(index + 1),accountsList[index]));
                        index++;
                    }
                    _UIBuffer.WriteLine("-------------------------------------------------------------------\n");
                    _UIBuffer.WriteLine("  Press 0 to go back");
                    if (int.TryParse(_UIBuffer.ReadLine(), out selectedAccountIndex))
                    {
                        if (selectedAccountIndex == 0)
                            return String.Empty;
                        if (selectedAccountIndex >= 1 && selectedAccountIndex <= (accountsCount))
                        {
                            return accountsList[selectedAccountIndex - 1];
                        }
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("  Enter between 1 and " + accountsCount);
                        _UIStyling.RestoreForegroundColor();
                    }
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Enter Only numeric values");
                    _UIStyling.RestoreForegroundColor();
                }
            }
        }

        // Display update operations of a customer.
        private int DisplayCustomerUpdateOperations()
        {
            int selectedOperation;

            while(true)
            {
                _UIBuffer.WriteLine("  Enter number corresponding to operation");
                _UIBuffer.WriteLine("  1. Update Customer Name");
                _UIBuffer.WriteLine("  2. Update Customer EmailID");
                _UIBuffer.WriteLine("  3. Update Customer MobileNumber");
                _UIBuffer.WriteLine("  4. Add a new account for the customer");
                _UIBuffer.WriteLine("  5. Delete the account for the customer");
                if (int.TryParse(_UIBuffer.ReadLine(), out selectedOperation))
                {
                    if(selectedOperation>=1 && selectedOperation<=5)
                    {
                        return selectedOperation;
                    }
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Enter value between 1 to 5");
                    _UIStyling.RestoreForegroundColor();
                }
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("  Enter only numeric values");
                _UIStyling.RestoreForegroundColor();
            }
        }

        // Add a Exchange Rate to a bank
        public void AddExchangeRateToBank()
        {
            string name;
            float rate;
            bool IsExist;
            
            while(true)
            {
                _UIBuffer.Write("  Enter name of the exchange: ");
                name = _UIBuffer.ReadLine();
                // Reading the exchage rate and if non-numeric values was entered then system prompts user to enter the value again. 
                while (true)
                {
                    _UIBuffer.WriteLine("  Enter rate of the exchange in terms of INDIAN RUPEE: ");
                    if (float.TryParse(_UIBuffer.ReadLine(), out rate))
                    {
                        break;
                    }
                    else
                    {
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine("  Enter only numeric values");
                        _UIStyling.RestoreForegroundColor();
                        continue;
                    }

                }
                IsExist = _exchangeRateService.CheckExchangeRateExists(_bankID, name);
                if(!IsExist)
                {
                    _exchangeRateService.AddExchangeRate(_bankID, name, rate);
                    break;
                }
                else
                {
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Exchange Rate already exists");
                    _UIStyling.RestoreForegroundColor();
                    break;
                }

            }
        }


    }
}
