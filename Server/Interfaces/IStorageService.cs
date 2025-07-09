using DiscountCodeSystem.Server.Models;

public interface IStorageService
{
    List<DiscountCode> LoadCodes();
    void SaveCodes(List<DiscountCode> codes);
}