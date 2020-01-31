using System;
using System.Collections.Generic;
using System.Text;
using BankApplication.Models;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.IO;
using BankApplication.Services;
using BankApplication.Data;
using System.Linq;

namespace BankApplication.Services
{
    public class BankService : Interfaces.IBankService
    {
        private DataProvider _dataProvider;
        private string _userID;
        public BankService(string userID,string JsonFilePath)
        {
            _dataProvider = new DataProvider(JsonFilePath);
            _userID = userID;
        }

        // Get all Banks in the application.
        public IList<Bank> GetAllBanks()
        {
            IList<Bank> Banks = _dataProvider.GetAllObjects<Bank>();

            return Banks;
        }

        // Get a Bank by ID.
        public Bank GetBankByID(string ID)
        {
            Bank bank;

            try
            {
                bank = _dataProvider.GetObjectByID<Bank>(ID);
                return bank;
            }
            catch
            {
                return null;
            }
        }

        // Update RTGS of same bank.
        public string UpdateRtgsOfSameBank(string bankID, float RtgsCharges)
        {
            Bank bank;
            string message;

            try
            {
                bank = _dataProvider.GetObjectByID<Bank>(bankID);
                bank.SameBankRTGSCharges = RtgsCharges;
                bank.ModifiedBy = _userID;
                bank.ModifeidOn = DateTime.Now;
                if (_dataProvider.UpdateObject<Bank>(bank))
                    message = "Updation Successul";
                else
                    message = "Updation Failed";
            }
            catch
            {
                message = "Updation Failed";
            }
            return message;
        }


        // Update IMPS of same bank.
        public string UpdateImpsOfSameBank(string bankID, float ImpsCharges)
        {
            Bank bank;
            string message;

            try
            {
                bank = _dataProvider.GetObjectByID<Bank>(bankID);
                bank.SameBankRTGSCharges = ImpsCharges;
                bank.ModifiedBy = _userID;
                bank.ModifeidOn = DateTime.Now;
                if (_dataProvider.UpdateObject<Bank>( bank))
                    message = "Updation Successul";
                else
                    message = "Updation Failed";
            }
            catch
            {
                message = "Updation Failed";
            }
            return message;
        }

        // Update RTGS of other bank.
        public string UpdateRtgsOfOtherBank(string bankID,float RtgsCharges)
        {
            Bank bank;
            string message;

            try
            {
                bank = _dataProvider.GetObjectByID<Bank>(bankID);
                bank.OtherBankRTGSCharges = RtgsCharges;
                bank.ModifiedBy = _userID;
                bank.ModifeidOn = DateTime.Now;
                if (_dataProvider.UpdateObject<Bank>(bank))
                    message = "Updation Successul";
                else
                    message = "Updation Failed";
            }
            catch
            {
                message = "Updation Failed";
            }
            return message;
        }

        // Update RTGS of same bank.
        public string UpdateImpsOfOtherBank(string bankID, float ImpsCharges)
        {
            Bank bank;
            string message;
            try
            {
                bank = _dataProvider.GetObjectByID<Bank>(bankID);
                bank.OtherBankRTGSCharges = ImpsCharges;
                bank.ModifiedBy = _userID;
                bank.ModifeidOn = DateTime.Now;
                if (_dataProvider.UpdateObject<Bank>(bank))
                    message = "Updation Successul";
                else
                    message = "Updation Failed";
            }
            catch
            {
                message = "Updation Failed";
            }
            return message;
        }

    }
}
