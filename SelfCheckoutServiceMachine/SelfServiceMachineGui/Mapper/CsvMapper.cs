namespace SelfCheckoutServiceMachine.Mapper;

using System.Collections.Generic;

public class CsvMapper<T>
{
    public List<T> MapStringListToObjects(List<string> lines, Func<string[], T> mapFunction)
    {
        var objects = new List<T>();

        for (int i = 1; i < lines.Count; i++)
        {
            var line = lines[i];
            var parts = line.Split(';');

            try
            {
                var obj = mapFunction(parts);
                objects.Add(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing line: {line}. Error: {ex.Message}");
            }
        }

        return objects;
    }

    public List<string> MapObjectsToStringList(List<T> objects, Func<T, string> mapFunction)
    {
        var lines = new List<string>();

        foreach (var obj in objects)
        {
            var line = mapFunction(obj);
            lines.Add(line);
        }

        return lines;
    }
}