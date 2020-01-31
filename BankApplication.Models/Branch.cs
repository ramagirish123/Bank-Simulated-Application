using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.Models
{
    public class Branch
    {
        // ID of the branch.
        public string ID { get; set; }

        // Name of the branch.
        public string Name { get; set; }

        // Address of the branch.
        public string Address { get; set; }

        // BankID of the bank to which this branch belong.
        public string BankID { get; set; }

    }
}