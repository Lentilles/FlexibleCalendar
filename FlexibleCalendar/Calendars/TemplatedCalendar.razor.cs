using FlexibleCalendar.Interfaces;
using FlexibleCalendar.Models;
using Microsoft.AspNetCore.Components;
using System.Drawing;

namespace FlexibleCalendar.Calendars;

public partial class TemplatedCalendar : CalendarBase
{
    [Parameter] public required IEnumerable<IWeekTemplate> Templates { get; set; } = [];

    [Parameter] public IEnumerable<ISpecialDate> SpecialDates { get; set; } = [];
    
    private string GetDayOfWeekStyle(DateOnly date)
    {
        if (ColorizeStyle != ColoredStyle.Filled)
        {
            return GetColorDateLikeBg(DateBackgroundCssVariable);
        }

        ISpecialDate? specialDate = SpecialDates.FirstOrDefault(x => x.Date == date);

        if (specialDate != null)
        {
            return GetMarkedDateColors(specialDate.Colors, ColorizeStyle);
        }
        
        IWeekTemplate? closestPastDate = Templates
            .Where(d => d.AcceptFromDate < date)
            .OrderByDescending(d => d)
            .FirstOrDefault();

        if (closestPastDate == null)
        {
            return GetColorDateLikeBg(DateBackgroundCssVariable);
        }

        IEnumerable<Color> colors = closestPastDate.Colors[date.DayOfWeek];
        
        return GetMarkedDateColors(colors, ColorizeStyle);
    }

    private string GetDayOfWeekPillStyle(DateOnly date)
    {
        if (ColorizeStyle != ColoredStyle.Pilled)
        {
            return GetColorDateLikeBg(DateBackgroundCssVariable);
        }
        
        ISpecialDate? specialDate = SpecialDates.FirstOrDefault(x => x.Date == date);

        if (specialDate != null)
        {
            return GetMarkedDateColors(specialDate.Colors, ColorizeStyle);;
        }
        
        IWeekTemplate? closestPastDate = Templates
            .Where(d => d.AcceptFromDate < date)
            .OrderByDescending(d => d.AcceptFromDate)
            .FirstOrDefault();

        if (closestPastDate == null)
        {
            return GetColorDateLikeBg(DateBackgroundCssVariable);
        }
        
        IEnumerable<Color> colors = closestPastDate.Colors[date.DayOfWeek];
        
        return GetMarkedDateColors(colors, ColorizeStyle);
    }

    private string GetTextColor(DateOnly date)
    {
        if (ColorizeStyle != ColoredStyle.Filled)
        {
            return string.Empty;
        }
        
        ISpecialDate? specialDate = SpecialDates.FirstOrDefault(x => x.Date == date);

        if (specialDate != null)
        {
            return GetDateTextColor(specialDate.TextColor);
        }
        
        IWeekTemplate? closestPastDate = Templates
            .Where(d => d.AcceptFromDate < date)
            .OrderByDescending(d => d)
            .FirstOrDefault();

        return closestPastDate == null ? string.Empty : GetDateTextColor(ColorTranslator.ToHtml(closestPastDate.TextColor));
    }
}
