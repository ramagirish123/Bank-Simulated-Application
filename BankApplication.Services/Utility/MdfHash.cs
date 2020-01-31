using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BankApplication.Services.Utility
{
    public class MdfHash : Interfaces.Utility.IMdfHash
    {
        // Generate MD5 hash for password.
        public string GetMD5Hash(string password)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data;
            StringBuilder sBuilder;
            String hashedPassword;

            // Convert the input string to a byte array and compute the hash.
            data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            hashedPassword = sBuilder.ToString();
            return hashedPassword;
        }
    }
}
