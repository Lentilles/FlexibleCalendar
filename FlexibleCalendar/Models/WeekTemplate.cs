using FlexibleCalendar.Interfaces;
using System.Drawing;

namespace FlexibleCalendar.Models;

public class WeekTemplate : IWeekTemplate
{
    public DateOnly AcceptFromDate { get; set; }
    public Dictionary<DayOfWeek, IEnumerable<Color>> Colors { get; set; } = [];
    public Color TextColor { get; set; }
}