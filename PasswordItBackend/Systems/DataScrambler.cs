using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordItBackend.Systems
{
    public static class DataScrambler
    {
        //Charcter collections for scrambling use
        //When processing char values as numbers, all values have to be >1
        private const string AllowedChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!?@#$%^&*()-_+=/,.:;<>~`";
        private static Dictionary<char, int> CharTable;
        private const char FormattedDataSeperator = ':';

        static DataScrambler()
        {
            //Build the dictionary
            CharTable = new();
            for(int i = 0; i < AllowedChars.Length; i++)
            {
                CharTable.Add(AllowedChars[i], i + 2);           
            }
        }

        // - - - Validating and Confirming - - -
        public static bool CharAllowed(char c) => AllowedChars.Contains(c);
        public static bool StringAllowed(string s)
        {
            foreach(char c in s)
            {
                if (!CharAllowed(c)) { return false; }              
            }
            return true;
        }

        public static string CutUnallowedChars(string s)
        {
            string allStr = string.Empty;
            foreach (char c in s)
            {
                if (CharAllowed(c)) { allStr += c; }
            }
            return allStr;
        }

        // - - - Scrambling and unscrambling data - - - TODO: This probably should throw exceptions, not just return nulls
        public static string? ScrambleOnKey(string data, string key)
        {
            if(data == null || data.Length < 1 || key == null || key.Length < 1)
            {
                Console.WriteLine("Data provided to scrambler is empty.");
                return null;
            }
            else if(!StringAllowed(key) || !StringAllowed(data))
            {
                Console.WriteLine("The provided data contains illegal characters");
                return null;
            }
            //Convert key into an array of values based on the table
            int[] dataValues = new int[data.Length];
            int[] keyValues = new int[key.Length];
            for (int i = 0; i < key.Length; i++) { keyValues[i] = CharTable[key[i]]; }
            //Build data array by scrambling the key values with 
            int keyIndex = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if(keyIndex >= keyValues.Length) { keyIndex = 0; }
                dataValues[i] = CharTable[data[i]] * keyValues[keyIndex];
                keyIndex++;
            }
            //Data should be scrambled, so use array to build a formatted string
            string output = ":"; 
            foreach(var num in dataValues)
            {
                output += $"{num}{FormattedDataSeperator}";
            }
            return output;
        }

        public static string? UnscrambleOnKey(string data, string key)
        {
            if (data == null || data.Length < 1 || key == null || key.Length < 1)
            {
                Console.WriteLine("Data provided to scrambler is empty.");
                return null;
            }
            //Convert key into an array of values based on the table
            int[] keyValues = new int[key.Length];
            for (int i = 0; i < key.Length; i++) { keyValues[i] = CharTable[key[i]]; }
            //Seperate scrambled data into values            
            string[] dataSplit = data.Split(FormattedDataSeperator, StringSplitOptions.RemoveEmptyEntries);
            //Parse split data into values
            int[] dataValues = new int[dataSplit.Length];
            for(int i = 0; i < dataSplit.Length; i++)
            {
                dataValues[i] = int.Parse(dataSplit[i]);
            }
            //Decode each letter and add to output string
            string output = "";
            int keyIndex = 0;
            for(int i = 0; i < dataValues.Length; i++)
            {
                if(keyIndex >= keyValues.Length) { keyIndex = 0; }
                //Make sure this index will be inside the bounds of the string
                int charIndex = (dataValues[i] / keyValues[keyIndex]) - 2;
                if (charIndex >= AllowedChars.Length || charIndex < 0) { output += " "; }
                else { output += AllowedChars[charIndex]; }               
                keyIndex++;
            }
            //Return result
            return output;
        }

        public static string GenerateScrambledMasterKey(string userKey)
        {
            string? scrambledMasterKey = ScrambleOnKey(ScramblerMaster, userKey);
            if(scrambledMasterKey == null) { 
                throw new ArgumentNullException("scrambledMasterKey", "The data scramble and unscramble methods are not set up right and should throw exceptions rather than returning nulls"); }
            return scrambledMasterKey;
        }

        //Scramblers Key Allows us to validate user login by checking if their scrambled master key matches when scrambled with an attempted password
        //This should not be changed at any time during writing after: 1/3/22 
        private const string ScramblerMaster = "pEh&gw9=saDF2Y$BxTmc-J@?V!y6AgM41-lb8UAPjKB84n&Gi8&*d6nt!1Buhgt5XQlRgnJMSR49uTV^if?sbLS^zC&=rW0MOi$DCxZNNNoV6S12f7a&Q0-0xWw&kNs%9EAx&vn%KPvqkKmNbSLX0Xx&zPgQx3-S-=VGwt_%QKqrInRnl2J%Kg6e?2*ayMq$l@&1cY@lsYZ&So5hNKPdwd*$STN66Dx^opWf&-%%IQ9-m2ykd=1GxXsiCyFkD";
    }
}