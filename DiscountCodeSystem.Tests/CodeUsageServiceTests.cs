using DiscountCodeSystem.Server.Models;
using DiscountCodeSystem.Server.Services;
using Moq;

namespace DiscountCodeSystem.Tests;

public class CodeUsageServiceTests
{
    [Fact]
    public void UseCode_ShouldReturn0_WhenCodeIsValid()
    {
        var code = new DiscountCode { Code = "ABC123", Used = false };
        var list = new List<DiscountCode> { code };

        var mockStorage = new Mock<IStorageService>();
        mockStorage.Setup(s => s.LoadCodes()).Returns(list);
        mockStorage.Setup(s => s.SaveCodes(It.IsAny<List<DiscountCode>>())).Verifiable();

        var service = new CodeUsageService(mockStorage.Object);
        var result = service.UseCode("ABC123");

        Assert.Equal(0, result);
        Assert.True(code.Used);
        mockStorage.Verify(s => s.SaveCodes(It.Is<List<DiscountCode>>(l => l[0].Used == true)), Times.Once);
    }

    [Fact]
    public void UseCode_ShouldReturn1_WhenCodeNotFound()
    {
        var mockStorage = new Mock<IStorageService>();
        mockStorage.Setup(s => s.LoadCodes()).Returns(new List<DiscountCode>());

        var service = new CodeUsageService(mockStorage.Object);

        var result = service.UseCode("INVALID");

        Assert.Equal(1, result);
    }

    [Fact]
    public void UseCode_ShouldReturn2_WhenCodeAlreadyUsed()
    {
        var code = new DiscountCode { Code = "XYZ789", Used = true };
        var list = new List<DiscountCode> { code };

        var mockStorage = new Mock<IStorageService>();
        mockStorage.Setup(s => s.LoadCodes()).Returns(list);

        var service = new CodeUsageService(mockStorage.Object);

        var result = service.UseCode("XYZ789");

        Assert.Equal(2, result);
    }
}
