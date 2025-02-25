using System.Text.Json;
using MovieCatalogLibrary.model;

namespace MovieCatalogLibrary.service;

/// <summary>
/// Предоставляет методы для сериализации и десериализации фильмов в формате JSON.
/// </summary>
public static class JsonParser
{
    /// <summary>
    /// Сериализует список фильмов в JSON-строку.
    /// </summary>
    /// <param name="films">Список фильмов для сериализации.</param>
    /// <returns>JSON-строка, представляющая список фильмов.</returns>
    /// <exception cref="NotSupportedException">Выбрасывается, если сериализация не поддерживается для данного типа.</exception>
    /// <exception cref="JsonException">Выбрасывается, если произошла ошибка при сериализации.</exception>
    public static string FilmsToJson(List<Film> films)
    {
        return JsonSerializer.Serialize(films, new JsonSerializerOptions { WriteIndented = true });
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
        return JsonSerializer.Deserialize<List<Film>>(data) ?? new List<Film>();
    }
}