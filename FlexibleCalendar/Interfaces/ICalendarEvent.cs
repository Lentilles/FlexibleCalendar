using System.Drawing;

namespace FlexibleCalendar.Interfaces;

public interface ICalendarEvent
{
    DateOnly StartDate { get; }
    DateOnly EndDate { get; }
    IEnumerable<Color> BackgroundColors { get; }
    Color TextColor { get; }
}