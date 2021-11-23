using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordItBackend.Systems
{
    public static class UserIDManager
    {
        private static List<int> ActiveIDs { get; set; }
        private static readonly string SAVEFILE = DataPaths.DataDirectory + @"UserIds.json";

        static UserIDManager()
        {
            try
            {
                ActiveIDs = FileManager.LoadList<int>(SAVEFILE);
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("No user id file found, using new list.");
                ActiveIDs = new();
            }
        }

        // - - Saving used IDs - - 
        public static void SaveIDList()
        {
            FileManager.SaveList(SAVEFILE, ActiveIDs);
        }

        //ID Generation and retrivial
        public static int GenerateUniqueID()
        {
            if(ActiveIDs.Count >= 9000)
            {
                throw new Exception("The program has run out of unique IDs");
            }
            //If there is a space left
            Random random = new();
            int newid;
            do
            {
                newid = random.Next(1000, 10000); //This gives 9,000 possible IDs
            }
            while (ActiveIDs.Contains(newid));
            ActiveIDs.Add(newid);
            return newid;
        }

        public static bool IDExists(int id) => ActiveIDs.Contains(id);
    }
}