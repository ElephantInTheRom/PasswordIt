using PasswordItBackend.Systems;
using System.Text.Json.Serialization;
using static PasswordItBackend.Systems.DataScrambler;

//User key should be encoded on itself when registering a user

namespace PasswordItBackend.Objects
{
    public class User
    {
        // - - User data
        public string Username { get; private set; }
        public int UserID { get; private set; }
        public List<LoginEntry> Entries { get; private set; }

        // - - User validation data
        public string ScrMasterKey { get; init; } //The Scramblers master key encoded with this users password
        [JsonIgnore] private string? OpenUserKey { get; set; } //Once the user is unlocked, the users password

        // - - User state
        [JsonIgnore] public bool UserValidated { get; private set; }
        [JsonIgnore] public bool EntriesScrambled { get; private set; }

        // - - Constructors - - 
        [JsonConstructor]
        public User(string username, int userid, string scrMasterKey, List<LoginEntry> entries)
        {
            //This constructor will be used by the json deserializer
            Username = username;
            UserID = userid;
            ScrMasterKey = scrMasterKey;
            Entries = entries;
            OpenUserKey = null;
            UserValidated = false;
            EntriesScrambled = true;
        }
        //Should be used when registering a new user in the program
        public User(string? username, string key)
        {
            Entries = new();
            UserID = UserIDManager.GenerateUniqueID();
            Username = username ?? $"User {UserID}";           
            ScrMasterKey = GenerateScrambledMasterKey(key); //TODO : Change the name of this to better fit what it is and then change its logic
            OpenUserKey = key;
            UserValidated = true;
            EntriesScrambled = false;
        }

        // - - Locking and unlocking the user - - 
        public void LockUser()
        {
            if (!EntriesScrambled)
            {
                EncodeEntries();
                UserValidated = false;
                OpenUserKey = null;
            }
        }

        public bool UnlockUser(string key)
        {
            if (ConfirmKeyAttempt(key, ScrMasterKey))
            {
                UserValidated = true;
                OpenUserKey = key;
                DecodeEntries();              
                return true;
            }
            else
            {
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
                    var username = UnscrambleOnKey(entry.Username, OpenUserKey);
                    var password = UnscrambleOnKey(entry.Password, OpenUserKey);
                    var e = new LoginEntry(title, username, password, false);
                    decodedEntries.Add(e);
                }
                Entries = decodedEntries;
                EntriesScrambled = false;
            }

        }

        private void EncodeEntries()
        {
            if(UserValidated)
            {
                List<LoginEntry> encodedEntries = new();
                foreach (var entry in Entries)
                {
                    var title = entry.Title;
                    var username = ScrambleOnKey(entry.Username, OpenUserKey);
                    var password = ScrambleOnKey(entry.Password, OpenUserKey);
                    var e = new LoginEntry(title, username, password, true);
                    encodedEntries.Add(e);
                }
                Entries = encodedEntries;
                EntriesScrambled = true;
            }
        }

        // - - Editing entry list - - 
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

        public void RemoveEntry(int entryNum)
        {
            int index = entryNum - 1;
            if (index >= Entries.Count || index < 0) 
                throw new IndexOutOfRangeException("That entry number does not exist and is outside the bounds of the array.");
            //
            Entries.Remove(Entries[index]);
        }

        public void AddEntry(string? title, string entryUsername, string entryPassword)
        {
            if (title == null && entryUsername == string.Empty && entryPassword == string.Empty)
                throw new ArgumentException("Cannot create an entry with empty properties!");
            if (!UserValidated || EntriesScrambled)
                throw new UserNotValidatedException("The user has not been validated yet, please validate before accessing data.", this);
            //
            Entries.Add(new LoginEntry(title ?? "New Entry", entryUsername, entryPassword, false));
        }

        // - - ToString method - -
        public override string ToString() => $"Name: {Username}, ID: {UserID}, Key: {OpenUserKey ?? "Not validated"}, # of entries: {Entries.Count}";
    }
}