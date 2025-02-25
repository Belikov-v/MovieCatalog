using MovieCatalogLibrary.model;

namespace MovieCatalogLibrary.service;

/// <summary>
/// Реализация интерфейса IFilmManager для управления фильмами в памяти.
/// </summary>
public class InMemoryFilmManager : IFilmManager
{
    // Словарь для хранения фильмов, где ключ — Id фильма, а значение — объект Film.
    private readonly Dictionary<int, Film> _films = new();

    // Счетчик для генерации уникальных Id.
    private int _index = 0;

    /// <summary>
    /// Возвращает все фильмы.
    /// </summary>
    /// <returns>Коллекция фильмов.</returns>
    public IEnumerable<Film> GetAllFilms()
    {
        return _films.Values;
    }

    /// <summary>
    /// Добавляет новый фильм.
    /// </summary>
    /// <param name="film">Фильм для добавления.</param>
    public void AddFilm(Film film)
    {
        // Присваиваем фильму уникальный Id.
        film.Id = _index;
        // Добавляем фильм в словарь.
        _films.Add(_index++, film);
    }

    /// <summary>
    /// Редактирует существующий фильм.
    /// </summary>
    /// <param name="film">Фильм с обновленными данными.</param>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если фильм с указанным Id не найден.</exception>
    public void EditFilm(Film film)
    {
        // Проверяем, существует ли фильм с указанным Id.
        if (!IsIdExist(film.Id)) throw new KeyNotFoundException("Фильм с указанным Id не найден.");
        // Обновляем фильм в словаре.
        _films[film.Id] = film;
    }

    /// <summary>
    /// Удаляет фильм по Id.
    /// </summary>
    /// <param name="idFilm">Id фильма для удаления.</param>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если фильм с указанным Id не найден.</exception>
    public void DeleteFilm(int idFilm)
    {
        // Проверяем, существует ли фильм с указанным Id.
        if (!IsIdExist(idFilm)) throw new KeyNotFoundException("Фильм с указанным Id не найден.");
        // Удаляем фильм из словаря.
        _films.Remove(idFilm);
    }

    /// <summary>
    /// Проверяет, существует ли фильм с указанным Id.
    /// </summary>
    /// <param name="id">Id фильма для проверки.</param>
    /// <returns>True, если фильм существует, иначе False.</returns>
    public bool IsIdExist(int id)
    {
        return _films.ContainsKey(id);
    }

    /// <summary>
    /// Возвращает фильм по Id.
    /// </summary>
    /// <param name="id">Id фильма.</param>
    /// <returns>Найденный фильм.</returns>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если фильм с указанным Id не найден.</exception>
    public Film GetFilm(int id)
    {
        // Проверяем, существует ли фильм с указанным Id.
        if (!IsIdExist(id)) throw new KeyNotFoundException("Фильм с указанным Id не найден.");
        // Возвращаем фильм из словаря.
        return _films[id];
    }
}