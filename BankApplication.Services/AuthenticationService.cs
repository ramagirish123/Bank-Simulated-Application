using System;
using System.Collections.Generic;
using BankApplication.Data;
using System.Linq;

namespace BankApplication.Services
{
    public class AuthenticationService
    {
        private DataProvider _dataProvider;
        private Interfaces.Utility.IMdfHash _MdfHash;

        public AuthenticationService(string JsonFilePath)
        {
            _MdfHash = new Utility.MdfHash();
            _dataProvider = new DataProvider(JsonFilePath);
        }
        // Authenticating User basing on UserID.
        public string AuthenticateUser(string EmailID, string password, string bankID)
        {
          
            IList<Models.User.User> Users = _dataProvider.GetAllObjects<Models.User.User>();
            Models.User.User user;
            string hashedPassword;

            try
            {
                user = Users.FirstOrDefault(user => user.EMail == EmailID && user.BankID == bankID && user.IsActive == true && user.Status == Models.User.Status.Accepted);
                hashedPassword = _MdfHash.GetMD5Hash(password);

                // Comparing the hashed passwords.
                if (hashedPassword == user.Password)
                {
                    return user.ID;
                }
                else
                {
                    return "";
                }
            }
            catch(Exception E)
            {
                Console.WriteLine(E.StackTrace);
                return "";
                
            }
        }
    }
}
