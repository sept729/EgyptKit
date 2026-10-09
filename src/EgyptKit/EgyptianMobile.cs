using System.Diagnostics.CodeAnalysis;

namespace EgyptKit;

public enum MobileCarrier
{
    Vodafone,
    Etisalat,
    Orange,
    WE
}

public readonly record struct EgyptianMobile
{
    private EgyptianMobile(string national) => National = national;

    public string National { get; }

    public string E164 => "+2" + National;

    public string International => "2" + National;

    public MobileCarrier Carrier => National[2] switch
    {
        '0' => MobileCarrier.Vodafone,
        '1' => MobileCarrier.Etisalat,
        '2' => MobileCarrier.Orange,
        _ => MobileCarrier.WE
    };

    public static bool TryParse(string? input, [NotNullWhen(true)] out EgyptianMobile? mobile)
    {
        mobile = null;
        if (string.IsNullOrWhiteSpace(input)) return false;

        var latin = Digits.ToLatin(input).Trim();
        foreach (var c in latin)
            if (!(c is >= '0' and <= '9' || c is ' ' or '-' or '(' or ')' or '+' or '.'))
                return false;

        var digits = Digits.Only(latin);
        if (digits.StartsWith("0020", StringComparison.Ordinal)) digits = digits[4..];
        else if (digits.StartsWith("20", StringComparison.Ordinal) && digits.Length == 12) digits = digits[2..];
        if (digits.Length == 10 && digits[0] == '1') digits = "0" + digits;

        if (digits.Length != 11 || digits[0] != '0' || digits[1] != '1' || digits[2] is not ('0' or '1' or '2' or '5'))
            return false;

        mobile = new EgyptianMobile(digits);
        return true;
    }

    public static EgyptianMobile Parse(string input) =>
        TryParse(input, out var mobile) ? mobile.Value : throw new FormatException($"'{input}' isn't a valid Egyptian mobile number.");

    public static bool IsValid(string? input) => TryParse(input, out _);

    public static string? Normalize(string? input) => TryParse(input, out var mobile) ? mobile.Value.National : null;

    public string Format() => $"{National[..3]} {National[3..7]} {National[7..]}";

    public string WhatsAppLink(string? message = null) =>
        string.IsNullOrEmpty(message)
            ? $"https://wa.me/{International}"
            : $"https://wa.me/{International}?text={Uri.EscapeDataString(message)}";

    public string TelLink() => "tel:" + E164;

    public override string ToString() => National;
}
