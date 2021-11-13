using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonConstructor]
        public User(string username, int userid, string userkey, List<LoginEntry> entries)
        {
            //This constructor will be used by the json deserializer
            Console.WriteLine("Constructor called by deserializer!");
            Username = username;
            UserID = userid;
            UserKey = userkey;
            Entries = entries;
        }
        public User(string username, int id, string key)
        {
            Entries = new();
            Username = username;
            UserID = id;
            UserKey = key;         
        }
        
        //Methods for editing the users entry list
        public string GetEntryList()
        {
            //Once data is being scrambled, there will need to be some way to decode it here.
            //Possibly could use a encoded user and decoded user as children of main user?
            if (Entries.Count == 0) { return "No entries."; }
            //else
            string output = "";
            for(int i = 0; i < Entries.Count; i++)
            {
                output += $"{i + 1}: {Entries[i].Title} : {Entries[i].Username} : {Entries[i].Password}\n";
            }

            return output;
        }

        /// <summary>
        /// Removes an entry from the users list based on the entry number.
        /// </summary>
        /// <param name="entryNum">The entry number to remove. This is not the index, it is the num that appears with the entry.</param>
        /// <returns>True or False depending on success.</returns>
        public bool RemoveEntry(int entryNum)
        {
            int index = entryNum - 1;
            if(index >= Entries.Count || index < 0) { return false; }
            else
            {
                Entries.Remove(Entries[index]);
                return true;
            }
        }

        /// <summary>
        /// Adds a new entry to the users list. Must have at least one perameter that is not blank. 
        /// </summary>
        /// <param name="title">The title of the entry, can be blank.</param>
        /// <param name="entryUsername">The username field for the entry.</param>
        /// <param name="entryPassword">The password or passkey feild for the entry.</param>
        /// <returns></returns>
        public bool AddEntry(string? title, string entryUsername, string entryPassword)
        {
            if(title == null && entryUsername == string.Empty && entryPassword == string.Empty)
            {
                return false;
            }
            else
            {
                //This should be where encoding and decoding of entry data takes place, once it is ready
                Entries.Add(new LoginEntry(title ?? "New Entry", entryUsername, entryPassword, false));
                return true;
            }
        }

        //Methods for formatting and representing the players data
        public override string ToString() => $"Name: {Username}, ID: {UserID}, Key: {UserKey}, # of entries: {Entries.Count}";
    }
}