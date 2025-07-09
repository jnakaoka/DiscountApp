using DiscountCodeSystem.Server.Models;
using DiscountCodeSystem.Server.Services;
using Moq;

namespace DiscountCodeSystem.Tests;

public class CodeGeneratorServiceTests
{
    [Fact]
    public void GenerateCodes_ShouldGenerateCorrectNumberOfCodes()
    {
        var mockStorage = new Mock<IStorageService>();
        mockStorage.Setup(s => s.LoadCodes()).Returns(new List<DiscountCode>());

        var service = new CodeGeneratorService(mockStorage.Object);
        var codes = service.GenerateCodes(10, 8);

        Assert.Equal(10, codes.Count);
        Assert.All(codes, c => Assert.False(string.IsNullOrWhiteSpace(c)));
    }

    [Fact]
    public void GenerateCodes_ShouldGenerateUniqueCodes()
    {
        var mockStorage = new Mock<IStorageService>();
        mockStorage.Setup(s => s.LoadCodes()).Returns(new List<DiscountCode>());

        var service = new CodeGeneratorService(mockStorage.Object);
        var codes = service.GenerateCodes(50, 8);
        var uniqueCount = codes.Select(c => c).Distinct().Count();

        Assert.Equal(50, uniqueCount);
    }
}
