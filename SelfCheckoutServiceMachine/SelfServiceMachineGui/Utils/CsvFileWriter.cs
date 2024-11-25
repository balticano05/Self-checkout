using System.IO;

namespace SelfCheckoutServiceMachine.Utils;

public class CsvFileWriter
{
    public static void WriteFile(string filePath, List<string> lines)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}