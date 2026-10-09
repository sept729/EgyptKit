using System.Text;

namespace EgyptKit;

public static class Slug
{
    private static readonly Dictionary<char, string> Latin = new()
    {
        ['ا'] = "a", ['ب'] = "b", ['ت'] = "t", ['ث'] = "th", ['ج'] = "g", ['ح'] = "h", ['خ'] = "kh",
        ['د'] = "d", ['ذ'] = "z", ['ر'] = "r", ['ز'] = "z", ['س'] = "s", ['ش'] = "sh", ['ص'] = "s",
        ['ض'] = "d", ['ط'] = "t", ['ظ'] = "z", ['ع'] = "a", ['غ'] = "gh", ['ف'] = "f", ['ق'] = "k",
        ['ك'] = "k", ['ل'] = "l", ['م'] = "m", ['ن'] = "n", ['ه'] = "h", ['و'] = "w", ['ي'] = "y",
        ['ء'] = ""
    };

    public static string Create(string? text, int maxLength = 60) => Build(text, maxLength, transliterate: false);

    public static string CreateLatin(string? text, int maxLength = 60) => Build(text, maxLength, transliterate: true);

    public static string Subdomain(string? text, int maxLength = 30)
    {
        var slug = CreateLatin(text, maxLength);
        return slug.Length >= 3 ? slug : string.Empty;
    }

    private static string Build(string? text, int maxLength, bool transliterate)
    {
        if (maxLength < 1) throw new ArgumentOutOfRangeException(nameof(maxLength));
        var normalized = ArabicText.Normalize(text);
        var sb = new StringBuilder(normalized.Length);
        var dash = false;

        foreach (var c in normalized)
        {
            string? piece = null;
            if (c is >= 'a' and <= 'z' or >= '0' and <= '9') piece = c.ToString();
            else if (c is >= '\u0600' and <= '\u06FF' && char.IsLetter(c))
                piece = transliterate ? (Latin.TryGetValue(c, out var l) ? l : string.Empty) : c.ToString();
            else if (char.IsLetter(c) && !transliterate) piece = c.ToString();

            if (string.IsNullOrEmpty(piece))
            {
                if (piece is null && sb.Length > 0) dash = true;
                continue;
            }

            if (dash) sb.Append('-');
            dash = false;
            sb.Append(piece);
        }

        var result = sb.ToString();
        if (result.Length <= maxLength) return result;
        var cut = result[..maxLength];
        var lastDash = cut.LastIndexOf('-');
        return (lastDash > maxLength / 2 ? cut[..lastDash] : cut).Trim('-');
    }
}
