using System.Drawing;

namespace FlexibleCalendar.Calendar;

public interface ICalendarEvent
{
    DateTime StartDate { get; }
    DateTime EndDate { get; }
    IEnumerable<Color> BackgroundColors { get; }
    Color TextColor { get; }
}

public class CalendarEvent(DateTime start, DateTime end, 
    IEnumerable<Color> color, Color textColor)
    : ICalendarEvent
{
    public DateTime StartDate { get; set; } = start;
    
    public DateTime EndDate { get; set; } = end;
    
    public IEnumerable<Color> BackgroundColors { get; set; } = color;
    
    public Color TextColor { get; set; } = textColor;
    
    
} 