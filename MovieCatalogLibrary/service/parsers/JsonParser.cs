using System.Text.Json;
using MovieCatalogLibrary.model;

namespace MovieCatalogLibrary.service.parsers;

/// <summary>
/// Предоставляет методы для сериализации и десериализации фильмов в формате JSON.
/// </summary>
public static class JsonParser
{
    // Кэшируем экземпляр JsonSerializerOptions для повторного использования.
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true // Опция для красивого форматирования JSON.
    };

    /// <summary>
    /// Сериализует список фильмов в JSON-строку.
    /// </summary>
    /// <param name="films">Список фильмов для сериализации.</param>
    /// <returns>JSON-строка, представляющая список фильмов.</returns>
    /// <exception cref="NotSupportedException">Выбрасывается, если сериализация не поддерживается для данного типа.</exception>
    public static string FilmsToJson(List<Film> films)
    {
        return JsonSerializer.Serialize(films, JsonOptions);
    }

    /// <summary>
    /// Десериализует JSON-строку в список фильмов.
    /// </summary>
    /// <param name="data">JSON-строка, содержащая данные о фильмах.</param>
    /// <returns>Список фильмов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если входная строка равна null.</exception>
    /// <exception cref="JsonException">Выбрасывается, если произошла ошибка при десериализации.</exception>
    /// <exception cref="NotSupportedException">Выбрасывается, если десериализация не поддерживается для данного типа.</exception>
    public static List<Film> GetFilmsFromJson(string data)
    {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentNullException(nameof(data), "Входная строка не может быть null или пустой.");

        return JsonSerializer.Deserialize<List<Film>>(data, JsonOptions) ?? [];
    }
}