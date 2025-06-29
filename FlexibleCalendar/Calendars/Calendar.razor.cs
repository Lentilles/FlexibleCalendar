using FlexibleCalendar.Extensions;
using FlexibleCalendar.Interfaces;
using FlexibleCalendar.Models;
using Microsoft.AspNetCore.Components;
using System.Drawing;
using System.Globalization;

namespace FlexibleCalendar.Calendars;

public partial class Calendar : ComponentBase
{
    /// <summary>
    /// The number of months to display at once. Allowed values: from 1 to 12.
    /// </summary>
    [Parameter]
    public int MonthsToShow { get; set; }

    /// <summary>
    /// The number of columns to use when rendering the calendar months grid.
    /// </summary>
    [Parameter]
    public int CalendarColumns { get; set; } = 1;

    /// <summary>
    /// The start date for the calendar. If not specified, January 1st of the current year is used by default.
    /// </summary>
    [Parameter]
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The culture used for month and weekday names. If not specified, en-US is used by default.
    /// </summary>
    [Parameter]
    public CultureInfo? Culture { get; set; } = new("en-US");

    /// <summary>
    /// The collection of calendar events to display. Each event can specify a date range and colors.
    /// </summary>
    [Parameter]
    public IEnumerable<ICalendarEvent>? Events { get; set; } = [];

    /// <summary>
    /// Event triggered when a day is clicked. Returns the clicked date.
    /// </summary>
    [Parameter]
    public EventCallback<DateOnly> OnDayClick { get; set; }

    /// <summary>
    /// The CSS variable name (e.g. --my-bg) to use as the background for date numbers. If set, it will be used as a background for all date cells.
    /// </summary>
    [Parameter]
    public string? DateBackgroundCssVariable { get; set; }
    
    [Parameter]
    public ColoredStyle ColorizeStyle { get; set; } = ColoredStyle.Filled;
    
    
    /// <summary>
    /// First Date of displayed calendar
    /// </summary>
    public DateOnly CalendarFirstDate => MonthsDays.First().FirstOrDefault(x => x.HasValue)!.Value;
    
    public DateOnly CalendarEndDate => MonthsDays.Last().LastOrDefault(x => x.HasValue)!.Value;
    
    public int StartYear => DisplayedMonths.First().Year;
    
    public int EndYear => DisplayedMonths.Last().Year;
    
    private DateOnly _displayedMonth;

    protected override void OnInitialized()
    {
        _displayedMonth = StartDate ?? new DateOnly(DateTime.Today.Year, 1, 1);
        CultureInfo culture = Culture ?? CultureInfo.CurrentCulture;
        _weekDays = culture.DateTimeFormat.AbbreviatedDayNames;
        if (culture.DateTimeFormat.FirstDayOfWeek == DayOfWeek.Monday)
        {
            _weekDays = _weekDays.Skip(1).Concat(_weekDays.Take(1)).ToArray();
        }
        GenerateDays();
    }

    protected override void OnParametersSet()
    {
        MonthsToShow = Math.Max(1, Math.Min(12, MonthsToShow));
    }
    
    private string[] _weekDays = [];
    
    private List<List<DateOnly?>> MonthsDays { get; set; } = [];
    
    private List<(int Year, string MonthName)> DisplayedMonths { get; set; } = [];
    
    private void ShowPreviousMonth()
    {
        _displayedMonth = _displayedMonth.AddMonths(-MonthsToShow);
        GenerateDays();
    }

    private void ShowNextMonth()
    {
        _displayedMonth = _displayedMonth.AddMonths(MonthsToShow);
        GenerateDays();
    }

    private void GenerateDays()
    {
        MonthsDays.Clear();
        DisplayedMonths.Clear();
        CultureInfo culture = Culture ?? CultureInfo.CurrentCulture;
        for (int m = 0; m < MonthsToShow; m++)
        {
            DateOnly month = _displayedMonth.AddMonths(m);
            string monthName = culture.DateTimeFormat.GetMonthName(month.Month);
            DisplayedMonths.Add((month.Year, monthName));
            List<DateOnly?> days = new();
            DateTime firstDayOfMonth = new(month.Year, month.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            int startDayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            for (int i = 0; i < startDayOfWeek; i++)
                days.Add(null);
            for (int day = 1; day <= daysInMonth; day++)
                days.Add(new DateOnly(month.Year, month.Month, day));
            MonthsDays.Add(days);
        }
        StateHasChanged();
    }

    private ICalendarEvent? GetEventForDate(DateOnly date)
    {
        return Events?.FirstOrDefault(ev => ev.StartDate <= date && ev.EndDate >= date);
    }

    private string GetBackgroundColor(DateOnly date)
    {
        if (ColorizeStyle != ColoredStyle.Filled)
        {
            return !string.IsNullOrWhiteSpace(DateBackgroundCssVariable) ? $"background: var({DateBackgroundCssVariable});" : string.Empty;
        }

        ICalendarEvent? ev = GetEventForDate(date);

        if (ev != null)
        {
            return $"background: repeating-linear-gradient(45deg, {ev.BackgroundColors.GetHtmlGradient()});";
        }
        
        return !string.IsNullOrWhiteSpace(DateBackgroundCssVariable) ? $"background: var({DateBackgroundCssVariable});" : string.Empty;
    }

    private string GetPillColor(DateOnly date)
    {
        if (ColorizeStyle != ColoredStyle.Pilled)
        {
            return string.Empty;
        }

        ICalendarEvent? ev = GetEventForDate(date);

        if (ev != null)
        {
            return $"background: repeating-linear-gradient(to right, {ev.BackgroundColors.GetHtmlGradient()});";
        }
        
        return !string.IsNullOrWhiteSpace(DateBackgroundCssVariable) ? $"background: var({DateBackgroundCssVariable});" : string.Empty;
    }
    
    private string? GetTextColor(DateOnly date)
    {
        ICalendarEvent? ev = GetEventForDate(date);
        return ev != null ? $"color: {ColorTranslator.ToHtml(ev.TextColor)};" : null;
    }
}