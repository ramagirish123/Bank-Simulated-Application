using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.Models.User
{

    public class User
    {
        // UserID.
        public string ID { get; set; }

        // Password of the user.  
        public string Password { get; set; }

        // Name of the user.
        public string Name { get; set; }

        // Indicates wheather user account is deleted or not.
        public bool IsActive { get; set; }

        // Email of the user.
        public string EMail { get; set; }

        // MobileNumber of the user.
        public string MobileNumber { get; set; }

        // Type of the user.
        public UserType Type { get; set; }

        public Status Status { get; set; }

        // Reason for decline.
        public string ? DeclineReason { get; set; }

        // Time of Creation of object.
        public DateTime CreatedOn { get;  set; }

        //  UserID of the created user
        public string CreatedBy { get;  set; }

        // TimeStamp of last modfification of this transaction.
        public DateTime ? ModifiedOn { get; set; }

        // UserID of user who modified this User last.
        public string ? ModifiedBy { get; set; }

        // BankID of the bank in which the user is present and it is the foreign key
        public string BankID { get; set; }


    }
}
