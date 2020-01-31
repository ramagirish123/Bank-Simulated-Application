using System;
using System.Collections.Generic;
using System.Text;
using BankApplication.Services;
using BankApplication.Models;
using System.IO;
using BankApplication.Data;
using System.Linq;

namespace BankApplication.UI
{
    public class App
    {
        private BankApplication.Models.Bank _currentBank;
        private Authentication.Login _userAuthentication;
        private Authentication.SignUp _userSignUp;
        private Bank _bankUI;
        private string _JsonFilePath;
        private Utility.UIStyling _UIStyling;
        private UIBuffer _UIBuffer;
       
        public App(string JsonFilePath)
        {
            _UIStyling = new Utility.UIStyling(100, 50);
            _UIBuffer = new UIBuffer();
            _userSignUp = new Authentication.SignUp(JsonFilePath, _UIStyling, _UIBuffer);
            _bankUI = new Bank(JsonFilePath, _UIStyling, _UIBuffer);
            _JsonFilePath = JsonFilePath;
            _userAuthentication = new Authentication.Login(JsonFilePath, _UIStyling, _UIBuffer);
        }

         public void Run()
        {
            Models.User.User AuthenticatedUser;
            string PressedKey;
            int selectedOperation;
         
            // UI for user logging or exiting from the application.
            while(true)
            {
                try
                {
                    _currentBank = _bankUI.DisplayAndSelectBank();

                }
                catch
                {
                    _UIBuffer.WriteLine("Failed to Log JSON FILE ERROR MESSAGE: ");
                    return;
                }
                while (true)
                {
                    _UIBuffer.WriteLine(_UIStyling.PadBoth("Welcome to ", '*'));
                    _UIBuffer.WriteLine(_UIStyling.PadBoth(_currentBank.Name, '*'));
                    _UIBuffer.WriteLine("  Press 1 to Login");
                    _UIBuffer.WriteLine("  Press 2 to Signup");
                    _UIBuffer.WriteLine("  Press 3 to Goback");
                    _UIBuffer.WriteLine("  Press 4 to Exit\n");
                    PressedKey = _UIBuffer.ReadLine();
                    _UIBuffer.WriteLine("\n");
                    if(PressedKey=="3")
                    {
                        _UIBuffer.Clear();
                        break;
                    }
                    switch (PressedKey)
                    {
                        case "1":
                            // Iterate the loop till user enter current correct details.
                            while (true)
                            {
                                AuthenticatedUser = _userAuthentication.Authenticate(_currentBank.ID);
                                if (AuthenticatedUser == null)
                                {
                                    _UIBuffer.WriteLine("  Wrong Credentials");
                                    continue;
                                }
                                else
                                {
                                    _UIBuffer.WriteLine("  WELCOME TO " + AuthenticatedUser.Name);
                                    break;
                                }
                            }
                            // If the user is staff then staff UI will be displayed or else customer UI will be displayed.
                            if (AuthenticatedUser.Type == Models.User.UserType.StaffMember)
                            {
                                while (true)
                                {
                                    _UIBuffer.WriteLine("  Press 1 for Staff Member Operations");
                                    _UIBuffer.WriteLine("  Press 2 for Customer Operations");
                                    _UIBuffer.WriteLine("  Press 3 to GoBack");
                                    if (int.TryParse(_UIBuffer.ReadLine(), out selectedOperation))
                                    {
                                        if(selectedOperation==3)
                                        {
                                            break;
                                        }
                                        switch (selectedOperation)
                                        {
                                            case 1:
                                                _UIBuffer.Clear();
                                                new BankApplication.UI.User.StaffMember.StaffMemberUI(AuthenticatedUser.ID, AuthenticatedUser.Name, _currentBank.ID, _JsonFilePath, _UIStyling, _UIBuffer).run();
                                                _UIBuffer.Clear();
                                                break;
                                            case 2:
                                                _UIBuffer.Clear();
                                                new BankApplication.UI.User.Customer.CustomerUI(AuthenticatedUser.ID, AuthenticatedUser.Name, _currentBank.ID, _JsonFilePath, _UIStyling, _UIBuffer).run();
                                                _UIBuffer.Clear();
                                                break;
                                            default:
                                                _UIBuffer.WriteLine("Enter numbers between 1 and 3");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        _UIBuffer.WriteLine("Enter only numeric values");
                                    }
                                    
                                }       
                            }
                            else
                            {
                                _UIBuffer.Clear();
                                new BankApplication.UI.User.Customer.CustomerUI(AuthenticatedUser.ID,AuthenticatedUser.Name, _currentBank.ID, _JsonFilePath, _UIStyling, _UIBuffer).run();
                                _UIBuffer.Clear();
                            }
                            break;
                        case "2":
                            _userSignUp.SignUpRequest(_currentBank.ID, _JsonFilePath);
                            break;
                        case "4":
                            return;
                        default:
                            _UIStyling.ChangeForegroundForErrorMessage();
                            _UIBuffer.WriteLine("Pressed incorrect key\n");
                            _UIStyling.RestoreForegroundColor();
                            break;

                    }
                }
               

            }
          
            
        }
    }
}
