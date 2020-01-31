using System;
using System.IO;
using BankApplication.Models;
using Newtonsoft.Json.Linq;
using BankApplication.Data;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.Web;
namespace BankApplication.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = JObject.Parse(File.ReadAllText("C://Users//satyasai.k//source//repos//Bank Simulated Application//UI//Configuration//AppSettings.json"))["DataFilePath"].ToString();
            App UI = new App("..//..//..//..//bankapplication.data//bank.json");
            UI.Run();

        }
    }
}

