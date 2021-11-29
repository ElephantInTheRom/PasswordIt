global using PasswordItBackend.Exceptions;
using PasswordItBackend;
using PasswordItBackend.Objects;
using PasswordItBackend.Systems;
using static PasswordItBackend.CommandConfig;

//fields used by the executing program
bool editingUser = false;
User? selectedUser = null;
string saveFilePath = DataPaths.DataDirectory + @"SessionData.json";
SessionManager session = new SessionManager();

//Try-catch for loading data, injects starting data into the session if the save file is gone somehow
try
{
    session.OpenSessionFromFile(saveFilePath);
}
catch (FileNotFoundException ex) 
{
    Console.WriteLine($"No data file - {ex.FileName} -  was found, starting with test data.");
    session = new();
    //This is probably not good practice for exception handling, but its good for testing
    var u1 = session.CreateUser("Hayden", "apple");
    var u2 = session.CreateUser("Brooke", "bee");
    var u3 = session.CreateUser("Jessica", "music");
    var u4 = session.CreateUser("Jason", "volvo");
    //Add entries to these users to start with
    u1.AddEntry("Amazon", "HaydenCJ", "marbles123");
    u1.AddEntry("Discord", "Elephant", "cookie123");
    u1.AddEntry("Google", "haydenJ", "cookie715");
    u2.AddEntry("Amazon", "QueenB", "bee412");
    u4.AddEntry(null, "lazyj", "guitar123");
}

Console.WriteLine(programWelcome);

while (true)
{
    string? command = Console.ReadLine();
    if(command != null)
    {
        if(editingUser)
        {
            if(command[0] == cmdNew) { AddEntry(); }
            else if(command[0] == cmdRemove) { RemoveEntry(command); }
            else if(command[0] == cmdList) { ListEntries(); }
            else if(command[0] == cmdExit)
            {
                Console.WriteLine("No longer editing user");
                editingUser = false;
                selectedUser = null;
            }
        }
        else
        {
            if (command[0] == cmdNew) { CreateUser(); }
            else if (command[0] == cmdList) { ListUsers(); }
            else if (command[0] == cmdSelect) { SelectUser(command); }
            else if (command[0] == cmdRemove) { RemoveUser(command); }
            else if (command[0] == cmdExit)
            {
                Console.WriteLine("Exiting program . . .");
                //Save data to a file
                session.SealAndSaveSession(saveFilePath);
                break;
            }
        }

        Console.WriteLine("");
    }
}

//Scoping into and editing user entries
void ListEntries()
{
    if (selectedUser == null)
    {
        Console.WriteLine("No user selected");
        return;
    }

    Console.WriteLine(selectedUser.GetEntryList());
}

void AddEntry()
{
    Console.WriteLine("Give a title for the entry or press enter to keep it blank");
    string? entryTitle = Console.ReadLine();
    Console.WriteLine("Give a username or account name for the entry.");
    string entryUsername = Console.ReadLine() ?? string.Empty;
    Console.WriteLine("Give a password or keyword for the entry.");
    string entryPassword = Console.ReadLine() ?? string.Empty;
    if (selectedUser == null)
    {
        Console.WriteLine("No user selected");
        return;
    }

    try
    {
        selectedUser.AddEntry(entryTitle, entryUsername, entryPassword);
        Console.WriteLine($"New entry added");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Not enough data provided, please try again");
    }   
}

void RemoveEntry(string command)
{
    if (selectedUser == null) 
    { 
        Console.WriteLine("No user selected");
        return;
    }

    int index = command.LastIndexOf(' ');
    if(index != -1)
    {
        string sub = command[(index + 1)..];
        int entryNum;
        if (int.TryParse(sub, out entryNum))
        {
            selectedUser.RemoveEntry(entryNum);
            Console.WriteLine($"Removed entry {entryNum}.");
        }
        else
        {
            Console.WriteLine("Please enter valid number");
        }
    }
    else
    {
        Console.WriteLine("Please enter valid number");
    }
}

void SelectUser(string command)
{
    int id;
    User user;
    if(command.Length >= 5 && int.TryParse(command[(command.Length - 4)..], out id))
    {
        if(session.GetUser(id, out user))
        {
            //Enter the password for the user
            if(!user.UserValidated)
                Console.WriteLine("Please enter the users password: ");
            while (!user.UserValidated)
            {               
                string? key = Console.ReadLine();
                if(key != null && user.UnlockUser(key))
                {
                    Console.WriteLine("Correct key given.");
                    break;
                }
                else
                {
                    Console.WriteLine("Incorrect password. Please try again: ");
                }
            }
            //After key has been given
            Console.WriteLine($"Now editing the user: {user.Username} <{user.UserID}>.");
            Console.WriteLine(userEditWelcome);
            editingUser = true;
            selectedUser = user;
        }
        else
        {
            Console.WriteLine("Not a valid user id");
        }
    }
    else
    {
        Console.WriteLine("Not a valid user id");
    }  
}

//Editing and accessing the list of users
void RemoveUser(string command)
{
    int id;
    if (command.Length >= 5 && int.TryParse(command[(command.Length - 4)..], out id))
    {
        if (session.RemoveUser(id))
        {
            Console.WriteLine($"User <{id}> was removed.");
        }
        else
        {
            Console.WriteLine("Not a valid user id");
        }
    }
    else
    {
        Console.WriteLine("Not a valid user id");
    }
}

void CreateUser()
{
    string name;
    string key;

    Console.WriteLine("Enter new name for the user . . .");
    name = Console.ReadLine();
    Console.WriteLine("Enter new key for the user . . .");
    key = Console.ReadLine();

    var newUser = session.CreateUser(name, key);

    Console.WriteLine($"New user created!");
    Console.WriteLine(newUser.ToString());
}

void ListUsers()
{
    Console.Write(session.GetFormattedSessionList());
}