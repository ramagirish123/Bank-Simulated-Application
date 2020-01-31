using BankApplication.Models.User;
using System.Collections.Generic;

namespace BankApplication.Services.Interfaces
{
    public interface IUserService
    {
        string AcceptUserRequestByID(string ID);
        string AddRequestedUser(Models.User.User user);
        string AddUser(User user);
        string AuthenticateUser(string EmailID, string password, string bankID);
        bool CheckEMailAvailbility(string Email, string bankID);
        string DeclineUserRequestByID(string ID, string declineReason);
        string DeleteUserByID(string ID);
        IList<User> GetAllCustomersOfBank(string userID, string bankID);
        int GetRequestedUsersCount(string bankID);
        IList<User> GetRequestedUsersOfBank(string bankID);
        User GetUserByID(string userID);
        string UpdateCustomerEMailByID(string ID, User user);
        string UpdateCustomerMobileNumberByID(string ID, User user);
        string UpdateCustomerNameByID(string ID, User user);
    }
}