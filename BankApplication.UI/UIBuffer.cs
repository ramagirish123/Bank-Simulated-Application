using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication.UI
{
    public class UIBuffer
    {
        public void WriteLine(String text="")
        {
            Console.WriteLine(text);
        }
        // Write a single line to buffer.
        public void Write(String text)
        {
            Console.Write(text);
        }

        // Write a single char.
        public void Write(char key)
        {
            Console.Write(key);
        }


        // Read line from the buffer.
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        // Read a single character from the buffer.
        public int Read()
        {
            return Console.Read();
        }

        // Clear the buffer.
        public void Clear()
        {
            Console.Clear();
        }

        // Read a key from console
        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
    }
}
