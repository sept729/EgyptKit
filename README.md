# EgyptKit

[![CI](https://github.com/sept729/EgyptKit/actions/workflows/ci.yml/badge.svg)](https://github.com/sept729/EgyptKit/actions/workflows/ci.yml)

The small Egypt-specific helpers every .NET project here ends up writing again: mobile numbers, national IDs, governorates and shipping zones, Arabic search, slugs, and amounts written out in Arabic for invoices.

No dependencies beyond the .NET base library. Everything accepts Arabic-Indic digits (`٠١٢٣`) as well as Latin ones, because that's what customers actually type.

I built these for [SEPT](https://sept.cloud), a storefront platform for Egyptian sellers, where checkout forms, order lookups and store search all run into the same problems.

## Mobile numbers

```csharp
EgyptianMobile.TryParse("+20 100 123 4567", out var mobile);   // also "0100-123-4567", "٠١٠٠١٢٣٤٥٦٧", "1001234567"

mobile.Value.National;          // "01001234567"
mobile.Value.E164;              // "+201001234567"
mobile.Value.Carrier;           // MobileCarrier.Vodafone
mobile.Value.Format();          // "010 0123 4567"
mobile.Value.WhatsAppLink("Hi, about order #1005");   // https://wa.me/201001234567?text=...

EgyptianMobile.Normalize(input); // "01001234567", or null if it isn't a valid mobile
```

Normalising on the way in means "010 1234 5678" and "+201012345678" end up as the same customer, so order lookups by phone keep working.

The carrier comes from the prefix (010 Vodafone, 011 Etisalat, 012 Orange, 015 WE). Numbers that were ported to another network keep their original prefix, so treat it as a hint.

## National IDs

```csharp
if (EgyptianNationalId.TryParse("30105151501234", out var id))
{
    id.BirthDate;       // 2001-05-15
    id.Gender;          // Gender.Male
    id.Governorate;     // Kafr El Sheikh / كفر الشيخ
    id.AgeOn(today);    // 25
    id.Masked();        // "301********234"
}
```

It checks the structure: length, century digit, a real birth date that isn't in the future, and a known governorate code (or 88 for people born abroad). The final check digit isn't verified, so a passing ID is well-formed, not confirmed to exist.

## Governorates and shipping zones

```csharp
Governorates.All;                     // all 27, with English and Arabic names
Governorates.Find("القاهره");          // Cairo
Governorates.Find("Kafr El-Sheikh");  // Kafr El Sheikh
Governorates.Find("محافظة الجيزة");    // Giza
Governorates.FromCode(29);            // Luxor (the code used in national IDs)

Governorates.Find("مطروح")!.Zone;     // ShippingZone.Remote
```

`Find` ignores case, hamza and taa marbuta differences, the "ال" prefix, the word "governorate", and common English spellings, so it can read whatever a customer typed into a free-text field.

Each governorate belongs to one of five zones that most Egyptian couriers price by: Greater Cairo, Alexandria, Delta and Canal, Upper Egypt, and Remote (Red Sea, New Valley, Matrouh, North and South Sinai). Use them to set shipping fees per zone instead of per governorate.

## Arabic search

```csharp
ArabicText.Normalize("مُحَمَّد أحمد");               // "محمد احمد"
ArabicText.Matches("تيشيرت قطن أبيض", "ابيض");      // true
ArabicText.Matches("فستان سهرة", "سهره");           // true
```

`Normalize` removes diacritics and tatweel, unifies alef forms, taa marbuta, alef maqsura and hamza carriers, converts digits and collapses spaces. Normalise both the stored text and the query and simple `Contains` searches stop missing results because of spelling. Store a normalised copy in a column if you search in the database.

## Slugs

```csharp
Slug.Create("تيشيرت قطن أبيض");       // "تيشيرت-قطن-ابيض"  (Arabic is valid in URLs)
Slug.CreateLatin("متجر أحمد");        // "mtgr-ahmd"
Slug.Subdomain("متجر أحمد");          // "mtgr-ahmd", or "" if too short
```

## Amounts in Arabic words

```csharp
ArabicNumbers.Pounds(1250m);        // "فقط ألف ومائتان وخمسون جنيه مصري لا غير"
ArabicNumbers.Pounds(99.5m);        // "فقط تسعة وتسعون جنيه مصري وخمسون قرش لا غير"
ArabicNumbers.ToWords(2_500_000);   // "مليونان وخمسمائة ألف"
ArabicNumbers.FormatPounds(1250.5m);        // "1,250.50 ج.م"
ArabicNumbers.FormatPounds(1250.5m, true);  // "١٬٢٥٠٫٥٠ ج.م"
```

This is the "تفقيط" line Egyptian invoices and receipts print under the total. It follows the common invoice style rather than full classical grammar.

## Validation attributes

```csharp
public sealed class CheckoutRequest
{
    [Required, EgyptianMobile] public string Phone { get; set; } = "";
    [EgyptianGovernorate] public string? Governorate { get; set; }
    [EgyptianNationalId] public string? NationalId { get; set; }
}
```

Empty values pass, so combine with `[Required]` when the field is mandatory.

## Sample

```bash
dotnet run --project samples/EgyptKit.Sample
```

## Tests

```bash
dotnet test
```

## License

MIT
