namespace EgyptKit.Tests;

public class ArabicNumbersTests
{
    [Theory]
    [InlineData(0, "صفر")]
    [InlineData(1, "واحد")]
    [InlineData(11, "أحد عشر")]
    [InlineData(20, "عشرون")]
    [InlineData(21, "واحد وعشرون")]
    [InlineData(100, "مائة")]
    [InlineData(200, "مائتان")]
    [InlineData(305, "ثلاثمائة وخمسة")]
    [InlineData(1000, "ألف")]
    [InlineData(1001, "ألف وواحد")]
    [InlineData(1250, "ألف ومائتان وخمسون")]
    [InlineData(2000, "ألفان")]
    [InlineData(3000, "ثلاثة آلاف")]
    [InlineData(11000, "أحد عشر ألف")]
    [InlineData(100000, "مائة ألف")]
    [InlineData(2500000, "مليونان وخمسمائة ألف")]
    [InlineData(1000000000, "مليار")]
    public void Spells_numbers_in_arabic(long number, string expected) =>
        Assert.Equal(expected, ArabicNumbers.ToWords(number));

    [Theory]
    [InlineData(1250, "فقط ألف ومائتان وخمسون جنيه مصري لا غير")]
    [InlineData(99.5, "فقط تسعة وتسعون جنيه مصري وخمسون قرش لا غير")]
    [InlineData(0.25, "فقط خمسة وعشرون قرش لا غير")]
    [InlineData(0, "فقط صفر جنيه مصري لا غير")]
    public void Spells_amounts_for_invoices(double amount, string expected) =>
        Assert.Equal(expected, ArabicNumbers.Pounds((decimal)amount));

    [Theory]
    [InlineData(1250, false, "1,250 ج.م")]
    [InlineData(1250.5, false, "1,250.50 ج.م")]
    [InlineData(1250.5, true, "١٬٢٥٠٫٥٠ ج.م")]
    public void Formats_pounds(double amount, bool arabic, string expected) =>
        Assert.Equal(expected, ArabicNumbers.FormatPounds((decimal)amount, arabic));

    [Fact]
    public void Negative_amounts_are_rejected() => Assert.Throws<ArgumentOutOfRangeException>(() => ArabicNumbers.Pounds(-1));
}
