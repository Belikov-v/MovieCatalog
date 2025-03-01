using System.Globalization;
using MovieCatalogLibrary.model;
using MovieCatalogLibrary.service.integrations;

namespace MovieCatalogLibrary.service.parsers;

/// <summary>
/// Класс для преобразования FilmDto в Film.
/// </summary>
public static class FilmConverter
{
    /// <summary>
    /// Преобразует объект FilmDto в объект Film.
    /// </summary>
    /// <param name="filmDto">Объект FilmDto.</param>
    /// <param name="isUpdate">Флаг, указывающий, является ли это обновлением.</param>
    /// <returns>Объект Film.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если filmDto равен null.</exception>
    public static Film ConvertFilmDtoToFilm(FilmDto filmDto, bool isUpdate = false)
    {
        // Проверка на null.
        if (filmDto == null)
            throw new ArgumentNullException(nameof(filmDto), "FilmDto не может быть null.");

        var film = new Film
        {
            Name = filmDto.Title,
            ReleaseYear = ParseReleaseYear(filmDto.Year),
            Genre = ParseGenre(filmDto.Genre),
            Rating = ParseRating(filmDto.ImdbRating),
            Plot = filmDto.Plot,
            Director = filmDto.Director,
            Actors = ParseActors(filmDto.Actors),
            ImdbRating = filmDto.ImdbRating,
            MetacriticRating = filmDto.MetaScore,
            TrailerUrl = ParseTrailerUrl(filmDto.ImdbId)
        };

        // Обрабатываем постер только если это не обновление или постер отсутствует.
        if (!isUpdate || string.IsNullOrEmpty(film.Poster))
        {
            film.Poster = ParsePoster(filmDto.Poster);
        }

        return film;
    }

    /// <summary>
    /// Преобразует строку с годом выпуска в целое число.
    /// </summary>
    /// <param name="year">Год выпуска в виде строки.</param>
    /// <returns>Год выпуска в виде целого числа.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если год выпуска некорректен.</exception>
    private static int ParseReleaseYear(string year)
    {
        if (string.IsNullOrEmpty(year))
            throw new ArgumentException("Год выпуска не может быть пустым.");

        if (int.TryParse(year, out var releaseYear))
            return releaseYear;

        throw new ArgumentException("Некорректный год выпуска.");
    }

    /// <summary>
    /// Формирует URL трейлера на основе IMDb ID.
    /// </summary>
    /// <param name="imdbId">IMDb ID фильма.</param>
    /// <returns>URL трейлера.</returns>
    private static string ParseTrailerUrl(string imdbId)
    {
        return $"https://www.imdb.com/title/{imdbId}/videogallery";
    }

    /// <summary>
    /// Загружает постер по URL.
    /// </summary>
    /// <param name="urlToDownloadPoster">URL постера.</param>
    /// <returns>Путь к сохраненному постеру.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если URL постера некорректен.</exception>
    private static string ParsePoster(string urlToDownloadPoster)
    {
        if (string.IsNullOrEmpty(urlToDownloadPoster))
            throw new ArgumentException("URL постера не может быть пустым.");

        return PosterDownloader.DownloadPosterAsync(urlToDownloadPoster).Result;
    }

    /// <summary>
    /// Преобразует строку с актерами в список.
    /// </summary>
    /// <param name="actorsInString">Строка с актерами, разделенными запятыми.</param>
    /// <returns>Список актеров.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если строка с актерами пуста.</exception>
    private static List<string> ParseActors(string actorsInString)
    {
        if (string.IsNullOrEmpty(actorsInString))
            throw new ArgumentException("Список актеров не может быть пустым.");

        return actorsInString.Split(',').Select(actor => actor.Trim()).ToList();
    }

    /// <summary>
    /// Преобразует строку с жанрами в перечисление GenreOfFilm.
    /// </summary>
    /// <param name="genreString">Строка с жанрами.</param>
    /// <returns>Перечисление GenreOfFilm.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если жанр некорректен.</exception>
    private static GenreOfFilm ParseGenre(string genreString)
    {
        if (string.IsNullOrEmpty(genreString))
            throw new ArgumentException("Жанр не может быть пустым.");

        var firstGenre = genreString.Split(',')[0].Trim();

        if (Enum.TryParse(firstGenre, ignoreCase: true, out GenreOfFilm genre))
        {
            return genre;
        }

        throw new ArgumentException($"Неизвестный жанр: {firstGenre}");
    }

    /// <summary>
    /// Преобразует рейтинг IMDb в целое число от 1 до 10.
    /// </summary>
    /// <param name="imdbRating">Рейтинг IMDb в виде строки.</param>
    /// <returns>Рейтинг в виде целого числа.</returns>
    private static int ParseRating(string imdbRating)
    {
        if (string.IsNullOrEmpty(imdbRating))
            return 0;

        // Используем CultureInfo.InvariantCulture для корректного парсинга.
        if (!double.TryParse(imdbRating, NumberStyles.Any, CultureInfo.InvariantCulture, out double rating))
            return 0;

        // Ограничиваем рейтинг значением 10.
        if (rating > 10)
            return 10;

        // Преобразуем рейтинг IMDb (например, 8.8) в целое число (например, 9).
        return (int)Math.Round(rating);
    }
}