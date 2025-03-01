using MovieCatalogLibrary.model;
using Spectre.Console;

namespace MovieCatalogLibrary.service.visualizations;

/// <summary>
/// Класс для создания и отображения круговых диаграмм (Breakdown Chart) на основе данных о фильмах.
/// </summary>
public static class BreakdownChart
{
    // Список доступных цветов для графиков.
    private static readonly List<Color> AvailableColors =
    [
        Color.Red,
        Color.Green,
        Color.Blue,
        Color.Yellow,
        Color.Purple,
        Color.Orange1,
        Color.Teal,
        Color.Maroon,
        Color.Olive,
        Color.Navy
    ];

    /// <summary>
    /// Создает и отображает круговую диаграмму распределения жанров фильмов.
    /// </summary>
    /// <param name="films">Список фильмов.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если films равен null.</exception>
    public static void GetGenreChart(IEnumerable<Film> films)
    {
        // Проверка на null.
        if (films == null)
            throw new ArgumentNullException(nameof(films), "Список фильмов не может быть null.");

        // Группируем фильмы по жанрам и подсчитываем количество.
        var genreDistribution = films
            .GroupBy(f => f.Genre)
            .Select(g => new { Genre = g.Key, Count = g.Count() });

        // Создаем диаграмму.
        var breakdownChart = new Spectre.Console.BreakdownChart()
            .Width(70);

        var random = new Random();

        // Добавляем элементы в диаграмму.
        foreach (var item in genreDistribution)
        {
            // Выбираем случайный цвет из списка.
            var color = AvailableColors[random.Next(AvailableColors.Count)];
            breakdownChart.AddItem(item.Genre.ToString(), item.Count, color);
        }

        // Отображаем диаграмму.
        AnsiConsole.Write(breakdownChart);
    }

    /// <summary>
    /// Создает и отображает круговую диаграмму распределения рейтингов фильмов.
    /// </summary>
    /// <param name="films">Список фильмов.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если films равен null.</exception>
    public static void GetRatingChart(IEnumerable<Film> films)
    {
        // Проверка на null.
        if (films == null)
            throw new ArgumentNullException(nameof(films), "Список фильмов не может быть null.");

        // Группируем фильмы по рейтингам и подсчитываем количество.
        var ratingDistribution = films
            .OrderBy(f => f.Rating)
            .GroupBy(f => f.Rating)
            .Select(g => new { Rating = g.Key, Count = g.Count() });

        // Создаем диаграмму.
        var breakdownChart = new Spectre.Console.BreakdownChart()
            .Width(70);

        var random = new Random();

        // Добавляем элементы в диаграмму.
        foreach (var item in ratingDistribution)
        {
            // Выбираем случайный цвет из списка.
            var color = AvailableColors[random.Next(AvailableColors.Count)];
            breakdownChart.AddItem(item.Rating.ToString(), item.Count, color);
        }

        // Отображаем диаграмму.
        AnsiConsole.Write(breakdownChart);
    }
}