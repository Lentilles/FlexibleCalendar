using System.Drawing;
using FlexibleCalendar.Calendar;
using Microsoft.AspNetCore.Components;

namespace FlexibleCalendar.Example.Components.Pages;

public partial class Home : ComponentBase
{
    private List<CalendarEvent> _events =
        [new(DateTime.Now, DateTime.Now.AddDays(7), Color.DarkSeaGreen, Color.Azure),
            new(DateTime.Now.AddDays(9), DateTime.Now.AddDays(15), Color.DarkOrange, Color.Black)];
}