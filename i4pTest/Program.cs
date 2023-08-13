using System;
using System.Collections.Generic;
using System.IO;

namespace i4pTest
{
    class CharHandling //Setting up the method of encryption
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

    class Encoder : CharHandling //Encrypting related method(s)
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

    class Decoder : CharHandling //Decrypting related method(s)
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

    class KeyBf : CharHandling //Key bruteforcing
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


        public string f() //Finding key
        {
            string finalKey = "";
            bool found = false;
            List<int> noWords = new List<int>();
            for (int j = 0; j < firstMessage.Length; j++)
            {

                for (int i = 0; i < words.Count; i++)
                {
                   
                    string keya = "";
                    string masik = "";

                    if (j < firstMessage.Length)
                    {
                        if (words[i].Length <= firstMessage.Length && words[i].Length <= firstMessage.Substring(j).Length)
                        {
                            keya = new Decoder(firstMessage.Substring(j, words[i].Length), words[i]).Decrypting();
                            Console.Write(words[i] + " " + firstMessage.Substring(j, words[i].Length) + " " + keya);
                        }
                    }

                    if (j < secondMessage.Length)
                    {
                        if (keya.Length <= secondMessage.Length && words[i].Length <= secondMessage.Substring(j).Length && keya.Length != 0)
                        {
                            masik = new Decoder(secondMessage.Substring(j, words[i].Length), keya).Decrypting().Split(' ')[0];
                            Console.Write(" : " + words[i] + " " + secondMessage.Substring(j, words[i].Length) + " " + keya + " " + masik + " ");
                        }
                    }

                    if (masik != "" && keya != "")
                    {
                        int k = 0;

                        while (k < words.Count && !words[k].Contains(masik))
                        {
                            k++;
                        }
                        Console.WriteLine(k);
                        if (k < words.Count)
                        {
                            Console.WriteLine(words[k]);
                            if (words[k].Length > masik.Length)
                            {

                                Console.WriteLine("ASD "+words[k].Substring(words[k].IndexOf(masik) + masik.Length));
                                Console.WriteLine("test "+secondMessage.Substring(j + words[k].IndexOf(masik) + masik.Length, words[k].Length - masik.Length));
                                keya += new Decoder(secondMessage.Substring(j + words[k].IndexOf(masik) + masik.Length, words[k].Length-masik.Length), words[k].Substring(words[k].IndexOf(masik) + masik.Length)).Decrypting();

                                keya += new Decoder(secondMessage[j].ToString(), " ").Decrypting();
                                Console.WriteLine(j);
                                finalKey += keya;
                                
                            }
                            else
                            {
                                
                                keya += new Decoder(firstMessage[j].ToString(), " ").Decrypting();
                                Console.WriteLine(j);
                                finalKey += keya;
                                
                            }
                            j = finalKey.Length+1;
                           
                        }
                        else
                        {
                            found = false;
                        }
                        Console.WriteLine(finalKey);
                    }

                }
            }
            return finalKey;
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
           
             KeyBf a = new KeyBf("tcyvfvjbkctcqhrmmuknepjf","vjyypccrdgixfppqrtcbmriiu z"); //1st message: accurate act; 2nd message: access actress; key: abcdefghijklmnopqrst
            TaskTwo();

          Console.WriteLine(a.f());


          
            Console.ReadKey();


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
