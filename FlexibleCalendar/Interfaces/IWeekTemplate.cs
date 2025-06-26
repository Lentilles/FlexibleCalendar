using System.Drawing;

namespace FlexibleCalendar.Interfaces;

public interface IWeekTemplate
{
    DateOnly AcceptFromDate { get; set; }
    Dictionary<DayOfWeek, IEnumerable<Color>> Colors { get; set; }
    Color TextColor { get; set; }
}