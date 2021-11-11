// See https://aka.ms/new-console-template for more information
using PasswordItBackend;

/* List of needs:
 * Create users
 * List session users
 * Enter into a user: add entries, remove entries and edit entries
 * Exit user and select new ones
*/


SessionManager session = new();

Console.WriteLine("Welcome to PasswordIt! \nType 'c' to create a new user. \nType 'l' to list all users. \nType 'e' to exit." +
    " \nType 'r <id>' to remove a user. \nType 's <id> to show a specific user.'");

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

//s 1235
//012345
//6 - 4

void ShowUser(string command)
{
    int id;
    if(command.Length >= 5 && int.TryParse(command[(command.Length - 4)..], out id))
    {

    }
    else
    {
        Console.WriteLine("Not a valid user id");
    }
    
}

void RemoveLoser(string command)
{

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