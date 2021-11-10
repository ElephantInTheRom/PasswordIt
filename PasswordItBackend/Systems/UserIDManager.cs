using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordItBackend.Systems
{
    public static class UserIDManager
    {
        private static List<int> ActiveIDs { get; set; }

        static UserIDManager()
        {
            //Right now the id list resets upon program termination, 
            //In the future the class will read from a file of in use ids and set the list to those
            ActiveIDs = new();
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
            while (!ActiveIDs.Contains(newid));
            return newid;
        }

        public static bool IDExists(int id) => ActiveIDs.Contains(id);
    }
}