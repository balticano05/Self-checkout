using System.IO;

namespace SelfCheckoutServiceMachine.Utils;

public class CsvFileWriter
{
    public static void WriteFile(string filePath, List<string> lines)
    {
        try
        {
            File.WriteAllLines(filePath, lines.AsEnumerable());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}