using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.UI.Utility
{
    public class UIStyling
    {
        int _width;
        int _height;

        public UIStyling(int width,int height)
        {
            _width = width;
            _height = height;
            Console.SetWindowSize(100, 50);
            Console.SetBufferSize(100, 50);
        }

        // Alligning the string to center of the screen with padding characters.
        public string PadBoth(string source,char paddingCharacter)
        {
            int spaces = _width - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft, paddingCharacter).PadRight(_width, paddingCharacter);
        }

        // Changing console foreground text colour to red
        public void ChangeForegroundForErrorMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public void RestoreForegroundColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
