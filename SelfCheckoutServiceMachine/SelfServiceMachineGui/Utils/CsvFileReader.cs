using System.IO;

namespace SelfCheckoutServiceMachine.Utils;

public class CsvFileReader
{
    public static List<string> ReadFile(string filePath)
    {
        try
        {
            return File.ReadAllLines(filePath).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing products file: {ex.Message}");
            return new List<string>();
        }
    }
}