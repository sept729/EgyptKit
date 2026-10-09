using System.Diagnostics.CodeAnalysis;

namespace EgyptKit;

public enum Gender
{
    Male,
    Female
}

public sealed record EgyptianNationalId
{
    public const int BornAbroadCode = 88;

    private EgyptianNationalId(string value, DateOnly birthDate, int governorateCode, Gender gender)
    {
        Value = value;
        BirthDate = birthDate;
        GovernorateCode = governorateCode;
        Gender = gender;
    }

    public string Value { get; }

    public DateOnly BirthDate { get; }

    public int GovernorateCode { get; }

    public Governorate? Governorate => Governorates.FromCode(GovernorateCode);

    public bool BornAbroad => GovernorateCode == BornAbroadCode;

    public Gender Gender { get; }

    public int AgeOn(DateOnly date)
    {
        var age = date.Year - BirthDate.Year;
        if (date < BirthDate.AddYears(age)) age--;
        return age;
    }

    public static bool TryParse(string? input, [NotNullWhen(true)] out EgyptianNationalId? id) =>
        TryParse(input, DateOnly.FromDateTime(DateTime.UtcNow), out id);

    public static bool TryParse(string? input, DateOnly today, [NotNullWhen(true)] out EgyptianNationalId? id)
    {
        id = null;
        if (string.IsNullOrWhiteSpace(input)) return false;

        var latin = Digits.ToLatin(input).Replace(" ", string.Empty).Replace("-", string.Empty);
        if (latin.Length != 14 || !latin.All(c => c is >= '0' and <= '9')) return false;

        var century = latin[0] switch { '2' => 1900, '3' => 2000, _ => 0 };
        if (century == 0) return false;

        var year = century + int.Parse(latin.AsSpan(1, 2));
        var month = int.Parse(latin.AsSpan(3, 2));
        var day = int.Parse(latin.AsSpan(5, 2));
        if (month is < 1 or > 12 || day < 1 || day > DateTime.DaysInMonth(year, month)) return false;

        var birth = new DateOnly(year, month, day);
        if (birth > today) return false;

        var gov = int.Parse(latin.AsSpan(7, 2));
        if (gov != BornAbroadCode && Governorates.FromCode(gov) is null) return false;

        var gender = (latin[12] - '0') % 2 == 1 ? Gender.Male : Gender.Female;
        id = new EgyptianNationalId(latin, birth, gov, gender);
        return true;
    }

    public static EgyptianNationalId Parse(string input) =>
        TryParse(input, out var id) ? id : throw new FormatException("That isn't a valid Egyptian national ID.");

    public static bool IsValid(string? input) => TryParse(input, out _);

    public string Masked() => Value[..3] + new string('*', 8) + Value[^3..];

    public override string ToString() => Value;
}
