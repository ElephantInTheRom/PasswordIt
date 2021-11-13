using System;
using System.Collections.Generic;

namespace PasswordItBackend
{
    public static class CommandConfig
    {
        public const char cmdNew = 'n';
        public const char cmdRemove = 'r';
        public const char cmdList = 'l';
        public const char cmdSelect = 's';
        public const char cmdExit = 'e';

        public static readonly string programWelcome = $"Welcome to PasswordIt! \nType '{cmdNew}' to create a new user. " +
            $"\nType '{cmdRemove} <id>' to remove a user. \nType '{cmdList}' to list all users. \nType '{cmdSelect} <id>' to select a specific user. \nType '{cmdExit}' to exit.";

        public static readonly string userEditWelcome = $"Type '{cmdNew}' to add an entry. \nType '{cmdRemove} <number>' to remove an entry. " +
            $"\nType '{cmdList}' to list entries. \nType '{cmdExit}' to exit editing user";
    }
}
