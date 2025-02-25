using MovieCatalogLibrary.model;

namespace MovieCatalogLibrary.service;

/// <summary>
/// Интерфейс для управления фильмами.
/// </summary>
public interface IFilmManager
{
    /// <summary>
    /// Возвращает все фильмы.
    /// </summary>
    /// <returns>Коллекция фильмов.</returns>
    IEnumerable<Film> GetAllFilms();

    /// <summary>
    /// Добавляет новый фильм.
    /// </summary>
    /// <param name="film">Фильм для добавления.</param>
    void AddFilm(Film film);

    /// <summary>
    /// Редактирует существующий фильм.
    /// </summary>
    /// <param name="film">Фильм с обновленными данными.</param>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если фильм с указанным Id не найден.</exception>
    void EditFilm(Film film);

    /// <summary>
    /// Удаляет фильм по Id.
    /// </summary>
    /// <param name="idFilm">Id фильма для удаления.</param>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если фильм с указанным Id не найден.</exception>
    void DeleteFilm(int idFilm);

    /// <summary>
    /// Проверяет, существует ли фильм с указанным Id.
    /// </summary>
    /// <param name="id">Id фильма для проверки.</param>
    /// <returns>True, если фильм существует, иначе False.</returns>
    bool IsIdExist(int id);

    /// <summary>
    /// Возвращает фильм по Id.
    /// </summary>
    /// <param name="id">Id фильма.</param>
    /// <returns>Найденный фильм.</returns>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если фильм с указанным Id не найден.</exception>
    Film GetFilm(int id);
}