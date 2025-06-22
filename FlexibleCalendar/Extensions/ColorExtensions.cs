using System.Drawing;
using System.Globalization;
using System.Text;

namespace FlexibleCalendar.Extensions;

internal static class ColorExtensions
{

    internal static string GetHtmlGradient(this IEnumerable<Color> colors)
    {
        List<Color> colorList = colors.ToList();
        int count = colorList.Count;

        switch (count)
        {
            case 0:
                return string.Empty;
            case 1:
                return ColorTranslator.ToHtml(colorList[0]);
        }

        StringBuilder sb = new();

        for (int i = 0; i < count; i++)
        {
            Color color = colorList[i];
            double startPercent = 100.0 * i / count;
            double endPercent = 100.0 * (i + 1) / count;
            sb.Append($"{ColorTranslator.ToHtml(color)} {startPercent.ToString("0.00", CultureInfo.InvariantCulture)}% {endPercent.ToString("0.00", CultureInfo.InvariantCulture)}%");
            if (i < count - 1)
                sb.Append(", ");
        }
        return sb.ToString();
    }
}
