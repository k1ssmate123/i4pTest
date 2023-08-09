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

        List<char> characters = new List<char>();

        public Encoding(string inputMessage, string encryptionKey)
        {
            message = inputMessage;
            key = encryptionKey;
        }
       
      public void GetCharacters()
        {
            for (int i = 97; i < 123; i++)
            {
                characters.Add((char)i);  //Filling up the charlist with ascii letters - 97 - a, 122 - z
            }
            characters.Add(' ');
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
