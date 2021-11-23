using PasswordItBackend.Systems;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static PasswordItBackend.Systems.DataScrambler;

//User key should be encoded on itself when registering a user

namespace PasswordItBackend.Objects
{
    public class User
    {
        //User data
        public string Username { get; private set; }
        public int UserID { get; private set; }
        //User validation data
        public string EncodedKey { get; init; } //Does this need to be public to be included in serialization?
        [JsonIgnore] public string? OpenKey { get; private set; }
        [JsonIgnore] public bool UserValidated { get; private set; }
        //User entries
        [JsonIgnore] public bool EntriesScrambled { get; private set; }
        public List<LoginEntry> Entries { get; private set; }

        //Constructors
        [JsonConstructor]
        public User(string username, int userid, string encodedKey, List<LoginEntry> entries)
        {
            //This constructor will be used by the json deserializer
            Console.WriteLine("Constructor called by deserializer!");
            Username = username;
            UserID = userid;
            EncodedKey = encodedKey;
            Entries = entries;
            OpenKey = null;
            UserValidated = false;
            EntriesScrambled = true;
        }
        //Should be used when registering a new user in the program
        public User(string username, string key)
        {
            Entries = new();
            Username = username;
            UserID = UserIDManager.GenerateUniqueID();
            EncodedKey = EncodeKey(key);
            OpenKey = key;
            UserValidated = true;
            EntriesScrambled = false;
        }

        // - - Lock and unlock a user and its data - -
        /// <summary>
        /// Locks a users data, and sets user to unvalidated state.
        /// </summary>
        public void LockUser()
        {
            if (!EntriesScrambled)
            {
                EncodeEntries();
                UserValidated = false;
                OpenKey = null;
            }
            else
            {
                Console.WriteLine("Entries already encoded.");
            }
        }
        /// <summary>
        /// Unlocks users data and sets user to a validated state.
        /// </summary>
        /// <param name="key">Key to use when unlocking.</param>
        /// <returns>True or false depending on if correct key was given.</returns>
        public bool UnlockUser(string key)
        {
            if (TestKey(key))
            {
                Console.WriteLine("Correct key was given, unlocking data.");
                UserValidated = true;
                DecodeEntries();              
                return true;
            }
            else
            {
                Console.WriteLine("Key entered was not correct.");
                return false;
            }
        }

        // - - Scramble and unscramble user data - - 
        private void DecodeEntries()
        {
            if (!UserValidated) { Console.WriteLine("User is not validated! Validate before using this method!"); }
            else
            {
                List<LoginEntry> decodedEntries = new();
                foreach (var entry in Entries)
                {
                    var title = entry.Title;
                    var username = UnscrambleOnKey(entry.Username, OpenKey);
                    var password = UnscrambleOnKey(entry.Password, OpenKey);
                    var e = new LoginEntry(title, username, password, false);
                    decodedEntries.Add(e);
                }
                Entries = decodedEntries;
                EntriesScrambled = false;
            }

        }

        private void EncodeEntries()
        {
            if (UserValidated == false) { Console.WriteLine("User data was not decoded, skipping encoding"); }
            else
            {
                List<LoginEntry> encodedEntries = new();
                foreach (var entry in Entries)
                {
                    var title = entry.Title;
                    var username = ScrambleOnKey(entry.Username, OpenKey);
                    var password = ScrambleOnKey(entry.Password, OpenKey);
                    var e = new LoginEntry(title, username, password, true);
                    encodedEntries.Add(e);
                }
                Entries = encodedEntries;
                EntriesScrambled = true;
            }
        }

        // - - Encode and confirm user key - - 
        private string EncodeKey(string key) => ScrambleOnKey(key, key);

        private bool TestKey(string key)
        {
            string? unscrambledKey = UnscrambleOnKey(EncodedKey, key);
            if (unscrambledKey == null) { return false; }
            else if (unscrambledKey == key)
            {
                Console.WriteLine("User key validated.");
                OpenKey = key;
                return true;
            }
            else
            {
                Console.WriteLine("User key is not validated");
                return false;
            }
        }

        // - - Editing entry list - - 
        /// <summary>
        /// Gets and returns a string containing the whole list of user entries.
        /// </summary>
        public string GetEntryList()
        {
            //Once data is being scrambled, there will need to be some way to decode it here.
            //Possibly could use a encoded user and decoded user as children of main user?
            if (Entries.Count == 0) { return "No entries."; }
            //else
            string output = "";
            for (int i = 0; i < Entries.Count; i++)
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
            if (index >= Entries.Count || index < 0) { return false; }
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
            if (title == null && entryUsername == string.Empty && entryPassword == string.Empty)
            {
                return false;
            }
            else if (!UserValidated || EntriesScrambled)
            {
                Console.WriteLine("This user has not been validated yet! Validate user before editing entries.");
                return false;
            }
            else
            {
                //This should be where encoding and decoding of entry data takes place, once it is ready
                Entries.Add(new LoginEntry(title ?? "New Entry", entryUsername, entryPassword, false));
                return true;
            }
        }

        // - - ToString method - -
        public override string ToString() => $"Name: {Username}, ID: {UserID}, Key: {OpenKey ?? "Not validated"}, # of entries: {Entries.Count}";
    }
}