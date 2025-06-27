using FlexibleCalendar.Interfaces;
using System.Drawing;

namespace FlexibleCalendar.Models;

public class SpecialDate : ISpecialDate
{
    public DateOnly Date { get; set; }
    public IEnumerable<Color> Colors { get; set; } = [];
    public Color TextColor { get; set; }
}