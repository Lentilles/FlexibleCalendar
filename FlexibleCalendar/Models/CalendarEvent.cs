using FlexibleCalendar.Interfaces;
using System.Drawing;

namespace FlexibleCalendar.Models;

public class CalendarEvent(DateOnly start, DateOnly end, 
    IEnumerable<Color> color, Color textColor)
    : ICalendarEvent
{
    public DateOnly StartDate { get; set; } = start;
    
    public DateOnly EndDate { get; set; } = end;
    
    public IEnumerable<Color> BackgroundColors { get; set; } = color;
    
    public Color TextColor { get; set; } = textColor;
    
    
} 