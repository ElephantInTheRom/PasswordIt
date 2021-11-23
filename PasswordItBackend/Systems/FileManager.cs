using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PasswordItBackend.Objects;

namespace PasswordItBackend.Systems
{
    public static class FileManager
    {
        //There may not end up being a reason to save a session once the UI is done, keep checking on this
        public static void SaveSession(string filepath, SessionManager session) => SaveList<User>(filepath, session.SessionUsers);

        /// <summary>
        /// Formats and saves a list into a json file at the specified path, used for any lists that need to be saved
        /// </summary>
        /// <typeparam name="T">The type that is stored in the list.</typeparam>
        /// <param name="filepath">The path of the file (with extension) to save to.</param>
        /// <param name="listData">The provided list to save.</param>
        public static void SaveList<T>(string filepath, List<T> listData)
        {
            try
            {
                //This setting will make the data file readable, delete after testing.
                JsonSerializerOptions options = new JsonSerializerOptions() { 
                    WriteIndented = true                    
                };
                string jsonraw = JsonSerializer.Serialize<List<T>>(listData, options);
                File.WriteAllText(filepath, jsonraw);
            }
            catch(DirectoryNotFoundException ex)
            {
                //If an incorrect directory was given, needs to save to a default path, remember to set this up
                Console.WriteLine($"Cannot write to that directory -{ex.Message}- saving to default path.");
                throw;
            }
        }

        /// <summary>
        /// Returns a list storing any type from the requested file path. 
        /// </summary>
        /// <typeparam name="T">The type that should be in the list</typeparam>
        /// <param name="filepath">The full path of the file to read from (with extension)</param>
        /// <returns>The list containing the type T</returns>
        public static List<T> LoadList<T>(string filepath)
        {
            try
            {
                string jsonraw = File.ReadAllText(filepath);
                T[]? userdata = JsonSerializer.Deserialize<T[]>(jsonraw);
                if(userdata != null) 
                {
                    return new List<T>(userdata);
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