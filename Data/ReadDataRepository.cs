using FjernvarmeMaalingApp.Data.Interfaces;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.Data;

public class ReadDataRepository(ILogger<ReadDataRepository> logger) : IReadDataRepository
{
    private readonly ILogger<ReadDataRepository> _logger = logger;
    private readonly string _filePath = "C:/temp/data.json";

    public async Task<string> ReadData(User? user)
    {
        string result = string.Empty;
        try
        {
            using (FileStream fs = new (_filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new (fs))
                {
                    string json = await sr.ReadToEndAsync();
                    List<string> jsonItems = SplitJsonObjects(json);
                    if (jsonItems.Count == 0) _logger.LogWarning("No data found in file");
                    if (user == null)
                    {
                        return json;
                    }

                    var measurements = jsonItems
                        .Select(jsonItem => JsonHelper.DeserializeObject<Measurement>(jsonItem))
                        .Where(measurement => measurement.RegisteredBy == user.Username)
                        .Select(m => JsonHelper.SerializeObject(m))
                        .ToList();
                    result = string.Join(",", measurements);

                    //foreach (string jsonItem in jsonItems) 
                    //{
                    //    var measurement = JsonHelper.DeserializeObject<Measurement>(jsonItem);
                    //    if (user != null && measurement.RegisteredBy == user.Username)
                    //    {
                    //        result += JsonHelper.SerializeObject(measurement) + ",";
                    //    }
                    //    else if (user == null)
                    //    {
                    //        result += JsonHelper.SerializeObject(measurement) + ",";
                    //    }
                    //}
                    return result;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while reading data");
            return string.Empty;
        }
    }
    public static List<string> SplitJsonObjects(string jsonString)
    {
        if (jsonString.Length <= 2) return [];        
        var jsonObjects = new List<string>();
        int depth = 0;
        int startIndex = 0;
        for (int i = 0; i < jsonString.Length; i++)
        {
            if (jsonString[i] == '{')
            {
                if (depth == 0)
                {
                    startIndex = i;
                }
                depth++;
            }
            else if (jsonString[i] == '}')
            {
                depth--;
                if (depth == 0)
                {
                    jsonObjects.Add(jsonString.Substring(startIndex, i - startIndex + 1));
                }
            }
        }
        return jsonObjects;
    }
}
