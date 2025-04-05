using System.Text;
using System.Text.Json.Nodes;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("File path required.");
            return;
        }

        var filePath = Path.GetFullPath(args[0]);

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        var fileFolder = Path.GetDirectoryName(filePath)!;
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var outputFilePath = Path.Combine(fileFolder, $"{fileName}_out.csv");


        var fileText = File.ReadAllText(filePath);

        var rootNode = JsonNode.Parse(fileText);

        if (rootNode == null || rootNode is not JsonArray array || array.Count == 0)
        {
            Console.WriteLine("JSON must be an array with at least one item.");
            return;
        }

        const char separator = ';';
        var columns = new SortedSet<string>();
        var lines = new List<Dictionary<string, string>>();

        foreach (var item in array)
        {
            var flatLine = new Dictionary<string, string>();
            FlattenJson(item!, "", flatLine);
            lines.Add(flatLine);

            foreach (var key in flatLine.Keys)
                columns.Add(key);
        }

        var sb = new StringBuilder();
        sb.AppendLine(string.Join(separator, columns));

        foreach (var line in lines)
        {
            var values = columns.Select(col =>
            {
                line.TryGetValue(col, out string? value);
                value ??= "";
                value = value.Replace("\"", "\"\"");

                if (decimal.TryParse(value, out _))
                    value = $"'{value}"; // Avoid Excel auto-format

                return $"\"{value}\"";
            });

            sb.AppendLine(string.Join(separator, values));
        }

        File.WriteAllText(outputFilePath, sb.ToString());


        Console.WriteLine($"CSV Path: {outputFilePath}");

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = outputFilePath,
            UseShellExecute = true
        });
    }


    static void FlattenJson(JsonNode node, string prefix, Dictionary<string, string> result)
    {
        switch (node)
        {
            case JsonObject obj:
                foreach (var prop in obj)
                {
                    var newPrefix = string.IsNullOrEmpty(prefix) ? prop.Key : $"{prefix}.{prop.Key}";
                    FlattenJson(prop.Value!, newPrefix, result);
                }
                break;

            case JsonArray arr:
                for (int i = 0; i < arr.Count; i++)
                {
                    var newPrefix = $"{prefix}[{i}]";
                    FlattenJson(arr[i]!, newPrefix, result);
                }
                break;

            default:
                result[prefix] = node.ToString();
                break;
        }
    }
}