using MovieCatalogLibrary.model;
using ScottPlot;

namespace MovieCatalogLibrary.service.visualizations;

/// <summary>
/// Класс для визуализации рейтингов фильмов.
/// </summary>
public static class RatingVisualizer
{
    // Относительный путь к папке с ресурсами.
    private static readonly string ResourcesPath = Path.Combine("resources");

    /// <summary>
    /// Создает гистограмму рейтингов фильмов и сохраняет её в файл.
    /// </summary>
    /// <param name="films">Список фильмов.</param>
    /// <param name="outputPath">Путь для сохранения гистограммы.</param>
    public static void PlotRatingsHistogram(List<Film> films, string? outputPath = null)
    {
        // Если путь не указан, используем путь по умолчанию.
        outputPath ??= Path.Combine(ResourcesPath, "ratings_histogram.png");

        // Проверка на null.
        if (films == null)
            throw new ArgumentNullException(nameof(films), "Список фильмов не может быть null.");

        // Проверка на пустой список.
        if (films.Count == 0)
        {
            Console.WriteLine("Нет данных для гистограммы.");
            return;
        }

        // Извлекаем рейтинги фильмов.
        var ratings = films.Select(f => (double)f.Rating).ToArray();

        // Создаем гистограмму.
        var hist = ScottPlot.Statistics.Histogram.WithBinCount(20, ratings);
        var plot = new Plot();

        // Добавляем гистограмму на график.
        plot.Add.Bars(hist.Bins, hist.Counts);

        // Настраиваем график.
        plot.Title("Гистограмма рейтингов");
        plot.XLabel("Рейтинг");
        plot.YLabel("Количество фильмов");

        // Сохраняем график в файл.
        EnsureDirectoryExists(outputPath); // Убедимся, что директория существует.
        plot.SavePng(outputPath, 800, 600);
        Console.WriteLine($"Гистограмма сохранена в {outputPath}");
    }

    /// <summary>
    /// Создает график средних рейтингов по жанрам и сохраняет его в файл.
    /// </summary>
    /// <param name="films">Список фильмов.</param>
    /// <param name="outputPath">Путь для сохранения графика.</param>
    public static void PlotRatingsByGenre(List<Film> films, string? outputPath = null)
    {
        // Если путь не указан, используем путь по умолчанию.
        outputPath ??= Path.Combine(ResourcesPath, "ratings_by_genre.png");

        // Проверка на null.
        if (films == null)
            throw new ArgumentNullException(nameof(films), "Список фильмов не может быть null.");

        // Группируем фильмы по жанрам и вычисляем средний рейтинг.
        var genreRatings = films
            .GroupBy(f => f.Genre)
            .Select(g => new { Genre = g.Key.ToString(), AvgRating = g.Average(f => f.Rating) })
            .ToList();

        // Проверка на пустой список.
        if (genreRatings.Count == 0)
        {
            Console.WriteLine("Нет данных для распределения по жанрам.");
            return;
        }

        // Создаем график.
        var plot = new Plot();

        // Средние рейтинги для каждого жанра.
        var values = genreRatings.Select(g => g.AvgRating).ToArray();

        // Создаем столбцы.
        var bar = plot.Add.Bars(values);
        foreach (var bb in bar.Bars)
        {
            bb.Size *= 0.3; // Уменьшаем ширину столбцов.
        }

        // Настраиваем метки оси X.
        var positions = Enumerable.Range(0, genreRatings.Count).Select(i => (double)i).ToArray();
        plot.Axes.Bottom.SetTicks(positions, genreRatings.Select(g => g.Genre).ToArray());

        // Настраиваем график.
        plot.Axes.Margins(bottom: 0);
        plot.Axes.AutoScaleX();
        plot.Title("Средний рейтинг по жанрам");
        plot.XLabel("Жанр");
        plot.YLabel("Средний рейтинг");

        // Сохраняем график в файл.
        EnsureDirectoryExists(outputPath); // Убедимся, что директория существует.
        plot.SavePng(outputPath, 1100, 800);
        Console.WriteLine($"График распределения по жанрам сохранен в {outputPath}");
    }

    /// <summary>
    /// Убеждается, что директория для сохранения файла существует. Если нет, создает её.
    /// </summary>
    /// <param name="filePath">Путь к файлу.</param>
    private static void EnsureDirectoryExists(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}