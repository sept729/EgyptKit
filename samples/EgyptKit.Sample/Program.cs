using System.Text;
using EgyptKit;

Console.OutputEncoding = Encoding.UTF8;

Section("Mobile numbers");
foreach (var input in new[] { "010 1234 5678", "+20 115 555 0000", "٠١٢٣٤٥٦٧٨٩٠", "0123" })
{
    if (EgyptianMobile.TryParse(input, out var m))
        Console.WriteLine($"{input,-20} -> {m.Value.National}  {m.Value.Carrier,-9} {m.Value.WhatsAppLink()}");
    else
        Console.WriteLine($"{input,-20} -> not a valid Egyptian mobile");
}

Section("National ID");
if (EgyptianNationalId.TryParse("30105151501234", out var id))
    Console.WriteLine($"{id.Masked()}  born {id.BirthDate:yyyy-MM-dd}  {id.Gender}  {id.Governorate?.NameEn} / {id.Governorate?.NameAr}");

Section("Governorates");
foreach (var name in new[] { "القاهره", "Alex", "kafr el-sheikh", "محافظة مطروح" })
{
    var g = Governorates.Find(name);
    Console.WriteLine($"{name,-18} -> {g?.NameEn ?? "?",-15} {g?.NameAr ?? "",-12} zone: {g?.Zone}");
}

Section("Arabic search");
var products = new[] { "تيشيرت قطن أبيض", "فستان سهرة", "جاكيت جينز" };
foreach (var query in new[] { "ابيض", "سهره", "جينز جاكيت" })
    Console.WriteLine($"{query,-12} -> {string.Join(", ", products.Where(p => ArabicText.Matches(p, query)))}");

Section("Slugs");
Console.WriteLine($"{Slug.Create("تيشيرت قطن أبيض")}  |  {Slug.CreateLatin("متجر أحمد")}  |  subdomain: {Slug.Subdomain("متجر أحمد")}");

Section("Amounts");
foreach (var amount in new[] { 1250m, 99.5m, 2_500_000m })
    Console.WriteLine($"{ArabicNumbers.FormatPounds(amount),-16} {ArabicNumbers.Pounds(amount)}");

static void Section(string title) => Console.WriteLine($"\n== {title} ==");
