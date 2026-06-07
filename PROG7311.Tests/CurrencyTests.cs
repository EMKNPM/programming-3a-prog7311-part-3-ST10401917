using Xunit;
using System;

public class CurrencyTests
{
    [Fact]
    public void ConvertUsdToZar_ShouldReturnCorrectValue()
    {
        // Arrange
        decimal usd = 100;
        decimal rate = 18.5m;

        var service = new CurrencyServiceMock(rate);

        // Act
        var result = service.Convert(usd);

        // Assert
        Assert.Equal(1850, result);
    }

    [Fact]
    public void Convert_ShouldThrow_WhenUsdIsNegative()
    {
        var service = new CurrencyServiceMock(18.5m);

        Assert.Throws<ArgumentException>(() => service.Convert(-100));
    }

    [Fact]
    public void Convert_ShouldThrow_WhenUsdIsZero()
    {
        var service = new CurrencyServiceMock(18.5m);

        Assert.Throws<ArgumentException>(() => service.Convert(0));
    }

    [Fact]
    public void Convert_ShouldThrow_WhenRateIsInvalid()
    {
        var service = new CurrencyServiceMock(0);

        Assert.Throws<ArgumentException>(() => service.Convert(100));
    }

    [Fact]
    public void Convert_ShouldHandleLargeValues()
    {
        var service = new CurrencyServiceMock(18.5m);

        var result = service.Convert(1000000);

        Assert.Equal(18500000, result);
    }
}

// Mock service
public class CurrencyServiceMock
{
    private readonly decimal _rate;

    public CurrencyServiceMock(decimal rate)
    {
        _rate = rate;
    }

    public decimal Convert(decimal usd)
    {
        if (usd <= 0)
            throw new ArgumentException("USD must be greater than 0");

        if (_rate <= 0)
            throw new ArgumentException("Rate must be greater than 0");

        return usd * _rate;
    }
}



