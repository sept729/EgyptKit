namespace EgyptKit.Tests;

public class SlugTests
{
    [Theory]
    [InlineData("تيشيرت قطن أبيض", "تيشيرت-قطن-ابيض")]
    [InlineData("Cotton Shirt - XL!", "cotton-shirt-xl")]
    [InlineData("فستان، سهرة؟", "فستان-سهره")]
    [InlineData("  --hello--  ", "hello")]
    [InlineData("", "")]
    public void Keeps_arabic_letters_by_default(string input, string expected) =>
        Assert.Equal(expected, Slug.Create(input));

    [Theory]
    [InlineData("متجر أحمد", "mtgr-ahmd")]
    [InlineData("شياكة", "shyakh")]
    [InlineData("Lamsa Store", "lamsa-store")]
    [InlineData("بيت الحيوانات 2", "byt-alhywanat-2")]
    public void Transliterates_to_latin(string input, string expected) =>
        Assert.Equal(expected, Slug.CreateLatin(input));

    [Fact]
    public void Respects_the_max_length_without_cutting_words()
    {
        var slug = Slug.Create("one two three four five six", 15);

        Assert.True(slug.Length <= 15);
        Assert.False(slug.EndsWith('-'));
        Assert.Equal("one-two-three", slug);
    }

    [Fact]
    public void Suggests_subdomains()
    {
        Assert.Equal("mtgr-ahmd", Slug.Subdomain("متجر أحمد"));
        Assert.Equal(string.Empty, Slug.Subdomain("؟"));
    }
}
