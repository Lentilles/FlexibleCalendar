# FlexibleCalendar

### EN

FlexibleCalendar is a Blazor component for displaying a calendar with the ability to color specific dates using `System.Drawing.Color` and support for interactive actions.

## Quick Start

1. Add the NuGet package `FlexibleCalendar` to your Blazor project.
2. Import the namespace:

```razor
@using FlexibleCalendar.Calendar

<Calendar MonthsToShow="6" CalendarColumns="3" />
```

This will display 6 months starting from 2025-01-01, 3 horizontally and 2 vertically.

## Usage Example


```razor
@using FlexibleCalendar.Calendar

<Calendar MonthsToShow="4"
          CalendarColumns="2"
          DateBackgroundCssVariable="--bs-body-bg"
          Events="_events"
          OnDayClick="OnDayClickHandler" />
```

```csharp
private List<CalendarEvent> _events =
    [new(DateTime.Now, DateTime.Now.AddDays(7), Color.DarkSeaGreen, Color.Azure),
     new(DateTime.Now.AddDays(9), DateTime.Now.AddDays(15), Color.DarkOrange, Color.Black)];

private void OnDayClickHandler(DateTime date)
{
    // Your code to handle day click
}
```

## Calendar Component Parameters


| Parameter                   | Type                          | Description                                                                                                                        | Requirement                                      |
|-----------------------------|-------------------------------|------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------|
| `MonthsToShow`              | `int`                         | Number of months to display at once. Allowed values: 1 to 12.                                                                      | Optional. Default is `1`.                        |
| `CalendarColumns`           | `int`                         | Number of columns for the months grid.                                                                                             | Optional. Default is `1`.                        |
| `StartDate`                 | `DateTime?`                   | Start date for the calendar. Defaults to January 1st of the current year.                                                          | Optional. Default is January 1st of current year. |
| `Culture`                   | `CultureInfo?`                | Culture for month and weekday names.                                                                                               | Optional. Default is `en-US`.                    |
| `Events`                    | `IEnumerable<ICalendarEvent>?`| Collection of events to display. Each event specifies a date range and colors.                                                     | Optional. Default is `Array.Empty`.              |
| `OnDayClick`                | `EventCallback<DateTime>`      | Event triggered when a day is clicked. Returns the selected date.                                                                  | Optional.                                        |
| `DateBackgroundCssVariable` | `string?`                     | CSS variable for the background of date cells (e.g., `--my-bg`). Used for all dates not in `Events`. Useful for theme compatibility.| Optional.                                        |

### Event Format


To display events, use objects implementing the `ICalendarEvent` interface. The default implementation is the `CalendarEvent` class.

Structure:

```csharp
public interface ICalendarEvent
{
    DateTime StartDate { get; set; }
    DateTime EndDate { get; set; }
    Color Color { get; set; }
    Color TextColor { get; set; }
}
```

## Styling

The component uses the following CSS classes:
- `.calendar` — calendar container
- `.calendar-header` — header with navigation
- `.calendar-months-grid` — months grid
- `.calendar-month-title` — month title
- `.calendar-weekdays` — weekdays row
- `.calendar-day` — day cell
- `.calendar-day.empty` — empty cell (for alignment)

You can override styles in your project or use CSS variables for customization.

### RU

Blazor-компонент для отображения календаря с возможностью окраски конкретных дат в различные цвета `System.Drawing.Color` с поддержкой интерактивных действий

## Быстрый старт

1. Добавьте Nuget пакет `FlexibleCalendar` на проект в ваш Blazor-проект.
2. Импортируйте пространство имён:
  
```razor
@using FlexibleCalendar.Calendar

<Calendar MonthsToShow="6" CalendarColumns="3" /> 
```

Отобразит 6 месяцев, начиная с 2025.01.01, 3 по горизонтали, 2 по вертикали.
## Пример использования


```razor
@using FlexibleCalendar.Calendar 

<Calendar MonthsToShow="4"
          CalendarColumns="2"
          DateBackgroundCssVariable="--bs-body-bg"
          Events="_events"
          OnDayClick="OnDayClickHandler" />

```

  

```csharp

private List<CalendarEvent> _events =

    [new(DateTime.Now, DateTime.Now.AddDays(7), Color.DarkSeaGreen, Color.Azure),

     new(DateTime.Now.AddDays(9), DateTime.Now.AddDays(15), Color.DarkOrange, Color.Black)];


private void OnDayClickHandler(DateTime date)

{

    // Ваш код обработки клика по дню

}

```

## Параметры компонента Calendar


| Параметр                    | Тип                            | Описание                                                                                                                                                                                 | Требование                                                 |
| --------------------------- | ------------------------------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------- |
| `MonthsToShow`              | `int`                          | Количество месяцев для отображения одновременно. Допустимые значения: от 1 до 12.                                                                                                        | ==Не обязательно==. По умолчанию `1`.                      |
| `CalendarColumns`           | `int`                          | Количество колонок для сетки месяцев.                                                                                                                                                    | ==Не обязательно==, по умолчанию `1`.                      |
| `StartDate`                 | `DateTime?`                    | Дата начала отображения календаря. По умолчанию 1 января текущего года.                                                                                                                  | ==Не обязательно==, по умолчанию `1` января текущего года. |
| `Culture`                   | `CultureInfo?`                 | Культура для названий месяцев и дней недели.                                                                                                                                             | ==Не обязательно==, по умолчанию `en-US`                   |
| `Events`                    | `IEnumerable<ICalendarEvent>?` | Коллекция событий для отображения. Каждое событие задаёт диапазон дат и цвета.                                                                                                           | ==Не обязательно== по умолчанию `Array.Empty`              |
| `OnDayClick`                | `EventCallback<DateTime>`      | Событие, вызываемое при клике по дню. Возвращает выбранную дату.                                                                                                                         | ==Не обязательно==.                                        |
| `DateBackgroundCssVariable` | `string?`                      | CSS-переменная для фона ячеек дат (например, `--my-bg`). Если задана, применяется ко всем датам не входящим в `Events`.  Нужно для совместимости с разными темами, разных css библиотек. | ==Не обязательно==.                                        |

### Формат событий


Для отображения событий используйте объекты, реализующие интерфейс `ICalendarEvent`, по умолчанию есть класс `CalendarEvent.

Его структура:

```csharp
public interface ICalendarEvent  
{  
    DateTime StartDate { get; set; }  
    DateTime EndDate { get; set; }  
    Color Color { get; set; }  
    Color TextColor { get; set; }  
}
```

## Стилизация

Компонент использует CSS-классы:

- `.calendar` — контейнер календаря

- `.calendar-header` — шапка с навигацией

- `.calendar-months-grid` — сетка месяцев

- `.calendar-month-title` — название месяца

- `.calendar-weekdays` — строка дней недели

- `.calendar-day` — ячейка дня

- `.calendar-day.empty` — пустая ячейка (для выравнивания)
  
Вы можете переопределить стили в своём проекте или использовать CSS-переменные для кастомизации.
