using System;
using System.Collections.Generic;
using System.IO;

namespace i4pTest
{
    class CharHandling
    {
        protected List<char> characters = GetCharacters();
        public static List<char> GetCharacters()
        {
            List<char> result = new List<char>();
            for (int i = 97; i < 123; i++)
            {
                result.Add((char)i);  //Filling up the charlist with ascii letters - 97 - a, 122 - z
            }
            result.Add(' ');
            return result;
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

    class Encoder : CharHandling
    {
        protected string message;
        protected string key;
        public string Message { get { return message; } }
        public string Key { get { return key; } }

        public Encoder(string inputMessage, string encryptionKey)
        {
            message = inputMessage.ToLower();
            key = encryptionKey;
        }
        public Encoder()
        {
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
    }

    class Decoder : CharHandling
    {
        string message, key;
        public string Message { get { return message; } }
        public string Key { get { return key; } }
        public Decoder(string inputMessage, string decryptionKey)
        {
            message = inputMessage.ToLower();
            key = decryptionKey;
        }

        public string Decrypting()
        {
            string decryptedMsg = "";
            for (int i = 0; i < message.Length; i++)
            {
                int sum = ReturnCharCode(message[i]) - ReturnCharCode(key[i]);
                if (sum < 0)
                {

                    sum = 27 + sum;
                }
                decryptedMsg += characters[sum];
            }
            return decryptedMsg;
        }
    }

    class KeyBf : CharHandling
    {
        protected string firstMessage;
        protected string secondMessage;
        public string FirstMessage { get { return firstMessage; } }
        public string SecondMessage { get { return secondMessage; } }
        List<string> words = WordsList();
        public KeyBf(string firstMessage, string secondMessage)
        {
            this.firstMessage = firstMessage;
            this.secondMessage = secondMessage;
        }

        public string FindingKey()
        {
            string key = "";
            int sorsz = 0;
            string keyAlt = "";
            for (int k = 0; k < words.Count; k++)
            {
                for (int i = 0; i < words.Count; i++)
                {
                    string f = firstMessage.Substring(sorsz, words[i].Length);
                    Decoder getKey = new Decoder(f, words[i]);
                    keyAlt = getKey.Decrypting();

                    string s = secondMessage.Substring(sorsz, words[i].Length);
                    Decoder findMatch = new Decoder(s, keyAlt);
                    int j = 0;
                    string secondMatch = findMatch.Decrypting();
                    while (j < secondMatch.Length && secondMatch[j] != ' ')
                    {
                        j++;
                    }
                    if (j < secondMatch.Length)
                    {
                        secondMatch = secondMatch.Substring(0, j);
                    }
                    while (j < words.Count && !words[j].Contains(secondMatch))
                    {
                        j++;
                    }
                    if (j < words.Count)
                    {
                        sorsz = words[i].Length - 1;
                        key += keyAlt;

                    }
                    else
                    {

                    }
                }
            }
            return key;
        }

        static List<string> WordsList()
        {
            List<string> wrds = new List<string>();
            StreamReader inp = new StreamReader("words.txt");
            while (!inp.EndOfStream)
            {
                wrds.Add(inp.ReadLine());

            }
            inp.Close();
            return wrds;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            KeyBf a = new KeyBf("pk ptejbaexjuyvvcbnfmxhx", "rr scmcruimdjftzhafuuzg e r");
            Console.WriteLine(a.FindingKey());
            Console.ReadKey();
            // KeyBf a = new KeyBf("syhdgqpcgtqrqowhqlfnyjbn", "uehgqyis xflfwulvkybflaqrvd");
            //a.FindingKey();
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
            Encoder encode = new Encoder(message, key);
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
            Decoder decoding = new Decoder(encMessage, key);
            Console.WriteLine("The message is: " + decoding.Decrypting());
        }
    }
}
