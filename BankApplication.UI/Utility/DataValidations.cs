using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BankApplication.UI.Utility
{
    class DataValidations
    {
        public bool IsEMailValid(string EMail)
        {
            return Regex.IsMatch(EMail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

        }
    }
}
