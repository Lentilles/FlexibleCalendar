using FlexibleCalendar.Interfaces;
using FlexibleCalendar.Models;
using Microsoft.AspNetCore.Components;

namespace FlexibleCalendar.Calendars;

public partial class Calendar : CalendarBase
{

    /// <summary>
    /// The collection of calendar events to display. Each event can specify a date range and colors.
    /// </summary>
    [Parameter]
    public IEnumerable<ICalendarEvent>? Events { get; set; } = [];

    private ICalendarEvent? GetEventForDate(DateOnly date)
    {
        return Events?.FirstOrDefault(ev => ev.StartDate <= date && ev.EndDate >= date);
    }

    private string GetBackgroundColor(DateOnly date)
    {
        if (ColorizeStyle != ColoredStyle.Filled)
        {
            return GetColorDateLikeBg(DateBackgroundCssVariable);
        }

        ICalendarEvent? ev = GetEventForDate(date);

        if (ev != null)
        {
            return GetMarkedDateColors(ev.BackgroundColors, ColorizeStyle);
        }

        return GetColorDateLikeBg(DateBackgroundCssVariable);
    }

    private string GetPillColor(DateOnly date)
    {
        if (ColorizeStyle != ColoredStyle.Pilled)
        {
            return GetColorDateLikeBg(DateBackgroundCssVariable);
        }

        ICalendarEvent? ev = GetEventForDate(date);

        if (ev != null)
        {
            return GetMarkedDateColors(ev.BackgroundColors, ColorizeStyle);
        }
        
        return GetColorDateLikeBg(DateBackgroundCssVariable);
    }
    
    private string? GetTextColor(DateOnly date)
    {
        ICalendarEvent? ev = GetEventForDate(date);
        return ev != null ? GetDateTextColor(ev.TextColor) : null;
    }
}