using BankApplication.Models;
using System.Collections.Generic;

namespace BankApplication.Services.Interfaces
{
    public interface IBankService
    {
        IList<Bank> GetAllBanks();
        Bank GetBankByID(string ID);
        string UpdateImpsOfOtherBank(string bankID, float ImpsCharges);
        string UpdateImpsOfSameBank(string bankID, float ImpsCharges);
        string UpdateRtgsOfOtherBank(string bankID, float RtgsCharges);
        string UpdateRtgsOfSameBank(string bankID, float RtgsCharges);
    }
}