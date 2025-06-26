using System.Drawing;
using FlexibleCalendar.Calendars;
using FlexibleCalendar.Models;
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

    private readonly List<WeekTemplate> _templates =
    [
        new WeekTemplate()
        {
            Colors = new()
            {
                { DayOfWeek.Monday, [Color.MediumVioletRed, Color.LightSkyBlue] },
                { DayOfWeek.Tuesday, [Color.DarkOrange, Color.MediumSeaGreen] },
                { DayOfWeek.Wednesday, [Color.Goldenrod, Color.MediumPurple] },
                { DayOfWeek.Thursday, [Color.IndianRed, Color.PaleGreen] },
                { DayOfWeek.Friday, [Color.RoyalBlue, Color.Khaki] },
                { DayOfWeek.Saturday, [Color.Sienna, Color.MistyRose] },
                { DayOfWeek.Sunday, [Color.Teal, Color.LightCoral] }
            },
            AcceptFromDate = new DateOnly(2025, 06, 1),
        },        
        new WeekTemplate()
        {
            Colors = new()
            {
                { DayOfWeek.Monday,    [Color.Red, Color.Yellow] },
                { DayOfWeek.Tuesday,   [Color.Blue, Color.Orange] },
                { DayOfWeek.Wednesday, [Color.Green, Color.Pink] },
                { DayOfWeek.Thursday,  [Color.Purple, Color.LightBlue] },
                { DayOfWeek.Friday,    [Color.Brown, Color.LightGreen] },
                { DayOfWeek.Saturday,  [Color.Gray, Color.Gold] },
                { DayOfWeek.Sunday,    [Color.Black, Color.White] }
            },
            AcceptFromDate = new DateOnly(2025, 06, 14),
        }
    ];

    private TemplatedCalendar _templatedCalendar = null!;

    private DateOnly CalendarFirstDate { get; set; }
    
    private DateOnly CalendarEndDate { get; set; }
    
    private int StartYear { get; set; }
    
    private int EndYear { get; set; }

    private void ReadData()
    {
        CalendarFirstDate = _templatedCalendar.CalendarFirstDate;
        CalendarEndDate = _templatedCalendar.CalendarEndDate;
        StartYear = _templatedCalendar.StartYear;
        EndYear = _templatedCalendar.EndYear;
    }
}