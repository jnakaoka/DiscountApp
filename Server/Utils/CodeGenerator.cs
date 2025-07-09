using DiscountCodeSystem.Server.Models;
using DiscountCodeSystem.Server.Storage;

namespace DiscountCodeSystem.Server.Services;

public class CodeGeneratorService
{
    private readonly IStorageService _storage;
    private readonly Random _random = new();
    private readonly object _lock = new();

    public CodeGeneratorService(IStorageService storage)
    {
        _storage = storage;
    }

    public List<string> GenerateCodes(int count, int length)
    {
        if (length < 7 || length > 8) throw new ArgumentException("Length must be 7 or 8.");
        if (count < 1 || count > 2000) throw new ArgumentException("Max 2000 codes per request.");

        lock (_lock)
        {
            var existing = _storage.LoadCodes();
            var codes = new List<string>();

            while (codes.Count < count)
            {
                var code = GenerateRandomCode(length);
                if (existing.All(c => c.Code != code) && !codes.Contains(code))
                {
                    codes.Add(code);
                    existing.Add(new DiscountCode { Code = code });
                }
            }

            _storage.SaveCodes(existing);
            return codes;
        }
    }

    private string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
