namespace MovieCatalogLibrary.model;

/// <summary>
/// Класс, представляющий DTO (Data Transfer Object) для фильма.
/// Используется для передачи данных о фильме между слоями приложения.
/// </summary>
public class FilmDto
{
    /// <summary>
    /// Конструктор для инициализации объекта FilmDto.
    /// </summary>
    /// <param name="title">Название фильма.</param>
    /// <param name="year">Год выпуска фильма.</param>
    /// <param name="genre">Жанр фильма.</param>
    /// <param name="poster">Ссылка на постер фильма.</param>
    /// <param name="imdbRating">Рейтинг IMDb.</param>
    /// <param name="metaScore">Рейтинг Metacritic.</param>
    /// <param name="plot">Описание сюжета фильма.</param>
    /// <param name="director">Режиссер фильма.</param>
    /// <param name="actors">Список актеров, участвующих в фильме.</param>
    /// <param name="imdbId">Идентификатор фильма на IMDb.</param>
    public FilmDto(
        string title,
        string year,
        string genre,
        string poster,
        string imdbRating,
        string metaScore,
        string plot,
        string director,
        string actors,
        string imdbId)
    {
        Title = title;
        Year = year;
        Genre = genre;
        Poster = poster;
        ImdbRating = imdbRating;
        MetaScore = metaScore;
        Plot = plot;
        Director = director;
        Actors = actors;
        ImdbId = imdbId;
    }

    /// <summary>
    /// Название фильма.
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Год выпуска фильма.
    /// </summary>
    public string Year { get; init; }

    /// <summary>
    /// Идентификатор фильма на IMDb.
    /// </summary>
    public string ImdbId { get; }

    /// <summary>
    /// Жанр фильма.
    /// </summary>
    public string Genre { get; init; }

    /// <summary>
    /// Ссылка на постер фильма.
    /// </summary>
    public string Poster { get; init; }

    /// <summary>
    /// Рейтинг фильма на IMDb.
    /// </summary>
    public string ImdbRating { get; }

    /// <summary>
    /// Рейтинг фильма на Metacritic.
    /// </summary>
    public string MetaScore { get; }

    /// <summary>
    /// Описание сюжета фильма.
    /// </summary>
    public string Plot { get; init; }

    /// <summary>
    /// Режиссер фильма.
    /// </summary>
    public string Director { get; init; }

    /// <summary>
    /// Список актеров, участвующих в фильме.
    /// </summary>
    public string Actors { get; init; }

    /// <summary>
    /// Возвращает строковое представление объекта FilmDto.
    /// </summary>
    /// <returns>Строка, содержащая информацию о фильме.</returns>
    public override string ToString()
    {
        return $"Название: {Title}\n" +
               $"Год выпуска: {Year}\n" +
               $"Жанр: {Genre}\n" +
               $"Постер: {Poster}\n" +
               $"Рейтинг IMDb: {ImdbRating}\n" +
               $"Рейтинг Metacritic: {MetaScore}\n" +
               $"Описание сюжета: {Plot}\n" +
               $"Режиссер: {Director}\n" +
               $"Актеры: {Actors}\n" +
               $"IMDb ID: {ImdbId}";
    }
}