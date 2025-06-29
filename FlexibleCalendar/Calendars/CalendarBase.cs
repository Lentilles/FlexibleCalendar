using FlexibleCalendar.Extensions;
using FlexibleCalendar.Models;
using Microsoft.AspNetCore.Components;
using System.Drawing;
using System.Globalization;

namespace FlexibleCalendar.Calendars;

/// <summary>
/// The base class for rendering a flexible calendar component with multiple customizable months,
/// visual styles, and culture-specific formatting.
/// </summary>
public abstract class CalendarBase : ComponentBase
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
    /// Event triggered when a day is clicked. Returns the clicked date.
    /// </summary>
    [Parameter]
    public EventCallback<DateOnly> OnDayClick { get; set; }

    /// <summary>
    /// The CSS variable name (e.g. --my-bg) to use as the background for date numbers.
    /// If set, it will be used as a background for all date cells.
    /// </summary>
    [Parameter]
    public string? DateBackgroundCssVariable { get; set; }

    /// <summary>
    /// Determines how date cells are styled with color. Options include filled or pilled styles.
    /// </summary>
    [Parameter]
    public ColoredStyle ColorizeStyle { get; set; } = ColoredStyle.Filled;

    /// <summary>
    /// Gets the first date that appears in the currently displayed months.
    /// </summary>
    public DateOnly CalendarFirstDate => MonthsDays.First().FirstOrDefault(x => x.HasValue)!.Value;

    /// <summary>
    /// Gets the last date that appears in the currently displayed months.
    /// </summary>
    public DateOnly CalendarEndDate => MonthsDays.Last().LastOrDefault(x => x.HasValue)!.Value;

    /// <summary>
    /// Gets the year of the first displayed month.
    /// </summary>
    public int StartYear => DisplayedMonths.First().Year;

    /// <summary>
    /// Gets the year of the last displayed month.
    /// </summary>
    public int EndYear => DisplayedMonths.Last().Year;

    /// <summary>
    /// Abbreviated names of weekdays, adjusted for the selected culture and start-of-week setting.
    /// </summary>
    protected string[] WeekDays { get; set; } = [];

    /// <summary>
    /// A list of lists containing nullable dates, representing each day in each displayed month.
    /// Null values represent empty cells (e.g., padding before the first day of the month).
    /// </summary>
    protected List<List<DateOnly?>> MonthsDays { get; set; } = [];

    /// <summary>
    /// List of displayed months along with their year and name for header or labeling purposes.
    /// </summary>
    protected List<(int Year, string MonthName)> DisplayedMonths { get; set; } = [];
    
    /// <summary>
    /// The first month currently displayed in the calendar.
    /// </summary>
    private DateOnly DisplayedMonth { get; set; }

    /// <summary>
    /// Returns a CSS style string that sets the text color based on a string color value.
    /// </summary>
    /// <param name="color">A string representing the color (e.g., "#FF0000").</param>
    /// <returns>CSS style string for setting the text color.</returns>
    protected virtual string GetDateTextColor(string color)
    {
        return $"color: {color};";
    }

    /// <summary>
    /// Returns a CSS style string that sets the text color based on a <see cref="Color"/> object.
    /// </summary>
    /// <param name="color">A System.Drawing.Color value.</param>
    /// <returns>CSS style string for setting the text color.</returns>
    protected virtual string GetDateTextColor(Color color)
    {
        return $"color: {ColorTranslator.ToHtml(color)};";
    }

    /// <summary>
    /// Returns a CSS background style string for a set of colors based on the selected <see cref="ColoredStyle"/>.
    /// </summary>
    /// <param name="colors">The list of colors to be used in the gradient.</param>
    /// <param name="style">The style of coloring (Filled or Pilled).</param>
    /// <returns>Formatted CSS background gradient string.</returns>
    protected virtual string GetMarkedDateColors(IEnumerable<Color> colors, ColoredStyle style)
    {
        string gradientDirection = style == ColoredStyle.Pilled ? "to right" : "45 deg";
        return $"background: repeating-linear-gradient({gradientDirection}, {colors.GetHtmlGradient()});";
    }

    /// <summary>
    /// Performs component initialization including setting the displayed month,
    /// culture settings, and weekday names.
    /// </summary>
    protected override void OnInitialized()
    {
        DisplayedMonth = StartDate ?? new DateOnly(DateTime.Today.Year, 1, 1);
        CultureInfo culture = Culture ?? CultureInfo.CurrentCulture;
        WeekDays = culture.DateTimeFormat.AbbreviatedDayNames;
        if (culture.DateTimeFormat.FirstDayOfWeek == DayOfWeek.Monday)
        {
            WeekDays = WeekDays.Skip(1).Concat(WeekDays.Take(1)).ToArray();
        }
        GenerateDays();
    }

    /// <summary>
    /// Validates and adjusts the number of months to show to be between 1 and 12.
    /// </summary>
    protected override void OnParametersSet()
    {
        MonthsToShow = Math.Max(1, Math.Min(12, MonthsToShow));
    }

    /// <summary>
    /// Navigates backward in time by the number of months currently displayed.
    /// Updates the calendar accordingly.
    /// </summary>
    protected void ShowPreviousMonth()
    {
        DisplayedMonth = DisplayedMonth.AddMonths(-MonthsToShow);
        GenerateDays();
    }

    /// <summary>
    /// Navigates forward in time by the number of months currently displayed.
    /// Updates the calendar accordingly.
    /// </summary>
    protected void ShowNextMonth()
    {
        DisplayedMonth = DisplayedMonth.AddMonths(MonthsToShow);
        GenerateDays();
    }

    /// <summary>
    /// Returns a CSS background style string referencing the provided CSS variable.
    /// </summary>
    /// <param name="cssBgVariable">The CSS variable name (e.g., --primary-bg).</param>
    /// <returns>Formatted background style string or empty string if input is null/empty.</returns>
    protected string GetColorDateLikeBg(string? cssBgVariable)
    {
        return !string.IsNullOrWhiteSpace(cssBgVariable) ? $"background: var({cssBgVariable});" : string.Empty;
    }

    /// <summary>
    /// Generates day grids and labels for the currently displayed months.
    /// This method is called when navigating months or initializing the calendar.
    /// </summary>
    private void GenerateDays()
    {
        MonthsDays.Clear();
        DisplayedMonths.Clear();
        CultureInfo culture = Culture ?? CultureInfo.CurrentCulture;

        for (int m = 0; m < MonthsToShow; m++)
        {
            DateOnly month = DisplayedMonth.AddMonths(m);
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
}
