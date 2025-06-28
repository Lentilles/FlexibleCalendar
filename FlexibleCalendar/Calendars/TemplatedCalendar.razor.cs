using FlexibleCalendar.Extensions;
using FlexibleCalendar.Interfaces;
using FlexibleCalendar.Models;
using Microsoft.AspNetCore.Components;
using System.Drawing;
using System.Globalization;

namespace FlexibleCalendar.Calendars;

public partial class TemplatedCalendar : ComponentBase
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
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// The culture used for month and weekday names. If not specified, en-US is used by default.
    /// </summary>
    [Parameter]
    public CultureInfo? Culture { get; set; } = new("en-US");

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

    [Parameter] public required IEnumerable<IWeekTemplate> Templates { get; set; } = [];

    [Parameter] public IEnumerable<ISpecialDate> SpecialDates { get; set; } = [];

    public DateOnly CalendarFirstDate => MonthsDays.First().FirstOrDefault(x => x.HasValue)!.Value;
    
    public DateOnly CalendarEndDate => MonthsDays.Last().LastOrDefault(x => x.HasValue)!.Value;
    
    public int StartYear => DisplayedMonths.First().Year;
    
    public int EndYear => DisplayedMonths.Last().Year;

    private DateTime _displayedMonth;

    protected override void OnInitialized()
    {
        _displayedMonth = StartDate?.Date ?? new DateTime(DateTime.Today.Year, 1, 1);
        CultureInfo culture = Culture ?? CultureInfo.CurrentCulture;
        _weekDays = culture.DateTimeFormat.AbbreviatedDayNames;
        // Сдвиг, чтобы неделя начиналась с понедельника
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
            DateTime month = _displayedMonth.AddMonths(m);
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

    private string GetDayOfWeekStyle(DateOnly date)
    {
        if (ColorizeStyle != ColoredStyle.Filled)
        {
            return !string.IsNullOrWhiteSpace(DateBackgroundCssVariable) ? $"background: var({DateBackgroundCssVariable});" : string.Empty;
        }

        ISpecialDate? specialDate = SpecialDates.FirstOrDefault(x => x.Date == date);

        if (specialDate != null)
        {
            return $"background: repeating-linear-gradient(45deg, {specialDate.Colors.GetHtmlGradient()};";
        }
        
        IWeekTemplate? closestPastDate = Templates
            .Where(d => d.AcceptFromDate < date)
            .OrderByDescending(d => d)
            .FirstOrDefault();

        if (closestPastDate == null)
        {
            return !string.IsNullOrWhiteSpace(DateBackgroundCssVariable) ? $"background: var({DateBackgroundCssVariable});" : string.Empty;
        }

        IEnumerable<Color> colors = closestPastDate.Colors[date.DayOfWeek];
        
        return $"background: repeating-linear-gradient(45deg, {colors.GetHtmlGradient()});";
    }

    private string GetDayOfWeekPillStyle(DateOnly date)
    {
        if (ColorizeStyle != ColoredStyle.Pilled)
        {
            return !string.IsNullOrWhiteSpace(DateBackgroundCssVariable) ? $"background: var({DateBackgroundCssVariable});" : string.Empty;
        }
        
        ISpecialDate? specialDate = SpecialDates.FirstOrDefault(x => x.Date == date);

        if (specialDate != null)
        {
            return $"background: repeating-linear-gradient(to right, {specialDate.Colors.GetHtmlGradient()});";
        }
        
        IWeekTemplate? closestPastDate = Templates
            .Where(d => d.AcceptFromDate < date)
            .OrderByDescending(d => d.AcceptFromDate)
            .FirstOrDefault();

        if (closestPastDate == null)
        {
            return !string.IsNullOrWhiteSpace(DateBackgroundCssVariable) ? $"background: var({DateBackgroundCssVariable});" : string.Empty;
        }
        
        IEnumerable<Color> colors = closestPastDate.Colors[date.DayOfWeek];
        
        return $"background: repeating-linear-gradient(to right, {colors.GetHtmlGradient()});";
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
            return $"color: {specialDate.TextColor};";
        }
        
        IWeekTemplate? closestPastDate = Templates
            .Where(d => d.AcceptFromDate < date)
            .OrderByDescending(d => d)
            .FirstOrDefault();

        return closestPastDate == null ? string.Empty : $"color: {ColorTranslator.ToHtml(closestPastDate.TextColor)};";
    }
}
