using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BankApplication.Models;
using BankApplication.Models.User;
using BankApplication.Services;
namespace BankApplication.UI.Authentication
{
   public class Login
    {
        private UserService _userService;
        private Utility.UIStyling _UIStyling;
        private Utility.PasswordHider _passwordHider;
        private AuthenticationService _authenticationService;
        private string _JsonFilePath;
        private Utility.DataValidations _dataValidations;
        private UIBuffer _UIBuffer;

        public Login(string JsonFilePath, Utility.UIStyling UIStyling, UIBuffer UIBuffer)
        {
            _UIStyling = UIStyling;
            _UIBuffer = UIBuffer;
            _passwordHider = new Utility.PasswordHider(_UIBuffer);
            _authenticationService = new AuthenticationService(JsonFilePath);
            _JsonFilePath = JsonFilePath;
            _dataValidations = new Utility.DataValidations();  
        }
        
      public Models.User.User Authenticate(string bankID)
         {
            string EmailID,Password,ID;
            bool IsEMailValid;

            do
            {
                Console.Write("  Enter EmailID: ");
                EmailID = Console.ReadLine();
                if (_dataValidations.IsEMailValid(EmailID))
                    break;
                _UIStyling.ChangeForegroundForErrorMessage();
                Console.WriteLine("  Enter valid Email ID");
                _UIStyling.RestoreForegroundColor();
            } while (true);

            Console.WriteLine("");
            Console.Write("  Enter Password: ");
            Password = _passwordHider.ReadPassword();
            Console.WriteLine("");
            ID = _authenticationService.AuthenticateUser(EmailID, Password, bankID);
            if(ID=="")
            {
                return null;
            }
            else
            {
                return new UserService(ID, _JsonFilePath).GetUserByID(ID);
            }
         }

      
    }
}
