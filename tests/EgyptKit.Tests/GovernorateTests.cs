namespace EgyptKit.Tests;

public class GovernorateTests
{
    [Fact]
    public void Has_all_27_governorates_with_unique_codes()
    {
        Assert.Equal(27, Governorates.All.Count);
        Assert.Equal(27, Governorates.All.Select(g => g.Code).Distinct().Count());
    }

    [Theory]
    [InlineData("Cairo", 1)]
    [InlineData("القاهرة", 1)]
    [InlineData("القاهره", 1)]
    [InlineData("قاهرة", 1)]
    [InlineData("  cairo ", 1)]
    [InlineData("الإسكندرية", 2)]
    [InlineData("اسكندرية", 2)]
    [InlineData("Alex", 2)]
    [InlineData("كفر الشيخ", 15)]
    [InlineData("Kafr El-Sheikh", 15)]
    [InlineData("kafrelsheikh", 15)]
    [InlineData("Menoufia", 17)]
    [InlineData("بور سعيد", 3)]
    [InlineData("البحر الأحمر", 31)]
    [InlineData("محافظة الجيزة", 21)]
    [InlineData("Giza Governorate", 21)]
    public void Finds_governorates_by_many_spellings(string name, int code) =>
        Assert.Equal(code, Governorates.Find(name)?.Code);

    [Theory]
    [InlineData("Paris")]
    [InlineData("")]
    [InlineData(null)]
    public void Unknown_names_return_null(string? name) => Assert.Null(Governorates.Find(name));

    [Fact]
    public void Groups_governorates_into_shipping_zones()
    {
        Assert.Equal(ShippingZone.GreaterCairo, Governorates.Find("Giza")!.Zone);
        Assert.Equal(ShippingZone.Remote, Governorates.Find("مطروح")!.Zone);
        Assert.Equal(3, Governorates.InZone(ShippingZone.GreaterCairo).Count());
        Assert.Equal(27, Enum.GetValues<ShippingZone>().Sum(z => Governorates.InZone(z).Count()));
    }

    [Fact]
    public void Looks_up_by_national_id_code() => Assert.Equal("Luxor", Governorates.FromCode(29)?.NameEn);
}
