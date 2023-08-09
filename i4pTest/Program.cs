using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace i4pTest
{
    
    public class Encoding
    {
        protected string message;
       protected string key;
        public string Message { get { return message; } }
        public string Key { get { return key; } }

        protected List<char> characters = GetCharacters();

        public Encoding(string inputMessage, string encryptionKey)
        {
            message = inputMessage;
            key = encryptionKey;
        }
        public Encoding()
        {
            
        }

        static List<char> GetCharacters()
        {
            List<char> result = new List<char>();
            for (int i = 97; i < 123; i++)
            {
                result.Add((char)i);  //Filling up the charlist with ascii letters - 97 - a, 122 - z
            }
            result.Add(' ');
            return result;
        }

        public string Encrypting()
        {

            string encryptedMsg = "";

            for (int i = 0; i < message.Length; i++)
            {
                int sums = ReturnCharCode(message[i]) + ReturnCharCode(key[i]);
                if (sums > 26)
                {
                    sums %= 27;
                }

                encryptedMsg += characters[sums];
            }

            return encryptedMsg;

        }

        protected int ReturnCharCode(char letter)
        {
            int j = 0;
            while (j < characters.Count && characters[j] != letter)
            {
                j++;
            }
            if (j < characters.Count)
            {
                return j;
            }
            return -1;

        }


    }

    class Decoding : Encoding
    {
        public Decoding(string inputMessage, string decryptionKey)
        {
            message = inputMessage;
            key = decryptionKey;
        }

        public string Decrypting()
        {
            string decryptedMsg = "";
      
            for (int i = 0; i < message.Length; i++)
            {
                int sum = ReturnCharCode(message[i]) - ReturnCharCode(key[i]);
                if(sum < 0)
                {
                  
                    sum = 27 + sum;
                }
                decryptedMsg += characters[sum];
            }
            return decryptedMsg;

        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            TaskTwo();
        }

        static void TaskOne()
        {
            string message, key;
            Console.Write("Message: ");
            message = Console.ReadLine();
            do
            {
                Console.Write("Key: ");
                key = Console.ReadLine();
            }
            while (key.Length < message.Length);
            Encoding encode = new Encoding(message, key);
            Console.WriteLine("The encrypted message is: " + encode.Encrypting());
        }

        static void TaskTwo()
        {
            string encMessage, key;
            Console.Write("Encrypted message: ");
            encMessage = Console.ReadLine();
            do
            {
                Console.Write("Key: ");
                key = Console.ReadLine();
            }
            while (key.Length < encMessage.Length);
            Decoding decoding = new Decoding(encMessage, key);
            Console.WriteLine("The message is: "+decoding.Decrypting());
          
        }
    }
}
