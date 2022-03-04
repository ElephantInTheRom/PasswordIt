using System.IO;
using System.Text;

string testData = "This is some test data 12345678::::";
byte[] testDataBytes = Encoding.UTF8.GetBytes(testData);
BinaryWriter writer;
BinaryReader reader;

//Create the file
try
{
    writer = new BinaryWriter(new FileStream(@"C:\Users\hayde\Desktop\testDataBytes", FileMode.Create));
}
catch (IOException ex)
{
    Console.WriteLine($"Cannot create test file: {ex.Message}");
    return;
}

//Write into the file
try
{
    writer.Write(testDataBytes);
}
catch(IOException ex)
{
    Console.WriteLine($"Could not write test data into the file: {ex.Message}");
}

writer.Close();

//Open the file
try
{
    reader = new BinaryReader(new FileStream(@"C:\Users\hayde\Desktop\testData", FileMode.Open));
}
catch(IOException ex)
{
    Console.WriteLine($"Cannot open the file: {ex.Message}");
    return;
}

//Read from the file
try
{
    string outputString = reader.ReadString();
    Console.WriteLine($"Input data: {testData} || Output data: {outputString}");
}
catch(IOException ex)
{
    Console.WriteLine($"Cannot read from the file: {ex.Message}");
}

reader.Close();