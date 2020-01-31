using System;
using System.Collections.Generic;
using System.Text;
using BankApplication.Services;
using BankApplication.Data;

namespace BankApplication.UI
{
    public class Bank
    {
        private BankService _bankService;
        private DataProvider _dataProvider;
        private Utility.UIStyling _UIStyling;
        private UIBuffer _UIBuffer;

        public Bank(string JsonFilePath, Utility.UIStyling UIStyling,UIBuffer UIBuffer)
        {
            _UIBuffer = UIBuffer;
            _UIStyling = UIStyling;
            _bankService = new BankService(string.Empty,JsonFilePath);
            _dataProvider = new DataProvider(JsonFilePath);
        }

        // Display all the banks and prompts user to select a bank from that.
        public Models.Bank DisplayAndSelectBank()
        {
            IList<Models.Bank> banks;
            Models.Bank selectedBank;
            int index;
            int banksCount;
            int selectedIndex;

            banks = _bankService.GetAllBanks(); ;
            banksCount = banks.Count;

            if(banksCount==0)
            {
                _UIStyling.ChangeForegroundForErrorMessage();
                _UIBuffer.WriteLine("There are no banks in the application\n");
                _UIStyling.RestoreForegroundColor();
            }
            while(true)
            {
                index = 0;
                _UIBuffer.WriteLine("Press number corresponding to the bank");
                _UIBuffer.WriteLine("-------------------------------");
                _UIBuffer.WriteLine(String.Format("  {0,-8}   {1,-10}\n", "S.No","Bank Name"));
                foreach (Models.Bank bank in banks)
                {
                    _UIBuffer.WriteLine(String.Format("  {0,-8}   {1,-10}", index+1, bank.Name));
                    index++;
                    
                }
                _UIBuffer.WriteLine("-------------------------------\n");
                if(int.TryParse(_UIBuffer.ReadLine(),out selectedIndex))
                {
                    _UIBuffer.WriteLine("");
                    if(selectedIndex>0 && selectedIndex<=banksCount)
                    {
                        selectedBank = banks[selectedIndex - 1];
                        return selectedBank;
                    }
                    else
                    {
                        _UIStyling.ChangeForegroundForErrorMessage();
                        _UIBuffer.WriteLine(String.Format("Enter numbers between 1 and {0}\n", banksCount));
                        _UIStyling.RestoreForegroundColor();
                    }
                }
                else
                {
                    _UIBuffer.WriteLine("");
                    _UIStyling.ChangeForegroundForErrorMessage();
                    _UIBuffer.WriteLine("Enter only numeric values\n");
                    _UIStyling.RestoreForegroundColor();
                }

            }

            
        }
     
    

    }
}
