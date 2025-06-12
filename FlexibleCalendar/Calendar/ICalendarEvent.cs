using System.Drawing;

namespace FlexibleCalendar.Calendar;

public interface ICalendarEvent
{
    DateTime StartDate { get; set; }
    DateTime EndDate { get; set; }
    Color Color { get; set; }
    Color TextColor { get; set; }
}

public class CalendarEvent : ICalendarEvent
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Color Color { get; set; }
    public Color TextColor { get; set; }

    public CalendarEvent(DateTime start, DateTime end, Color color, Color textColor)
    {
        StartDate = start;
        EndDate = end;
        Color = color;
        TextColor = textColor;
    }
} 