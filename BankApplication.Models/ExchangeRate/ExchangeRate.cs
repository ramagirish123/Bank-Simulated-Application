using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.Models.ExchangeRate
{
    public class ExchangeRate
    {
    public string ID { get; set;
        }
    public string BankID { get; set; }
    public string CurrencyName { get; set; }
    public float Rate { get; set; }
    }
}
