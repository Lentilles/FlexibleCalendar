using System.Drawing;
using FlexibleCalendar.Calendar;
using Microsoft.AspNetCore.Components;

namespace FlexibleCalendar.Example.Components.Pages;

public partial class Home : ComponentBase
{
    private readonly List<CalendarEvent> _events =
        [
            new(DateTime.Now, DateTime.Now.AddDays(7), [Color.DarkSeaGreen, Color.DarkCyan], Color.Azure),
            new(DateTime.Now.AddDays(7), DateTime.Now.AddDays(9), [Color.DarkSeaGreen], Color.Azure),
            new(DateTime.Now.AddDays(10), DateTime.Now.AddDays(12), [Color.DarkRed, Color.DarkOrange], Color.White),
            new(DateTime.Now.AddDays(13), DateTime.Now.AddDays(16), [Color.DarkRed, Color.DarkOrange, Color.DarkCyan, Color.DarkMagenta], Color.White),
        ];
}