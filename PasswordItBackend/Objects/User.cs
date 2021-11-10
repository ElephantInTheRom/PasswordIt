using System;
using System.Collections.Generic;

namespace PasswordItBackend.Objects
{
    public class User
    {
        //User data
        public string Username { get; private set; }
        public int UserID { get; private set; }
        public string UserKey { get; private set; }
        //User entries
        public List<LoginEntry> Entries { get; private set; }

        //Constructor
        public User(string username, int id, string key, bool newuser)
        {
            //User is not responsible for generating own ID, as it should be handled by
            //The same classes that keep overall record data
            Username = username;
            UserID = id;
            UserKey = key;
            if(newuser)
            {
                Entries = new List<LoginEntry>();
                //Some code to tell the system a new user has been created
            }
            else
            {
                //Some code that retrives this users data from the provided ID and name
                Console.WriteLine("Not implemented yet!");
            }
            
        }

    }
}
