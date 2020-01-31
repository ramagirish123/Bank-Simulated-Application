using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.Models
{
    public class Bank
    {
        // ID of the Bank
        public string ID { get; set; }

        // Name of the Bank. 
        public string  Name { set; get; }
        
        // RTGS charges for same Bank Transactions.
        public float SameBankRTGSCharges { set; get; }

        // IMPS charges for same Bank Transactions.
        public float SameBankIMPSCharges { set; get; }

        // IMPS charges for differnt Bank Transactions.
        public float OtherBankRTGSCharges { set; get; }

        // RTGS charges for differnt Bank Transactions.
        public float OtherBankIMPSCharges { set; get; }

        // Flag that represent wheather Bank is deleted.
        public bool IsActive { get; set; }

        // Date of creation of account.
        public DateTime CreatedOn { get;  set; }

        // UserID of manager who created this Bank.
        public string CreatedBy { get;  set; }

        // Date of modification of Bank.
        public DateTime ? ModifeidOn { get; set; }

        // ID of the Mangager who modified the Bank.
        public string ? ModifiedBy { get; set; }

    }
}
