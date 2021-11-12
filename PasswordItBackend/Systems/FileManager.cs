using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PasswordItBackend.Objects;

namespace PasswordItBackend.Systems
{
    public static class FileManager
    {
        public static void SaveSession(string filepath, SessionManager session)
        {
            try
            {
                string jsonraw = JsonSerializer.Serialize(session);
                File.WriteAllText(filepath, jsonraw);
            }
            catch(DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Cannot write to that directory -{ex.Message}- saving to default path.");
                throw;
            }
        }

        public static SessionManager LoadSession(string filepath)
        {
            try
            {
                string jsonraw = File.ReadAllText(filepath);
                SessionManager? manager = JsonSerializer.Deserialize<SessionManager>(jsonraw);
                if(manager != null) { return manager; }
                else
                {
                    throw new Exception("Nothing was found inside the data file.");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"No data file with the name {filepath} could be found.");
                return new SessionManager();
            }
            catch(JsonException ex)
            {
                Console.WriteLine($"Issue in json deserialization - {ex.Message}");
                throw;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"There was an issue reading the file data - {ex.Message}");
                throw;
            }
        }
    }
}