using System.Drawing;

namespace FlexibleCalendar.Interfaces;

public interface ISpecialDate
{
    DateOnly Date { get; set; }
    IEnumerable<Color> Colors { get; set; }
    Color TextColor { get; set; }
}