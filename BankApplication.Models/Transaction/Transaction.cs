using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.Models.Transaction
{
    public class Transaction
    {
       
        public Transaction()
        {

            ID = Guid.NewGuid().ToString();
            IsReverted = false;
            CreatedOn = DateTime.Now;
        }


        // ID of the transaction.
        public string ID { get; set; }

        // Type of the transaction.
        public TransactionType Type { get;  set;  }

        // Money transferred or withdrawled or deposited from the account.
        public float Amount { get;  set; }

        // Boolean value that indicate wheather transaction is reverted.
        public bool IsReverted { get;  set; }

        // Transaction created time.
        public DateTime CreatedOn { get;  set; }

        // Transaction modified time.
        public DateTime ModifiedOn { get; set; }

        // UserID of the last user who modified the transaction.
        public string ModifiedBy { get; set; }

        // AccountID to which amount is credited.
     
        // AccountID to which the amount is Credited
        public string ?PayeeID { get; set; } // Payor , Payee

        // AccountID to which amount is debited.
        public string ?PayorID { get; set; }
    }
}
