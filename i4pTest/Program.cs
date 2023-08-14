using System;
using System.CodeDom.Compiler;
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

        public void FindingKey()
        {
            int sNum = 0;
            string altKey = "";
            string fullKey = "";
            int main = 0;
            int secondary = 1;

            for (int i = 0; i < words.Count; i++)
            {
                string firstSub = msgs[main].Substring(sNum, words[i].Length);
                altKey = GetDecodedMsg(firstSub, words[i]);  //Return a piece of the key based on wordlist Nth element
                Console.WriteLine("\n" + firstSub + " " + words[i] + " " + altKey);
                string secondSub = msgs[secondary].Substring(sNum, altKey.Length);
                string wordAttempt = GetDecodedMsg(secondSub, altKey).Split(' ')[0]; // Return a wordpiece with the previously gained key; if it contains two bit /seperated by space/, it only displays the first
                Console.WriteLine(secondSub + " " + wordAttempt);
                int j = 0;
                while (j < words.Count && !words[j].Contains(wordAttempt)) //Linear search for full word based on previous word string
                {
                    j++;
                }
                if (j < words.Count)
                {
                    if (words[j].EndsWith(wordAttempt)) //If it ends with the bit, no additional operation needed
                    {
                        fullKey += altKey;

                        sNum += firstSub.Length + 1;

                        fullKey += GetDecodedMsg(msgs[main][firstSub.Length].ToString(), " ");
                        Console.WriteLine(fullKey);

                    }
                    else //If it starts with it, or in the middle of the word, we need to decrypt the rest of the word (on the right of the bit)
                    {
                        int lengthSub = words[j].Substring(0, words[j].IndexOf(wordAttempt, StringComparison.Ordinal)).Length;
                        secondSub = msgs[secondary].Substring(sNum + lengthSub + wordAttempt.Length, words[j].Substring(lengthSub + wordAttempt.Length).Length);
                        altKey += GetDecodedMsg(secondSub, words[j].Substring(lengthSub + wordAttempt.Length));
                        string firstSubMod = msgs[main].Substring(sNum, altKey.Length); //We have to check if the new,longer key finds word in first message too
                        wordAttempt = GetDecodedMsg(firstSubMod, altKey);
                        if (wordAttempt.Contains(" "))
                        {
                            wordAttempt = wordAttempt.Split(' ')[1];
                        }
                        int k = 0;
                        while (k < words.Count && words[k].Contains(wordAttempt)) { k++; }
                        if (k < words.Count)
                        {
                            fullKey += altKey;
                            sNum += firstSub.Length + 1;
                            fullKey += GetDecodedMsg(msgs[main][firstSub.Length].ToString(), " ");
                            Console.WriteLine(fullKey);
                        }
                        else
                        {
                            fullKey = "";
                            sNum = 0;
                        }
                    }
                }
            }
            Console.WriteLine(fullKey);
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
        public void Finding()
        {
            int k = 0;

            int s = 0; //shorter
            int l = 1; //longer
            string fullKey = "";
            for (int i = 0; i < words.Count; i++)
            {
                string firstSub = msgs[s].Substring(k, words[i].Length);
                string keyA = GetDecodedMsg(firstSub, words[i]);

                Console.WriteLine("\n"+firstSub + " "+ keyA+" " + words[i]);

                string secondSub = msgs[l].Substring(k, keyA.Length);
                string wordA = GetDecodedMsg(secondSub, keyA);
                if(wordA.Contains(" "))
                {
                    wordA = wordA.Split(' ')[1];
                }
                Console.WriteLine(secondSub+" "+wordA);
                int j = LinearSearch(wordA);
                if (j < words.Count)
                {
                    int pos = words[j].IndexOf(wordA) + wordA.Length;
                    string letters = words[j].Substring(pos);
               
                    if (secondSub.Length+letters.Length > firstSub.Length)
                    {
                        Console.WriteLine(msgs[l].Substring(k + pos, letters.Length)+" "+letters);
                        string extraLetters = GetDecodedMsg(msgs[l].Substring(k + pos, letters.Length), letters);
                       

                      
                        keyA += extraLetters;
                        Console.WriteLine(extraLetters);
                        
                        Console.WriteLine((msgs[s].Substring(k + pos, letters.Length)));
                        Console.WriteLine(GetDecodedMsg(msgs[s].Substring(k + pos, letters.Length), extraLetters));

                     
                          if (GetDecodedMsg(msgs[s].Substring(k + pos, letters.Length), extraLetters).Contains(" "))
                          {
                            k++;
                          }

                        k += keyA.Length;
                        keyA += GetDecodedMsg(msgs[l][k].ToString(), " ");

                       

                        string temp = msgs[s];
                        msgs[s] = msgs[l];
                        msgs[l]= temp;
                        i = -1;
                        
                    }
                    fullKey += keyA;
                    Console.WriteLine(fullKey);
                }
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
            KeyBf finding = new KeyBf(" t bkgkjdviinbyuijabywicudn", "ym zazrukrtoyu qdkinquj ");



            finding.Finding();
            //TaskTwo();
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
