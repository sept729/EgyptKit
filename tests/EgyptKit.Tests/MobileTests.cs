namespace EgyptKit.Tests;

public class MobileTests
{
    [Theory]
    [InlineData("01012345678", "01012345678")]
    [InlineData("010 1234 5678", "01012345678")]
    [InlineData("010-1234-5678", "01012345678")]
    [InlineData("+201012345678", "01012345678")]
    [InlineData("00201012345678", "01012345678")]
    [InlineData("201012345678", "01012345678")]
    [InlineData("1012345678", "01012345678")]
    [InlineData("٠١٠١٢٣٤٥٦٧٨", "01012345678")]
    [InlineData("(+20) 115 555 0000", "01155550000")]
    public void Accepts_common_ways_of_writing_a_number(string input, string expected) =>
        Assert.Equal(expected, EgyptianMobile.Normalize(input));

    [Theory]
    [InlineData("")]
    [InlineData("0101234567")]
    [InlineData("010123456789")]
    [InlineData("01312345678")]
    [InlineData("02012345678")]
    [InlineData("0223456789")]
    [InlineData("+966501234567")]
    [InlineData("010abc45678")]
    [InlineData(null)]
    public void Rejects_numbers_that_are_not_egyptian_mobiles(string? input) =>
        Assert.False(EgyptianMobile.IsValid(input));

    [Theory]
    [InlineData("01012345678", MobileCarrier.Vodafone)]
    [InlineData("01112345678", MobileCarrier.Etisalat)]
    [InlineData("01212345678", MobileCarrier.Orange)]
    [InlineData("01512345678", MobileCarrier.WE)]
    public void Detects_the_carrier(string input, MobileCarrier carrier) =>
        Assert.Equal(carrier, EgyptianMobile.Parse(input).Carrier);

    [Fact]
    public void Exposes_every_format()
    {
        var m = EgyptianMobile.Parse("010 1234 5678");

        Assert.Equal("01012345678", m.National);
        Assert.Equal("+201012345678", m.E164);
        Assert.Equal("201012345678", m.International);
        Assert.Equal("010 1234 5678", m.Format());
        Assert.Equal("tel:+201012345678", m.TelLink());
    }

    [Fact]
    public void Builds_whatsapp_links()
    {
        var m = EgyptianMobile.Parse("01012345678");

        Assert.Equal("https://wa.me/201012345678", m.WhatsAppLink());
        Assert.Equal("https://wa.me/201012345678?text=%D8%A3%D9%87%D9%84%D8%A7%D9%8B%20%23123", m.WhatsAppLink("أهلاً #123"));
    }

    [Fact]
    public void Parse_throws_on_bad_input() => Assert.Throws<FormatException>(() => EgyptianMobile.Parse("123"));
}
