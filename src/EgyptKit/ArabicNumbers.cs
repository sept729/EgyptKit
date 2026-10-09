using System.Globalization;
using System.Text;

namespace EgyptKit;

public static class ArabicNumbers
{
    private static readonly string[] Ones =
    {
        "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة",
        "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر"
    };

    private static readonly string[] Tens =
        { "", "", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };

    private static readonly string[] Hundreds =
        { "", "مائة", "مائتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };

    private static readonly (long Value, string One, string Two, string Plural)[] Scales =
    {
        (1_000_000_000_000, "تريليون", "تريليونان", "تريليونات"),
        (1_000_000_000, "مليار", "ملياران", "مليارات"),
        (1_000_000, "مليون", "مليونان", "ملايين"),
        (1_000, "ألف", "ألفان", "آلاف")
    };

    public static string ToWords(long number)
    {
        if (number == 0) return "صفر";
        if (number < 0) return "سالب " + ToWords(-number);

        var parts = new List<string>();
        var rest = number;
        foreach (var (value, one, two, plural) in Scales)
        {
            var count = rest / value;
            rest %= value;
            if (count == 0) continue;
            parts.Add(count switch
            {
                1 => one,
                2 => two,
                <= 10 => $"{BelowThousand((int)count)} {plural}",
                _ => $"{ToWords(count)} {one}"
            });
        }

        if (rest > 0) parts.Add(BelowThousand((int)rest));
        return string.Join(" و", parts);
    }

    public static string Pounds(decimal amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount can't be negative.");
        var rounded = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        var pounds = (long)Math.Truncate(rounded);
        var piasters = (int)((rounded - pounds) * 100);

        var sb = new StringBuilder("فقط ");
        if (pounds > 0 || piasters == 0) sb.Append(ToWords(pounds)).Append(" جنيه مصري");
        if (piasters > 0)
        {
            if (pounds > 0) sb.Append(" و");
            sb.Append(ToWords(piasters)).Append(" قرش");
        }
        return sb.Append(" لا غير").ToString();
    }

    public static string FormatPounds(decimal amount, bool arabicDigits = false)
    {
        var rounded = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        var text = rounded == Math.Truncate(rounded)
            ? rounded.ToString("#,0", CultureInfo.InvariantCulture)
            : rounded.ToString("#,0.00", CultureInfo.InvariantCulture);
        if (arabicDigits) text = Digits.ToArabic(text);
        return text + " ج.م";
    }

    private static string BelowThousand(int n)
    {
        var hundreds = n / 100;
        var rest = n % 100;
        var parts = new List<string>(2);
        if (hundreds > 0) parts.Add(Hundreds[hundreds]);
        if (rest > 0)
        {
            if (rest < 20) parts.Add(Ones[rest]);
            else if (rest % 10 == 0) parts.Add(Tens[rest / 10]);
            else parts.Add($"{Ones[rest % 10]} و{Tens[rest / 10]}");
        }
        return string.Join(" و", parts);
    }
}
