using System.Text;
using MovieCatalogLibrary.model;
using MovieCatalogLibrary.service;
using MovieCatalogLibrary.service.visualizations;
using MovieCatalogLibrary.service.сoreOperations;

namespace MovieCatalogConsole.service;

/// <summary>
/// Класс для работы с фильмами, предоставляющий методы для отображения, редактирования и управления фильмами.
/// </summary>
public static class FilmOperator
{
    private static readonly IFilmManager FilmManager = new InMemoryFilmManager();
    private static readonly Dictionary<GenreOfFilm, int> GenreAccessCount = new();

    /// <summary>
    /// Отображает таблицу с фильмами.
    /// </summary>
    public static void DisplayTable()
    {
        var table = new FilmTableView();
        var films = FilmManager.GetAllFilms().ToList();
        table.Render(films);
    }

    /// <summary>
    /// Отображает график средних рейтингов по жанрам.
    /// </summary>
    public static void DisplayRatingByGenre()
    {
        RatingVisualizer.PlotRatingsByGenre(GetAllFilmsFromStorage());
    }

    /// <summary>
    /// Отображает гистограмму рейтингов фильмов.
    /// </summary>
    public static void DisplayRatingHistogram()
    {
        RatingVisualizer.PlotRatingsHistogram(GetAllFilmsFromStorage());
    }

    /// <summary>
    /// Отображает рекомендации фильмов на основе популярных жанров.
    /// </summary>
    public static void DisplayRecommendations()
    {
        if (GenreAccessCount.Keys.Count == 0)
        {
            DisplayAllFilms();
        }
        else
        {
            int uniqueGenresCount = 3;
            int topN = GenreAccessCount.Keys.Count >= uniqueGenresCount
                ? uniqueGenresCount
                : GenreAccessCount.Keys.Count;
            List<GenreOfFilm> popularGenres = GetMostPopularGenres(topN);
            List<Film> films = FilmManager.GetRecommendedFilms(popularGenres);
            foreach (var film in films)
            {
                Console.WriteLine(FormFilmForDisplaying(film));
            }
        }
    }

    /// <summary>
    /// Отображает диаграммы распределения фильмов по жанрам и рейтингам.
    /// </summary>
    public static void DisplayСhart()
    {
        List<Film> films = FilmManager.GetAllFilms().ToList();
        Console.WriteLine("Диаграмма распределения фильмов по жанрам:");
        BreakdownChart.GetGenreChart(films);
        Console.WriteLine("Диаграмма распределения фильмов по рейтингам:");
        BreakdownChart.GetRatingChart(films);
    }

    /// <summary>
    /// Отображает информацию о фильме по его Id.
    /// </summary>
    public static void GetFilm()
    {
        var id = GetIdFilm();
        var film = FilmManager.GetFilm(id);
        GenreAccessCount[film.Genre] = GenreAccessCount.GetValueOrDefault(film.Genre) + 1;
        Console.WriteLine("Выбранный фильм: \n");
        Console.WriteLine(FormFilmForDisplaying(film));
    }

    /// <summary>
    /// Отображает карусель с обложками фильмов.
    /// </summary>
    public static void DisplayCarousel()
    {
        var films = FilmManager.GetAllFilms().ToList();
        FilmCarousel.Render(films);
    }

    /// <summary>
    /// Отображает все фильмы в каталоге.
    /// </summary>
    public static void DisplayAllFilms()
    {
        var stringBuilder = new StringBuilder($"Каталог фильмов:");

        foreach (var film in GetAllFilmsFromStorage())
        {
            stringBuilder.Append(FormFilmForDisplaying(film));
        }

        Console.WriteLine(stringBuilder.ToString());
    }

    /// <summary>
    /// Форматирует информацию о фильме для отображения.
    /// </summary>
    /// <param name="film">Фильм для форматирования.</param>
    /// <returns>Строка с информацией о фильме.</returns>
    private static string FormFilmForDisplaying(Film film)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"\n {new string('-', 20)} \n");
        stringBuilder.Append(film);
        stringBuilder.Append($"\n {new string('-', 20)} \n");
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Редактирует рейтинг фильма.
    /// </summary>
    public static void EditRatingFilm()
    {
        var id = GetIdFilm();
        var film = FilmManager.GetFilm(id);
        FilmBuilder.SetRating(film);
        FilmManager.EditFilm(film);
    }

    /// <summary>
    /// Удаляет фильм по его Id.
    /// </summary>
    public static void DeleteFilm()
    {
        var id = GetIdFilm();
        FilmManager.DeleteFilm(id);
    }

    /// <summary>
    /// Получает Id фильма от пользователя.
    /// </summary>
    /// <returns>Id фильма.</returns>
    private static int GetIdFilm()
    {
        while (true)
        {
            Console.WriteLine("Введите id фильма.");
            try
            {
                var id = int.Parse(Console.ReadLine() ?? throw new ArgumentNullException());
                if (!FilmManager.IsIdExist(id)) throw new KeyNotFoundException();
                return id;
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Ошибка: Некорректный формат ввода. (Детали: {e.Message})");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Ошибка: Входное значение не может быть null. (Детали: {e.Message})");
            }
            catch (OverflowException e)
            {
                Console.WriteLine($"Ошибка: Введенное значение выходит за допустимые пределы. (Детали: {e.Message})");
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine($"Ошибка: Запрашиваемый ключ не найден. (Детали: {e.Message})");
            }
        }
    }

    /// <summary>
    /// Добавляет фильм в хранилище.
    /// </summary>
    /// <param name="film">Фильм для добавления.</param>
    public static void AddFilmToStorage(Film film)
    {
        FilmManager.AddCustomFilm(film);
    }

    /// <summary>
    /// Возвращает все фильмы из хранилища.
    /// </summary>
    /// <returns>Список фильмов.</returns>
    public static List<Film> GetAllFilmsFromStorage()
    {
        return FilmManager.GetAllFilms().OrderBy(film => film.Id).ToList();
    }

    /// <summary>
    /// Добавляет новый фильм в хранилище.
    /// </summary>
    public static void AddFilm()
    {
        var film = FilmBuilder.BuildFilm(); // Создание фильма
        AddFilmToStorage(film); // Добавление фильма
    }

    /// <summary>
    /// Возвращает список самых популярных жанров.
    /// </summary>
    /// <param name="topN">Количество жанров для возврата.</param>
    /// <returns>Список популярных жанров.</returns>
    private static List<GenreOfFilm> GetMostPopularGenres(int topN)
    {
        // Сортируем словарь по значениям (количеству обращений) в порядке убывания
        var sortedGenres = GenreAccessCount.OrderByDescending(pair => pair.Value);

        // Берем топ-N жанров
        var topGenres = sortedGenres.Take(topN).Select(pair => pair.Key).ToList();

        return topGenres;
    }

    /// <summary>
    /// Добавляет существующий фильм из внешнего источника.
    /// </summary>
    public static void AddExistingFilm()
    {
        while (true)
        {
            var film = new Film();
            FilmBuilder.SetName(film);
            FilmBuilder.SetReleaseYear(film);
            try
            {
                FilmManager.AddExistingFilm(film);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    /// <summary>
    /// Обновляет данные о фильмах.
    /// </summary>
    /// <param name="state">Состояние.</param>
    public static void UpdateData(object? state)
    {
        FilmManager.UpdateData();
    }
}