using System;
using System.Collections.Generic;
using System.Text;
using BankApplication.Services;

namespace BankApplication.UI.Authentication
{
    class SignUp
    {
        private UserService _userService;
        private string _bankID;
        private Utility.UIStyling _UIStyling;
        private Utility.PasswordHider _passwordHider;
        private Utility.DataValidations _dataValidations;
        private UIBuffer _UIBuffer;

        public SignUp(string JsonFilePath, Utility.UIStyling UIStyling, UIBuffer UIBuffer)
        {
            _UIBuffer = UIBuffer;
            _userService = new UserService("",JsonFilePath);
            _UIStyling = UIStyling;
            _passwordHider = new Utility.PasswordHider(_UIBuffer);
            _dataValidations = new Utility.DataValidations();
            
        }
        
        // Send request to join into the bank.
        public void SignUpRequest(string bankID,string JsonFilePath)
        {
            Models.User.User user=new Models.User.User();
            bool IsEMailAvailable;
            string ID;
           
            _UIBuffer.WriteLine();
            _UIBuffer.Write("  Enter your name: ");
            user.Name = _UIBuffer.ReadLine();
            _UIBuffer.WriteLine();
            _UIBuffer.Write("  Enter Password: ");
            user.Password = _passwordHider.ReadPassword();
            _UIBuffer.WriteLine();
            _UIBuffer.Write("  Enter phone number: ");
            user.MobileNumber = _UIBuffer.ReadLine();
            _UIBuffer.WriteLine();
            while (true)
            {
                do
                {
                    _UIBuffer.Write("  Enter EmailID: ");
                    user.EMail = _UIBuffer.ReadLine();
                    if (_dataValidations.IsEMailValid(user.EMail))
                        break;
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("  Enter valid Email ID");
                    _UIStyling.RestoreForegroundColor();
                } while (true);
                IsEMailAvailable = _userService.CheckEMailAvailbility(user.EMail, _bankID);
                if (IsEMailAvailable)
                {
                    user.BankID = _bankID;
                    ID = _userService.AddRequestedUser(user);
                    _UIBuffer.WriteLine(ID + " is your Request-ID");
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
    }
}
