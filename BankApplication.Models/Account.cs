using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.Models
{

    public class Account
    {
        // ID of the Account.  
        public string ID { get; set; }

        // ID of the User who hold the account.
        public string UserID { get; set; }
        
        // Balance of the account.
        public float Balance { get; set; }
        
        // Rate of interest for the account.
        public float ? RateOfInterest { get; set; }

        // Flag that represent wheather the account is deleted.
        public bool IsActive { get; set; }

        // Date on which the account is created.
        public DateTime CreatedOn { get;  set; }

        //  ID of the user who created that account.
        public string CreatedBy { get;  set; }

        // Date of modification.
        public DateTime ? ModifiedOn { get; set; }

        // ID of the user who modified the account
        public string ? ModifiedBy { get; set; }

    }
}
