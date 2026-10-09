using System.Globalization;
using System.Text;

namespace EgyptKit;

public static class ArabicText
{
    public static string Normalize(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        var sb = new StringBuilder(text.Length);
        var lastWasSpace = true;
        foreach (var raw in text)
        {
            var c = raw;
            if (c is >= 'ً' and <= 'ٟ' || c is 'ٰ' or 'ـ') continue;

            c = c switch
            {
                'أ' or 'إ' or 'آ' or 'ٱ' => 'ا',
                'ة' => 'ه',
                'ى' => 'ي',
                'ؤ' => 'و',
                'ئ' => 'ي',
                >= '٠' and <= '٩' => (char)('0' + (c - '٠')),
                >= '۰' and <= '۹' => (char)('0' + (c - '۰')),
                _ => char.ToLowerInvariant(c)
            };

            if (char.IsWhiteSpace(c))
            {
                if (!lastWasSpace) sb.Append(' ');
                lastWasSpace = true;
                continue;
            }

            sb.Append(c);
            lastWasSpace = false;
        }

        return sb.ToString().TrimEnd();
    }

    public static bool ContainsArabic(string? text) =>
        !string.IsNullOrEmpty(text) && text.Any(c => c is >= '؀' and <= 'ۿ');

    public static bool Matches(string? text, string? query)
    {
        var q = Normalize(query);
        if (q.Length == 0) return true;
        var t = Normalize(text);
        return q.Split(' ').All(word => t.Contains(word, StringComparison.Ordinal));
    }

    public static string RemoveDiacritics(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        var sb = new StringBuilder(text.Length);
        foreach (var c in text.Normalize(NormalizationForm.FormD))
        {
            if (c is >= 'ً' and <= 'ٟ' || c is 'ٰ' or 'ـ') continue;
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark && c < '؀') continue;
            sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
