namespace EgyptKit.Tests;

public class NationalIdTests
{
    private static readonly DateOnly Today = new(2026, 10, 9);

    private static EgyptianNationalId Parse(string value)
    {
        Assert.True(EgyptianNationalId.TryParse(value, Today, out var id));
        return id!;
    }

    [Fact]
    public void Reads_birth_date_governorate_and_gender()
    {
        var id = Parse("30105151501234");

        Assert.Equal(new DateOnly(2001, 5, 15), id.BirthDate);
        Assert.Equal(15, id.GovernorateCode);
        Assert.Equal("Kafr El Sheikh", id.Governorate?.NameEn);
        Assert.Equal(Gender.Male, id.Gender);
        Assert.False(id.BornAbroad);
    }

    [Fact]
    public void Reads_twentieth_century_and_female()
    {
        var id = Parse("29912011234562");

        Assert.Equal(new DateOnly(1999, 12, 1), id.BirthDate);
        Assert.Equal("الدقهلية", id.Governorate?.NameAr);
        Assert.Equal(Gender.Female, id.Gender);
    }

    [Fact]
    public void Recognises_people_born_abroad()
    {
        var id = Parse("28501018800115");

        Assert.True(id.BornAbroad);
        Assert.Null(id.Governorate);
    }

    [Fact]
    public void Accepts_arabic_digits_and_spaces() =>
        Assert.Equal("30105151501234", Parse("٣٠١٠٥١٥ ١٥٠١٢٣٤").Value);

    [Theory]
    [InlineData("3010515150123")]
    [InlineData("301051515012345")]
    [InlineData("10105151501234")]
    [InlineData("30113151501234")]
    [InlineData("30102301501234")]
    [InlineData("30105159901234")]
    [InlineData("32712011501234")]
    [InlineData("3010515150123x")]
    [InlineData("")]
    public void Rejects_invalid_ids(string value) =>
        Assert.False(EgyptianNationalId.TryParse(value, Today, out _));

    [Fact]
    public void Calculates_age()
    {
        var id = Parse("30105151501234");

        Assert.Equal(25, id.AgeOn(new DateOnly(2026, 10, 9)));
        Assert.Equal(24, id.AgeOn(new DateOnly(2026, 5, 14)));
        Assert.Equal(25, id.AgeOn(new DateOnly(2026, 5, 15)));
    }

    [Fact]
    public void Masks_the_middle_digits() => Assert.Equal("301********234", Parse("30105151501234").Masked());
}
