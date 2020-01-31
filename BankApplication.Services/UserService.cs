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
    public class UserService : Interfaces.IUserService
    {
        private DataProvider _dataProvider;
        private Interfaces.Utility.IMdfHash _MdfHash;
        private string _userID;

        public UserService(string userID, string JsonFilePath)
        {
            _MdfHash = new Utility.MdfHash();
            _dataProvider = new DataProvider(JsonFilePath);
            _userID = userID;
        }

        // Authenticating User basing on UserID.
        public string AuthenticateUser(string EmailID, string password, string bankID)
        {
            IList<Models.User.User> Users = _dataProvider.GetAllObjects<Models.User.User>();
            Models.User.User user;
            string hashedPassword;

            try
            {
                user = Users.First(user => user.EMail == EmailID && user.BankID == bankID && user.IsActive == true && user.Status == Models.User.Status.Accepted);
                hashedPassword = _MdfHash.GetMD5Hash(password);

                // Comparing the hashed passwords.
                if (hashedPassword == user.Password)
                {
                    return user.ID;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        // Service that add new user.
        public string AddUser(Models.User.User user)
        {
            string message;
            user.ID = Guid.NewGuid().ToString();
            user.Password = _MdfHash.GetMD5Hash(user.Password);
            user.Status = Models.User.Status.Accepted;
            user.Type = Models.User.UserType.Customer;
            user.IsActive = true;
            user.CreatedOn = DateTime.Now;
            message = _dataProvider.AddObject<Models.User.User>(user);
            return message;

        }

        // Service that add requested user.
        public string AddRequestedUser(Models.User.User user)
        { 
            string message;
            user.IsActive = false;
            user.ID = Guid.NewGuid().ToString();
            user.Status = Models.User.Status.Pending;
            user.CreatedBy = _userID;
            user.CreatedOn = DateTime.Now;
            user.Type = Models.User.UserType.Customer;
            message = _dataProvider.AddObject<Models.User.User>(user);
            return message;
        }

        // Delete user.
        public string DeleteUserByID(string ID)
        {
            Models.User.User user = _dataProvider.GetObjectByID<Models.User.User>(ID);

            user.IsActive = false;
            user.ModifiedBy = _userID;
            user.ModifiedOn = DateTime.Now;

            if (_dataProvider.UpdateObject<Models.User.User>(user))
                return "Deleted successfully";
            else
                return "Deletion Failed";
        }

        // Retriving UserBaseObject based on UserID by making password null.
        public Models.User.User GetUserByID(string userID)
        {
            Models.User.User user;

            try
            {
                user = _dataProvider.GetObjectByID<Models.User.User>(userID);
                user.Password = null;
                return user;
            }

            catch
            {
                return default;
            }
        }


        // Get all Customers of the bank.
        public IList<Models.User.User> GetAllCustomersOfBank(string userID, string bankID)
        {
            IList<Models.User.User> users;

            users = _dataProvider.GetAllObjects<Models.User.User>();
            try
            {
                return users.Where(user => user.IsActive == true && user.Type == Models.User.UserType.Customer && user.BankID == bankID && user.Status == Models.User.Status.Accepted).ToList();
            }
            catch
            {
                return default;
            }
        }

        // Update Customer name.
        public string UpdateCustomerNameByID(string ID, Models.User.User user)
        {
            Models.User.User modifiedUser = _dataProvider.GetObjectByID<Models.User.User>(ID);
            string message;

            try
            {
                modifiedUser.Name = user.Name;
                modifiedUser.ModifiedBy = _userID;
                modifiedUser.ModifiedOn = DateTime.Now;
                if (_dataProvider.UpdateObject<Models.User.User>(modifiedUser))
                    message = "successfully Updated";
                else
                    message = "Updation Failed";
            }
            catch
            { 
                message = "Updation Failed";
            }
            return message;
        }

        // Update Customer MobileNumber.
        public string UpdateCustomerMobileNumberByID(string ID, Models.User.User user)
        {
            Models.User.User modifiedUser = _dataProvider.GetObjectByID<Models.User.User>(ID);
            string message;

            try
            {
                modifiedUser.MobileNumber = user.MobileNumber;
                modifiedUser.ModifiedBy = _userID;
                modifiedUser.ModifiedOn = DateTime.Now;
                if (_dataProvider.UpdateObject<Models.User.User>(modifiedUser))
                    message = "successfully Updated";
                else
                    message = "Updation Failed";
            }
            catch
            {
                message = "Updation Failed";
            }
            return message;
        }

        // Update Customer Email.
        public string UpdateCustomerEMailByID(string ID, Models.User.User user)
        {
            Models.User.User modifiedUser = _dataProvider.GetObjectByID<Models.User.User>(ID);
            string message;

            try
            {
                modifiedUser.EMail = user.EMail;
                modifiedUser.ModifiedBy = _userID;
                modifiedUser.ModifiedOn = DateTime.Now;
                if (_dataProvider.UpdateObject<Models.User.User>(modifiedUser))
                    message = "successfully Updated";
                else
                    message = "Updation Failed";
            }
            catch
            {
                message = "Updation Failed";
            }
            return message;
        }

        // Check wheather Email is available for a new user in a bank.
        public bool CheckEMailAvailbility(string Email, string bankID)
        {
            bool isAvailble;
            IList<Models.User.User> users = _dataProvider.GetAllObjects<Models.User.User>();

            try
            {
                isAvailble = !users.Any(user => user.EMail == Email && user.BankID == bankID);
            }
            catch
            {
                isAvailble = false;
            }
            return isAvailble;
        }

        // Get requested users count of a bank.
        public int GetRequestedUsersCount(string bankID)
        {
            int requestedUsersCount;
            IList<Models.User.User> users = _dataProvider.GetAllObjects<Models.User.User>();

            try
            {
                requestedUsersCount = users.Where(user => user.Status == Models.User.Status.Pending && user.IsActive == true && user.BankID == bankID).Count();
            }
            catch
            {
                requestedUsersCount = 0;
            }
            return requestedUsersCount;
        }

        // Get all requested users.
        public IList<Models.User.User> GetRequestedUsersOfBank(string bankID)
        {
            IList<Models.User.User> requestedUsers;
            IList<Models.User.User> users = _dataProvider.GetAllObjects<Models.User.User>();

            try
            {
                requestedUsers = users.Where(user => user.BankID == bankID && user.IsActive == true && user.Status == Models.User.Status.Pending).ToList();
            }
            catch
            {
                return null;
            }
            return requestedUsers;
        }


        // Accept request of user.
        public string AcceptUserRequestByID(string ID)
        {
            Models.User.User user = _dataProvider.GetObjectByID<Models.User.User>(ID);
            string message;

            try
            {
                user.Status = Models.User.Status.Accepted;
                user.ModifiedBy = ID;
                user.ModifiedOn = DateTime.Now;
                if (_dataProvider.UpdateObject<Models.User.User>(user))
                    message = "Acceptance Successful";
                else
                    message = "Acceptance Failed";
            }
            catch
            {
                message = "Acceptance Failed";
            }
            return message;

        }

        // Decline request of user.
        public string DeclineUserRequestByID(string ID, string declineReason)
        {
            Models.User.User user = _dataProvider.GetObjectByID<Models.User.User>(ID);
            string message;

            try
            {
                user.Status = Models.User.Status.Rejected;
                user.ModifiedBy = ID;
                user.ModifiedOn = DateTime.Now;
                if (_dataProvider.UpdateObject<Models.User.User>(user))
                    message = "User Declined";
                else
                    message = "Declination Failed";
            }
            catch
            {
                message = "Declination Failed";
            }
            return message;

        }


    }
}
