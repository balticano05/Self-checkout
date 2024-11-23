namespace SelfCheckoutServiceMachine.Utils;

public class CsvFileReader
{
    public static List<string> ReadFile(string filePath)
    {
        var lines = new List<string>();

        try
        {
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing products file: {ex.Message}");
        }

        return lines;
    }
}