using System;
using System.Collections.Generic;
using System.Linq;
using PasswordItBackend.Objects;
using PasswordItBackend.Systems;

namespace PasswordItBackend
{
    /// <summary>
    /// Session manager holds data about all the user profiles stored in this session. 
    /// Also holds behavior for setting up new user profiles and making sure all the data accosiated is correct. 
    /// </summary>
    //Should this class be static? it works as non static but there will only need to be one reference
    public class SessionManager
    {
        // - - Data - - 
        public List<User> SessionUsers { get; private set; }
        public int UserCount { get { return SessionUsers.Count; } }

        // - - Constructors - - 
        public SessionManager()
        {
            SessionUsers = new();
        }

        public SessionManager(List<User> userData)
        {
            SessionUsers = userData;
        }

        // - - Closing and opening up user data - - 
        public void OpenSessionFromFile(string filepath)
        {
            SessionUsers = FileManager.LoadList<User>(filepath);
            //Do some exception handling here         
        }

        public void SealAndSaveSession(string filepath)
        {
            //Encode all user data
            foreach(var user in SessionUsers)
            {
                user.LockUser();
            }
            //Save list of users to the specified save file
            FileManager.SaveList(filepath, SessionUsers);
            UserIDManager.SaveIDList();
            //Send out program ending events
            Console.WriteLine("Data encoded and saved successfully");
        }

        // - - Retriving the list - - 
        public User[] GetSessionList() => SessionUsers.ToArray();

        /// <summary>
        /// Gets a string with a list of all users in alphebetical order and formatted to be readable.
        /// </summary>
        public string GetFormattedSessionList()
        {
            User[] list = (from user in SessionUsers
                          orderby user.Username ascending
                          select user).ToArray();
            string output = "";
            foreach(User user in list)
            {
                output += user.ToString() + "\n";
            }
            return output;
        }

        // - - Editing user list - - 
        /// <summary>
        /// Create a new user and add it to the session list of users.
        /// </summary>
        /// <param name="username">The username for the account.</param>
        /// <param name="userkey">The encoding and decoding key for the account.</param>
        /// <returns>The newly created user.</returns>
        public User CreateUser(string username, string userkey)
        {
            User newUser = new User(username, userkey);
            SessionUsers.Add(newUser);
            return newUser;
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
        /// Removes a user from the list by its unique ID.
        /// </summary>
        /// <param name="id">The id of the user to remove.</param>
        /// <returns>True or false depending on success.</returns>
        public bool RemoveUser(int id)
        {
            foreach (User u in SessionUsers)
            {
                if (u.UserID == id)
                {
                    SessionUsers.Remove(u);
                    return true;
                }
            }
            return false;
        }
    }
}