using System;

namespace PasswordItBackend.Objects
{
    public struct LoginEntry
    {
        //Basic login will just contain a username and password
        public string? Title { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool DataScrambled { get; private set; }

        //The struct should not be in charge of scrambling or unscrambling the data, 
        //but it should know whether or not it is scrambled

        //At the very least, an entry needs a title
        public LoginEntry(string title)
        {
            Title = title;
            Username = string.Empty;
            Password = string.Empty;
            DataScrambled = false;
        }
        //An entry can contain an empty description and username if they are not needed
        public LoginEntry(string? title, string username, string password, bool scrambled)
        {
            Title = title;
            Username = username;
            Password = password;
            DataScrambled = scrambled;
        }

        //Data editing methods
         
    }
}