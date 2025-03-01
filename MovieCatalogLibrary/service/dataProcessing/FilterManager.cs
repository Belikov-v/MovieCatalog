using MovieCatalogLibrary.model;
using Spectre.Console;

namespace MovieCatalogLibrary.service.dataProcessing;

/// <summary>
/// Класс для создания фильтров по жанру и рейтингу фильмов.
/// </summary>
public static class FilterManager
{
    /// <summary>
    /// Создает фильтр по жанру.
    /// </summary>
    /// <param name="filmsForOperation">Список фильмов для фильтрации.</param>
    /// <returns>Операция фильтрации по жанру.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="filmsForOperation"/> равен null.</exception>
    /// <exception cref="InvalidOperationException">Выбрасывается, если <paramref name="filmsForOperation"/> пуст.</exception>
    public static Operation CreateGenreFilter(List<Film> filmsForOperation)
    {
        // Проверка на null.
        if (filmsForOperation == null)
            throw new ArgumentNullException(nameof(filmsForOperation), "Список фильмов не может быть null.");

        // Проверка на пустой список.
        if (filmsForOperation.Count == 0)
            throw new InvalidOperationException("Список фильмов пуст.");

        // Получаем уникальные жанры из списка фильмов.
        var genres = filmsForOperation.Select(f => f.Genre.ToString()).Distinct().ToList();
        genres.Insert(0, "Все жанры");

        // Запрашиваем у пользователя выбор жанра.
        var selectedGenre = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Выберите жанр для фильтрации:")
                .AddChoices(genres)
        );

        // Возвращаем операцию фильтрации.
        return new Operation
        {
            Name = $"Фильтр по жанру: {selectedGenre}",
            Apply = films => selectedGenre == "Все жанры"
                ? films
                : films.Where(f => f.Genre.ToString() == selectedGenre).ToList()
        };
    }

    /// <summary>
    /// Создает фильтр по рейтингу.
    /// </summary>
    /// <param name="filmsForOperation">Список фильмов для фильтрации.</param>
    /// <returns>Операция фильтрации по рейтингу.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="filmsForOperation"/> равен null.</exception>
    /// <exception cref="InvalidOperationException">Выбрасывается, если <paramref name="filmsForOperation"/> пуст.</exception>
    public static Operation CreateRatingFilter(List<Film> filmsForOperation)
    {
        // Проверка на null.
        if (filmsForOperation == null)
            throw new ArgumentNullException(nameof(filmsForOperation), "Список фильмов не может быть null.");

        // Проверка на пустой список.
        if (filmsForOperation.Count == 0)
            throw new InvalidOperationException("Список фильмов пуст.");

        // Получаем уникальные рейтинги из списка фильмов.
        var ratings = filmsForOperation.Select(f => f.Rating.ToString()).Distinct().ToList();
        ratings.Insert(0, "Любой рейтинг");

        // Запрашиваем у пользователя выбор рейтинга.
        var selectedRating = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Выберите рейтинг для фильтрации:")
                .AddChoices(ratings)
        );

        // Возвращаем операцию фильтрации.
        return new Operation
        {
            Name = $"Фильтр по рейтингу: {selectedRating}",
            Apply = films => selectedRating == "Любой рейтинг"
                ? films
                : films.Where(f => f.Rating.ToString() == selectedRating).ToList()
        };
    }
}