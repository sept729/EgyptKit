namespace EgyptKit.Tests;

public class ArabicTextTests
{
    [Theory]
    [InlineData("أحمد", "احمد")]
    [InlineData("إسلام", "اسلام")]
    [InlineData("آمنة", "امنه")]
    [InlineData("مصطفى", "مصطفي")]
    [InlineData("مُحَمَّد", "محمد")]
    [InlineData("جـــميل", "جميل")]
    [InlineData("مسؤول", "مسوول")]
    [InlineData("قائمة", "قايمه")]
    [InlineData("  تيشيرت    قطن  ", "تيشيرت قطن")]
    [InlineData("Shirt ٣ XL", "shirt 3 xl")]
    public void Normalizes_for_search(string input, string expected) =>
        Assert.Equal(expected, ArabicText.Normalize(input));

    [Theory]
    [InlineData("تيشيرت قطن أبيض", "ابيض")]
    [InlineData("تيشيرت قطن أبيض", "قطن تيشيرت")]
    [InlineData("فستان سهرة", "سهره")]
    [InlineData("Cotton Shirt", "cotton")]
    [InlineData("anything", "")]
    public void Matches_queries_regardless_of_spelling(string text, string query) =>
        Assert.True(ArabicText.Matches(text, query));

    [Fact]
    public void Does_not_match_missing_words() => Assert.False(ArabicText.Matches("تيشيرت قطن", "جينز"));

    [Theory]
    [InlineData("مرحبا", true)]
    [InlineData("hello", false)]
    [InlineData("hello مرحبا", true)]
    [InlineData("", false)]
    public void Detects_arabic(string text, bool expected) => Assert.Equal(expected, ArabicText.ContainsArabic(text));

    [Fact]
    public void Converts_digits_both_ways()
    {
        Assert.Equal("2026", Digits.ToLatin("٢٠٢٦"));
        Assert.Equal("123", Digits.ToLatin("۱۲۳"));
        Assert.Equal("١٬٢٥٠٫٥٠", Digits.ToArabic("1,250.50"));
        Assert.Equal("01012345678", Digits.Only("٠١٠-١٢٣ ٤٥٦٧٨"));
    }
}
