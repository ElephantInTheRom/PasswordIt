﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordItBackend.Systems
{
    public static class DataScrambler
    {
        /*
            The responsibilities of this class are to:
            - Scramble and return data based on a raw key
            - Unscramble and return data based on a raw key
            - Confirm key with a scrambled key and a new key
            - Read strings and look for allowed characters
        */

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

        //Methods for confirming or comparing data
        public static bool CharAllowed(char c) => AllowedChars.Contains(c);
        public static bool StringAllowed(string s)
        {
            foreach(char c in s)
            {
                if (!CharAllowed(c)) { return false; }              
            }
            return true;
        }

        //Methods for scrambling or unscrambling data
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
                output += AllowedChars[(dataValues[i] / keyValues[keyIndex]) - 2];
                keyIndex++;
            }
            //Return result
            return output;
        }
    }
}