    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i4pTest
{

    class Encoding
    {
        string message;
        string key;
       public string Message { get { return message; } }
        public string Key { get { return key; } }

        public Encoding(string inputMessage, string encryptionKey)
        {
            message = inputMessage;
            key = encryptionKey;
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Message: ");
            string message = Console.ReadLine();
            Console.Write("Key: ");
            string key = Console.ReadLine();
            Encoding encode = new Encoding(message, key);
        }
    }
}
