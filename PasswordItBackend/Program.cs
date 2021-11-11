// See https://aka.ms/new-console-template for more information
using PasswordItBackend;

/* List of needs:
 * Create users
 * List session users
 * Enter into a user: add entries, remove entries and edit entries
 * Exit user and select new ones
*/


SessionManager session = new();

Console.WriteLine("Welcome to PasswordIt! Type 'c' to create a new user. Type 'l' to list all users. Type 'e' to exit.");

while(true)
{
    string? command = Console.ReadLine();
    if(command != null)
    {
        if(command[0] == 'c')
        {
            CreateUser();
            Console.WriteLine("\n");
        }
        else if(command[0] == 'l')
        {
            ListUsers();
            Console.WriteLine("\n");
        }
        else if(command[0] == 'e')
        {
            Console.WriteLine("Exiting program . . .");
            break;
        }
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