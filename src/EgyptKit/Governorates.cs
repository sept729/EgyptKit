using System.Text;

namespace EgyptKit;

public enum ShippingZone
{
    GreaterCairo,
    Alexandria,
    DeltaAndCanal,
    UpperEgypt,
    Remote
}

public sealed record Governorate(int Code, string NameEn, string NameAr, ShippingZone Zone)
{
    public string Name(bool arabic) => arabic ? NameAr : NameEn;

    public override string ToString() => NameEn;
}

public static class Governorates
{
    public static readonly Governorate Cairo = new(1, "Cairo", "القاهرة", ShippingZone.GreaterCairo);
    public static readonly Governorate Alexandria = new(2, "Alexandria", "الإسكندرية", ShippingZone.Alexandria);
    public static readonly Governorate PortSaid = new(3, "Port Said", "بورسعيد", ShippingZone.DeltaAndCanal);
    public static readonly Governorate Suez = new(4, "Suez", "السويس", ShippingZone.DeltaAndCanal);
    public static readonly Governorate Damietta = new(11, "Damietta", "دمياط", ShippingZone.DeltaAndCanal);
    public static readonly Governorate Dakahlia = new(12, "Dakahlia", "الدقهلية", ShippingZone.DeltaAndCanal);
    public static readonly Governorate Sharqia = new(13, "Sharqia", "الشرقية", ShippingZone.DeltaAndCanal);
    public static readonly Governorate Qalyubia = new(14, "Qalyubia", "القليوبية", ShippingZone.GreaterCairo);
    public static readonly Governorate KafrElSheikh = new(15, "Kafr El Sheikh", "كفر الشيخ", ShippingZone.DeltaAndCanal);
    public static readonly Governorate Gharbia = new(16, "Gharbia", "الغربية", ShippingZone.DeltaAndCanal);
    public static readonly Governorate Monufia = new(17, "Monufia", "المنوفية", ShippingZone.DeltaAndCanal);
    public static readonly Governorate Beheira = new(18, "Beheira", "البحيرة", ShippingZone.DeltaAndCanal);
    public static readonly Governorate Ismailia = new(19, "Ismailia", "الإسماعيلية", ShippingZone.DeltaAndCanal);
    public static readonly Governorate Giza = new(21, "Giza", "الجيزة", ShippingZone.GreaterCairo);
    public static readonly Governorate BeniSuef = new(22, "Beni Suef", "بني سويف", ShippingZone.UpperEgypt);
    public static readonly Governorate Fayoum = new(23, "Fayoum", "الفيوم", ShippingZone.UpperEgypt);
    public static readonly Governorate Minya = new(24, "Minya", "المنيا", ShippingZone.UpperEgypt);
    public static readonly Governorate Assiut = new(25, "Assiut", "أسيوط", ShippingZone.UpperEgypt);
    public static readonly Governorate Sohag = new(26, "Sohag", "سوهاج", ShippingZone.UpperEgypt);
    public static readonly Governorate Qena = new(27, "Qena", "قنا", ShippingZone.UpperEgypt);
    public static readonly Governorate Aswan = new(28, "Aswan", "أسوان", ShippingZone.UpperEgypt);
    public static readonly Governorate Luxor = new(29, "Luxor", "الأقصر", ShippingZone.UpperEgypt);
    public static readonly Governorate RedSea = new(31, "Red Sea", "البحر الأحمر", ShippingZone.Remote);
    public static readonly Governorate NewValley = new(32, "New Valley", "الوادي الجديد", ShippingZone.Remote);
    public static readonly Governorate Matrouh = new(33, "Matrouh", "مطروح", ShippingZone.Remote);
    public static readonly Governorate NorthSinai = new(34, "North Sinai", "شمال سيناء", ShippingZone.Remote);
    public static readonly Governorate SouthSinai = new(35, "South Sinai", "جنوب سيناء", ShippingZone.Remote);

    public static IReadOnlyList<Governorate> All { get; } = new[]
    {
        Cairo, Alexandria, PortSaid, Suez, Damietta, Dakahlia, Sharqia, Qalyubia, KafrElSheikh, Gharbia,
        Monufia, Beheira, Ismailia, Giza, BeniSuef, Fayoum, Minya, Assiut, Sohag, Qena, Aswan, Luxor,
        RedSea, NewValley, Matrouh, NorthSinai, SouthSinai
    };

    private static readonly Dictionary<int, Governorate> ByCode = All.ToDictionary(g => g.Code);

    private static readonly Dictionary<string, Governorate> ByName = BuildNames();

    public static Governorate? FromCode(int code) => ByCode.TryGetValue(code, out var g) ? g : null;

    public static Governorate? Find(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        return ByName.TryGetValue(Key(name), out var g) ? g : null;
    }

    public static IEnumerable<Governorate> InZone(ShippingZone zone) => All.Where(g => g.Zone == zone);

    private static Dictionary<string, Governorate> BuildNames()
    {
        var map = new Dictionary<string, Governorate>(StringComparer.Ordinal);
        void Add(Governorate g, params string[] names)
        {
            foreach (var n in names) map[Key(n)] = g;
        }

        foreach (var g in All) Add(g, g.NameEn, g.NameAr);

        Add(Alexandria, "Alex", "اسكندرية", "إسكندرية");
        Add(PortSaid, "Port-Said", "Portsaid", "بور سعيد");
        Add(Sharqia, "Sharkia", "Sharkeya", "Al Sharqia");
        Add(Qalyubia, "Qalyoubia", "Kalyubia", "Qaliubiya");
        Add(KafrElSheikh, "Kafr Elsheikh", "Kafr Al Sheikh", "Kafrelsheikh");
        Add(Gharbia, "Gharbeya", "Al Gharbia");
        Add(Monufia, "Menoufia", "Menofia", "Minufiya");
        Add(Beheira, "Behira", "Buhayrah", "Al Beheira");
        Add(Dakahlia, "Dakahleya", "Daqahliya");
        Add(Fayoum, "Fayum", "Faiyum");
        Add(Assiut, "Asyut", "Asyout", "اسيوط");
        Add(Sohag, "Suhag", "Souhag");
        Add(Aswan, "اسوان");
        Add(Luxor, "الاقصر");
        Add(Matrouh, "Marsa Matrouh", "مرسى مطروح", "Matruh");
        Add(Damietta, "Domyat", "Dumyat");
        Add(Ismailia, "Ismailiya", "الاسماعيلية");
        Add(RedSea, "البحر الاحمر");
        Add(Cairo, "Al Qahira", "Qahira");
        Add(Giza, "Gizeh", "Al Giza");
        return map;
    }

    private static string Key(string name)
    {
        var n = ArabicText.Normalize(name);
        var parts = n.Split(new[] { ' ', '-', '_', '.' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.StartsWith("ال", StringComparison.Ordinal) && p.Length > 3 ? p[2..] : p)
            .Where(p => p is not ("al" or "el" or "governorate" or "محافظه"));
        var sb = new StringBuilder();
        foreach (var p in parts) sb.Append(p);
        return sb.ToString();
    }
}
