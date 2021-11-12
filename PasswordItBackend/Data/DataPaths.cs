using System;

namespace PasswordItBackend
{
    public static class DataPaths
    {
        //Directory the exe runs from
        public static readonly string WorkingDirectory = Environment.CurrentDirectory;

        //Directory where the csproj is located
        public static readonly string ProjectDirectory =
            Directory.GetParent(WorkingDirectory).Parent.Parent.FullName;

        //Directory of the data folder
        public static readonly string DataDirectory = ProjectDirectory + @"/Data/";
    }
}
