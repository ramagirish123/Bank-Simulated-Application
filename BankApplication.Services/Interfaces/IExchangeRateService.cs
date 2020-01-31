using BankApplication.Models.ExchangeRate;
using System.Collections.Generic;

namespace BankApplication.Services.Interfaces
{
    public interface IExchangeRateService
    {
        string AddExchangeRate(string bankID, string name, float rate);
        bool CheckExchangeRateExists(string bankID, string currencyName);
        IList<ExchangeRate> GetExchangeRatesByBankID(string bankID);
    }
}