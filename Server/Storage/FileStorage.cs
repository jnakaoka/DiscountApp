using DiscountCodeSystem.Server.Models;
using System.Text.Json;

namespace DiscountCodeSystem.Server.Storage;

public class FileStorageService : IStorageService
{
    private readonly string _filePath = "codes.json";
    //private readonly object _lock = new();

    public List<DiscountCode> LoadCodes()
    {
        if (!File.Exists(_filePath))
            return new List<DiscountCode>();

        var json = File.ReadAllText(_filePath);
        return System.Text.Json.JsonSerializer.Deserialize<List<DiscountCode>>(json) ?? new List<DiscountCode>();
        //lock (_lock)
        //{
        //    if (!File.Exists(_filePath)) return new List<DiscountCode>();
        //    var json = File.ReadAllText(_filePath);
        //    return JsonSerializer.Deserialize<List<DiscountCode>>(json) ?? new();
        //}
    }

    public void SaveCodes(List<DiscountCode> codes)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(codes);
        File.WriteAllText(_filePath, json);
        //lock (_lock)
        //{
        //    var json = JsonSerializer.Serialize(codes);
        //    File.WriteAllText(_filePath, json);
        //}
    }
}
