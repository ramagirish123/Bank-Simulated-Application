using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace BankApplication.UI.Utility
{
    public class PasswordHider
    {
        private UIBuffer _UIBuffer;
       public PasswordHider(UIBuffer UIBuffer)
        {
            _UIBuffer = UIBuffer;
        }
       public string ReadPassword()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = _UIBuffer.ReadKey();
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Thread.Sleep(200);
                    _UIBuffer.Write("\b \b");
                    _UIBuffer.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        _UIBuffer.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            _UIBuffer.WriteLine();
            return pass;
        }
    }
}
