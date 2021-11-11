// See https://aka.ms/new-console-template for more information
using PasswordItBackend;
using PasswordItBackend.Objects;

/* List of needs:
 * Create users
 * List session users
 * Enter into a user: add entries, remove entries and edit entries
 * Exit user and select new ones
*/


SessionManager session = new();
bool editingUser = false;
User? selectedUser = null;

Console.WriteLine("Welcome to PasswordIt! \nType 'c' to create a new user. \nType 'l' to list all users. \nType 'e' to exit." +
    " \nType 'r <id>' to remove a user. \nType 's <id> to select a specific user.'");

//Create users to start with
var u1 = session.CreateUser("Hayden", "apple");
var u2 = session.CreateUser("Brooke", "bee");
var u3 = session.CreateUser("Jessica", "music");
//Add entries to thes users to start with
u1.AddEntry("Amazon", "HaydenCJ", "marbles123");
u1.AddEntry("Discord", "Elephant", "cookie123");
u1.AddEntry("Google", "haydenJ", "cookie715");
u2.AddEntry("Amazon", "QueenB", "bee412");


while (true)
{
    string? command = Console.ReadLine();
    if(command != null)
    {
        if(editingUser)
        {
            if(command[0] == 'a') { AddEntry(); }
            else if(command[0] == 'r') { RemoveEntry(command); }
            else if(command[0] == 'l') { ListEntries(); }
            else if(command[0] == 'e')
            {
                Console.WriteLine("No longer editing user");
                editingUser = false;
                selectedUser = null;
            }
        }
        else
        {
            if (command[0] == 'c') { CreateUser(); }
            else if (command[0] == 'l') { ListUsers(); }
            else if (command[0] == 's') { SelectUser(command); }
            else if (command[0] == 'r') { RemoveUser(command); }
            else if (command[0] == 'e')
            {
                Console.WriteLine("Exiting program . . .");
                break;
            }
        }

        Console.WriteLine("\n");
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

    if (selectedUser.AddEntry(entryTitle, entryUsername, entryPassword))
    {
        Console.WriteLine($"New entry added");
    }
    else
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
        if (int.TryParse(sub, out entryNum) && selectedUser.RemoveEntry(entryNum))
        {
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
            Console.WriteLine($"Now editing the user: {user.Username} <{user.UserID}>.");
            Console.WriteLine($"Type 'a' to add an entry. \nType 'r <number>' to remove an entry. \nType 'l' to list entries. " +
                $"\nType 'e' to exit editing user");
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
    Console.WriteLine(session.GetFormattedSessionList());
}