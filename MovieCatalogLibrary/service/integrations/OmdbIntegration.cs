using System.Text.Json;
using MovieCatalogLibrary.model;

namespace MovieCatalogLibrary.service.integrations;

public static class OmdbIntegration
{
    // Используем статический HttpClient для повторного использования соединений.
    private static readonly HttpClient HttpClient = new();

    // API-ключ для доступа к OMDb API.
    private const string ApiKey = "89f870b5";

    /// <summary>
    /// Получает информацию о фильмах по списку объектов Film.
    /// </summary>
    /// <param name="filmsForUpdating">Список объектов Film, для которых нужно получить информацию.</param>
    /// <returns>Список объектов FilmDto с информацией о фильмах.</returns>
    public static async Task<List<FilmDto>> GetFilms(List<Film> filmsForUpdating)
    {
        // Создаем список задач для асинхронного выполнения запросов к API.
        var tasks = filmsForUpdating.Select(GetFilm).ToList();

        // Ожидаем завершения всех задач.
        var films = await Task.WhenAll(tasks);

        // Возвращаем список фильмов. Если какой-то фильм не найден, он будет null.
        return films.ToList();
    }

    /// <summary>
    /// Получает информацию о фильме из OMDb API по названию и году выпуска.
    /// </summary>
    /// <param name="filmForUpdating">Объект Film, содержащий название и год выпуска.</param>
    /// <returns>Объект FilmDto с информацией о фильме.</returns>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если фильм не найден.</exception>
    public static async Task<FilmDto> GetFilm(Film filmForUpdating)
    {
        // Экранируем название фильма и год выпуска для использования в URL.
        var title = Uri.EscapeDataString(filmForUpdating.Name);
        var releaseYear = Uri.EscapeDataString($"{filmForUpdating.ReleaseYear}");

        // Формируем URL для запроса к OMDb API.
        var apiUrl = $"http://www.omdbapi.com/?apikey={ApiKey}&t={title}&y={releaseYear}";

        // Выполняем запрос к API.
        var response = await HttpClient.GetStringAsync(apiUrl);

        // Десериализуем JSON-ответ в объект FilmDto.
        var film = JsonSerializer.Deserialize<FilmDto>(response);

        // Проверяем, что фильм найден и его название не пустое.
        if (film != null && !string.IsNullOrEmpty(film.Title))
        {
            return film;
        }

        // Если фильм не найден, выбрасываем исключение.
        throw new KeyNotFoundException("Фильм не найден в OMDb.");
    }
}