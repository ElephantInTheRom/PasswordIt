using System;
using System.Collections.Generic;
using System.Linq;
using PasswordItBackend.Objects;
using PasswordItBackend.Systems;

namespace PasswordItBackend
{
    public class SessionManager
    {
        //Session lists
        public static List<User> SessionUsers { get; private set; } = new();

        /// <summary>
        /// Create a new user and add it to the session list of users.
        /// </summary>
        /// <param name="username">The username for the account.</param>
        /// <param name="userkey">The encoding and decoding key for the account.</param>
        /// <returns>The newly created user.</returns>
        public User CreateUser(string username, string userkey)
        {
            int id = UserIDManager.GenerateUniqueID();
            User newUser = new User(username, id, userkey, true);
            SessionUsers.Add(newUser);
            return newUser;
        }

        //Retriving users
        public User[] GetSessionList() => SessionUsers.ToArray();
        /// <summary>
        /// Gets a string with a list of all users in alphebetical order and formatted to be readable.
        /// </summary>
        public string GetFormattedSessionList()
        {
            User[] list = (from user in SessionUsers
                          orderby user.Username descending
                          select user).ToArray();
            string output = "";
            foreach(User user in list)
            {
                output += user.ToString() + "\n";
            }
            return output;
        }

        /// <summary>
        /// Gets a user by its unique ID.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <param name="user">The user if one is found</param>
        /// <returns>True if a user is found, false if not.</returns>
        public bool GetUser(int id, out User? user)
        {
            foreach (User u in SessionUsers)
            {
                if(u.UserID == id)
                {
                    user = u;
                    return true;
                }
            }
            user = null;
            return false;
        }
        /// <summary>
        /// Gets a user by its name, if two accounts share the same name, the oldest created one will be picked.
        /// </summary>
        /// <param name="name">The username to search for.</param>
        /// <param name="user">The out user if one is found.</param>
        /// <returns>True if a user is found, false if not.</returns>
        public bool GetUser(string name, out User? user)
        {
            foreach (User u in SessionUsers)
            {
                if (u.Username == name)
                {
                    user = u;
                    return true;
                }
            }
            user = null;
            return false;
        }
    }
}