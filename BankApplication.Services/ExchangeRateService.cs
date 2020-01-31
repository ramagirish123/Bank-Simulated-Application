using System;
using System.Collections.Generic;
using System.Text;
using BankApplication.Data;
using BankApplication.Models.ExchangeRate;
using System.Linq;

namespace BankApplication.Services
{
    public class ExchangeRateService : Interfaces.IExchangeRateService
    {
        private DataProvider _dataProvider;

        public ExchangeRateService(string JsonFilePath)
        {
            _dataProvider = new DataProvider(JsonFilePath);
        }

        // Add new exchange rates for a bank.
        public string AddExchangeRate(string bankID, string name, float rate)
        {
            ExchangeRate exchangeRate = new ExchangeRate()
            {
                CurrencyName = name,
                BankID = bankID,
                Rate = rate
            };
            if (_dataProvider.AddObject<ExchangeRate>(exchangeRate) == "")
                return "Operation Failed";
            else
                return "Successfully added";

        }

        // Get exchange rates of a bank.
        public IList<ExchangeRate> GetExchangeRatesByBankID(string bankID)
        {
            IList<ExchangeRate> exchangeRates = _dataProvider.GetAllObjects<ExchangeRate>();
            IList<ExchangeRate> bankExchangeRate;

            try
            {
                bankExchangeRate = exchangeRates.Where(exchangeRate => exchangeRate.BankID == bankID).ToList();
            }
            catch (Exception)
            {
                return null;
            }
            return bankExchangeRate;
        }

        // check Wheather ExchangeRate Exists in a bank.
        public bool CheckExchangeRateExists(string bankID, string currencyName)
        {
            IList<ExchangeRate> exchangeRates = _dataProvider.GetAllObjects<ExchangeRate>();
            try
            {
                return exchangeRates.Any(exchangeRate => exchangeRate.BankID == bankID && exchangeRate.CurrencyName == currencyName);
            }
            catch (NullReferenceException)
            {
                return false;
            }

        }
    }
}
