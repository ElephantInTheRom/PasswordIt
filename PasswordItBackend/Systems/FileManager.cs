using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PasswordItBackend.Objects;

namespace PasswordItBackend.Systems
{
    public static class FileManager
    {
        public static void SaveSession(string filepath, SessionManager session) => SaveUserdata(filepath, session.SessionUsers);

        public static void SaveUserdata(string filepath, List<User> userData)
        {
            try
            {
                //This setting will make the data file readable, delete after testing.
                JsonSerializerOptions options = new JsonSerializerOptions() { 
                    WriteIndented = true                    
                };
                string jsonraw = JsonSerializer.Serialize(userData, options);
                File.WriteAllText(filepath, jsonraw);
            }
            catch(DirectoryNotFoundException ex)
            {
                //If an incorrect directory was given, must be corrected at higher level
                Console.WriteLine($"Cannot write to that directory -{ex.Message}- saving to default path.");
                throw;
            }
        }

        /// <summary>
        /// Returns a list of users with all their saved data from the filepath given
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="JsonException"></exception>
        /// <returns>The list of users</returns>
        public static List<User> LoadUserdata(string filepath)
        {
            try
            {
                string jsonraw = File.ReadAllText(filepath);
                User[]? userdata = JsonSerializer.Deserialize<User[]>(jsonraw);
                if(userdata != null) 
                {
                    return new List<User>(userdata);
                }
                else
                {
                    throw new JsonException("Nothing was found inside the data file.");
                }
            }
            catch (FileNotFoundException ex)
            {
                //If an incorrect filepath was given, must be corrected at higher level
                Console.WriteLine($"No data file with the name {filepath} could be found.");
                throw;
            }
            catch(JsonException ex)
            {
                //If the json could not be read, it might be corrupted and some sort of backup will need to kick in
                Console.WriteLine($"Issue in json deserialization - {ex.Message}");
                throw;
            }
            catch(Exception ex)
            {
                //If there was some error reading file data, should be logged in a higher level and reported to user
                Console.WriteLine($"There was an issue reading the file data - {ex.Message}");
                throw;
            }
        }
    }
}