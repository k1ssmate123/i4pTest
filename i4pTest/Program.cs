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
       
        void GetCharacters()
        {
            for (int i = 97; i < 123; i++)
            {
                characters.Add((char)i);  //Filling up the charlist with ascii letters - 97 - a, 122 - z
            }
            characters.Add(' ');
        }

        public string Encrypting()
        {
            GetCharacters();
            string encryptedMsg="";

            for (int i = 0; i < message.Length; i++)
            {
                int sums = ReturnCharCode(message[i])+ReturnCharCode(key[i]);
                if (sums > 26)
                {
                    sums %= 27;
                }
              
                encryptedMsg += characters[sums];
            }

            return encryptedMsg;

        }

        int ReturnCharCode(char letter)
        {
            int j = 0;
            while(j < characters.Count&& characters[j] != letter)
            {
                j++;
            }
            if(j < characters.Count)
            {
                return j;
            }
            return -1;

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
            Console.WriteLine(encode.Encrypting());


        }
    }
}
