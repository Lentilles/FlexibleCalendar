using System.Drawing;

namespace FlexibleCalendar.Interfaces;

public interface ICalendarEvent
{
    DateTime StartDate { get; }
    DateTime EndDate { get; }
    IEnumerable<Color> BackgroundColors { get; }
    Color TextColor { get; }
}