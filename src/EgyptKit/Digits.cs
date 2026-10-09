using System.Text;

namespace EgyptKit;

public static class Digits
{
    public static string ToLatin(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        var sb = new StringBuilder(text.Length);
        foreach (var c in text)
        {
            if (c is >= '٠' and <= '٩') sb.Append((char)('0' + (c - '٠')));
            else if (c is >= '۰' and <= '۹') sb.Append((char)('0' + (c - '۰')));
            else sb.Append(c);
        }
        return sb.ToString();
    }

    public static string ToArabic(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        var sb = new StringBuilder(text.Length);
        foreach (var c in text)
        {
            if (c is >= '0' and <= '9') sb.Append((char)('٠' + (c - '0')));
            else if (c == ',') sb.Append('٬');
            else if (c == '.') sb.Append('٫');
            else sb.Append(c);
        }
        return sb.ToString();
    }

    public static string Only(string? text)
    {
        var latin = ToLatin(text);
        var sb = new StringBuilder(latin.Length);
        foreach (var c in latin)
            if (c is >= '0' and <= '9') sb.Append(c);
        return sb.ToString();
    }
}
