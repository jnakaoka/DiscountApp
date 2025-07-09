using DiscountCodeSystem.Server.Models;
using DiscountCodeSystem.Server.Storage;
using System.Reflection.Emit;
using System.Text.Json;

namespace DiscountCodeSystem.Server.Services;

public class CodeUsageService
{
    private readonly IStorageService _storage;
    private readonly object _lock = new();
    private List<DiscountCode> _codes = new();

    public CodeUsageService(IStorageService storage)
    {
        _storage = storage;
    }

    // Return: 0 = Success, 1 = Code not found, 2 = Already used
    public byte UseCode(string code)
    {
        lock (_lock)
        {
            var list = _storage.LoadCodes();
            var target = list.FirstOrDefault(c => c.Code == code);

            if (target == null) return 1;
            if (target.Used) return 2;

            target.Used = true;
            _storage.SaveCodes(list);
            return 0;
        }
    }

    public List<DiscountCode> GetAllCodes()
    {
        if (!File.Exists("codes.json"))
            return new List<DiscountCode>();

        var json = File.ReadAllText("codes.json");
        Console.WriteLine(json);
        var codes = JsonSerializer.Deserialize<List<DiscountCode>>(json);
        return codes?
        .Where(c => !string.IsNullOrWhiteSpace(c.Code) && c.Code.Length <= 255)
        .ToList()
        ?? new List<DiscountCode>();
    }
}
