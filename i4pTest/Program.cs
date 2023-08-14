using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;

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

            for (int j = 0; j < firstMessage.Length; j++)
            {

                for (int i = 0; i < words.Count; i++)
                {

                    string keya = "";
                    string masik = "";
                    string cutFMsg = firstMessage.Substring(j, words[i].Length);
                    if (j < firstMessage.Length)
                    {
                        if (words[i].Length <= firstMessage.Length && words[i].Length <= firstMessage.Substring(j).Length)
                        {
                            keya = new Decoder(cutFMsg, words[i]).Decrypting();
                            Console.WriteLine("FM Sub: " + cutFMsg + " - " + words[i] + " - " + keya);
                        }
                    }

                    if (j < secondMessage.Length)
                    {
                        if (keya.Length <= secondMessage.Length && words[i].Length <= secondMessage.Substring(j).Length && keya.Length != 0)
                        {
                            masik = new Decoder(secondMessage.Substring(j, words[i].Length), keya).Decrypting().Split(' ')[0];
                            Console.WriteLine("SM Sub: " + secondMessage.Substring(j, words[i].Length) + " - " + keya + " - " + masik);
                        }
                    }

                    if (masik != "" && keya != "")
                    {
                        int k = 0;

                        while (k < words.Count && !words[k].Contains(masik))
                        {
                            k++;
                        }

                        if (k < words.Count)
                        {
                            if (words[k].Length > masik.Length && words[k].Length > cutFMsg.Length && !words[k].EndsWith(masik))
                            {
                                Console.WriteLine(masik + " - " + words[k]);
                                Console.WriteLine(secondMessage.Substring(j + words[k].IndexOf(masik) + masik.Length, words[k].Length - masik.Length - 1) + " - " + words[k].Substring(words[k].IndexOf(masik) + masik.Length));
                                keya += new Decoder(secondMessage.Substring(j + words[k].IndexOf(masik) + masik.Length, words[k].Length - masik.Length - 1), words[k].Substring(words[k].IndexOf(masik) + masik.Length)).Decrypting();
                                string firstLong = new Decoder(firstMessage.Substring(j, keya.Length), keya).Decrypting();


                                keya += new Decoder(secondMessage[j].ToString(), " ").Decrypting();

                            }

                            j += keya.Length;
                            finalKey += keya;
                        }

                        Console.WriteLine(finalKey);
                    }

                }
            }
            return finalKey;
        }

        public void AttempQuadrillionTwo()
        {
            int sNum = 0;
            string altKey = "";
            string fullKey = "";
            for (int i = 0; i < words.Count; i++)
            {
                string firstSub = firstMessage.Substring(sNum, words[i].Length);
                altKey = GetDecodedMsg(firstSub, words[i]);  //Return a piece of the key based on wordlist Nth element
                Console.WriteLine("\n"+firstSub+" " + words[i]+" "+altKey);
                string secondSub = secondMessage.Substring(sNum, altKey.Length);
                string wordAttempt = GetDecodedMsg(secondSub, altKey).Split(' ')[0]; // Return a wordpiece with the previously gained key; if it contains two bit /seperated by space/, it only displays the first
                Console.WriteLine(secondSub+" "+wordAttempt);
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
                       
                        fullKey += GetDecodedMsg(firstMessage[firstSub.Length].ToString(), " ");
                        Console.WriteLine(fullKey);

                    }
                    else //If it starts with it, or in the middle of the word, we need to decrypt the rest of the word (on the right of the bit)
                    {
                        int lengthSub = words[j].Substring(0, words[j].IndexOf(wordAttempt, StringComparison.Ordinal)).Length;
                        secondSub = secondMessage.Substring(sNum +lengthSub+ wordAttempt.Length, words[j].Substring(lengthSub + wordAttempt.Length).Length);
                       
                       
                        altKey += GetDecodedMsg(secondSub, words[j].Substring(lengthSub+wordAttempt.Length));

                        string firstSubMod = firstMessage.Substring(sNum, altKey.Length); //We have to check if the new,longer key finds word in first message too
                        wordAttempt = GetDecodedMsg(firstSubMod, altKey);
                        if(wordAttempt.Contains(" "))
                        {
                            wordAttempt = wordAttempt.Split(' ')[1];
                        }
                        int k = 0;
                        while(k<words.Count && words[k].Contains(wordAttempt)) { k++; }
                        if (k < words.Count)
                        {
                            
                            fullKey += altKey;
                            sNum += firstSub.Length+1;
                            fullKey += GetDecodedMsg(firstMessage[firstSub.Length].ToString(), " ");
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

        string GetDecodedMsg(string msg, string key)
        {
            return new Decoder(msg, key).Decrypting();
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
            KeyBf a = new KeyBf("ym zazrukrtoyu qdkinquj", " t bkgkjdviinbyuijabywicudn");
            a.AttempQuadrillionTwo();
            



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
