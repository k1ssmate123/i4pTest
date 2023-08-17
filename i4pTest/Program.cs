using System;
using System.Collections.Generic;
using System.IO;

namespace i4pTest
{
    public class CharHandling //Setting up the method of encryption
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

    public class Encryption : CharHandling //Encrypting related method(s)
    {
        protected string message;
        protected string key;
        public string Message { get { return message; } }
        public string Key { get { return key; } set { key = value; } }

        public Encryption(string inputMessage, string encryptionKey)
        {
            message = inputMessage.ToLower();
            key = encryptionKey;
        }
        public Encryption()
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

    public class Decryption : CharHandling //Decrypting related method(s)
    {
        string message, key;
        public string Message { get { return message; } }
        public string Key { get { return key; } }
        public Decryption(string inputMessage, string decryptionKey)
        {
            message = inputMessage.ToLower();
            key = decryptionKey;
        }

        public string Decrypting()
        {
            string decryptedMsg = "";
            try
            {
                for (int i = 0; i < message.Length; i++)
                {
                    int sum = ReturnCharCode(message[i]) - ReturnCharCode(key[i]);

                    if (sum < 0)
                    {
                        sum = 27 + sum;
                    }
                    decryptedMsg += characters[sum];
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Wrong key!");
            }
            return decryptedMsg;
        }
    }

    class KeyBf : CharHandling //Key bruteforcing
    {
        protected string firstMessage;
        protected string secondMessage;
        List<string> msgs = new List<string>();
        public string FirstMessage { get { return firstMessage; } }
        public string SecondMessage { get { return secondMessage; } }
        List<string> words = WordsList();
        public KeyBf(string firstMessage, string secondMessage)
        {
            this.firstMessage = firstMessage;
            this.secondMessage = secondMessage;
            msgs.Add(firstMessage);
            msgs.Add(secondMessage);
        }


        public int LinearSearch(string word)
        {
            int j = 0;
            while (j < words.Count && !words[j].Contains(word))
            {
                j++;
            }
            return j;
        }
        public void FindingKey()
        {
            int k = 0;

            int s = 0; //shorter
            int l = 1; //longer
            string fullKey = "";
            string keyA = "";
            for (int i = 0; i < words.Count; i++)
            {
                string firstSub = msgs[s].Substring(k, words[i].Length);
                if (keyA == "") { keyA = GetDecodedMsg(firstSub, words[i]); }

                Console.WriteLine("\n" + firstSub + " " + keyA + " " + words[i]);
                k = fullKey.Length;
                string secondSub = msgs[l].Substring(k, keyA.Length);
                string wordA = GetDecodedMsg(secondSub, keyA);
                Console.WriteLine(wordA);
                if (wordA.Contains(" ") && !wordA.EndsWith(" "))
                {
                    wordA = wordA.Split(' ')[1];
                }
                Console.WriteLine(secondSub + " " + wordA);
                int j = LinearSearch(wordA);
                if (j < words.Count)
                {
                    int pos = words[j].IndexOf(wordA) + wordA.Length;
                    string letters = words[j].Substring(pos);
                    Console.WriteLine(letters.Length + " : " + secondSub.Length + " : " + firstSub.Length);
                    if (secondSub.Length + letters.Length > firstSub.Length)
                    {
                        Console.WriteLine(msgs[l].Substring(k + pos, letters.Length) + " " + letters);
                        string extraLetters = GetDecodedMsg(msgs[l].Substring(k + pos, letters.Length), letters);


                        // k += keyA.Length + 1;
                        fullKey += keyA;
                        keyA = extraLetters.Substring(1);



                        fullKey += GetDecodedMsg(msgs[s][k + keyA.Length + 2].ToString(), " ");
                        string temp = msgs[s];
                        msgs[s] = msgs[l];
                        msgs[l] = temp;
                        i = -1;
                    }
                    else
                    {
                        Console.WriteLine("didlidudli");
                        fullKey += keyA;
                        fullKey += GetDecodedMsg(msgs[l].Substring(k + pos, letters.Length), letters);

                        // k +=keyA.Length+1;
                        fullKey += GetDecodedMsg(msgs[l][k + keyA.Length + 1].ToString(), " ");
                        keyA = "";
                        string temp = msgs[s];
                        msgs[s] = msgs[l];
                        msgs[l] = temp;
                        i = -1;

                    }
                    Console.WriteLine(fullKey);
                }
                else { keyA = ""; }
            }
        }


        string GetDecodedMsg(string msg, string key)
        {
            return new Decryption(msg, key).Decrypting();
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
            KeyBf reversing = new KeyBf(" t bkgkjdviinbyuijabywicudn", "ym zazrukrtoyu qdkinquj ");
            //Message: curiosity killed the cat
            //Key: wtjrnhjbnsjgnjwnesbjrsjhgnbvjrd
            //The encrypted message is: ym zazrukrtoyu qdkinquj

            //Message: early bird catches the worm
            //Key: wtjrnhjbnsjgnjwnesbjrsjhgnbvjrd
            //The encrypted message is:  t bkgkjdviinbyuijabywicudn
            reversing.FindingKey();
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
            Encryption encode = new Encryption(message, key);
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
            Decryption decoding = new Decryption(encMessage, key);
            Console.WriteLine("The message is: " + decoding.Decrypting());
        }
    }
}
