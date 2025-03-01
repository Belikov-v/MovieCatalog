using MovieCatalogLibrary.model;

namespace MovieCatalogLibrary.service.сoreOperations;

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
    /// Добавляет новый фильм, созданный пользователем.
    /// </summary>
    /// <param name="film">Фильм для добавления.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="film"/> равен null.</exception>
    /// <exception cref="InvalidOperationException">Выбрасывается, если фильм с таким Id уже существует.</exception>
    void AddCustomFilm(Film film);

    /// <summary>
    /// Редактирует существующий фильм.
    /// </summary>
    /// <param name="film">Фильм с обновленными данными.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="film"/> равен null.</exception>
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

    /// <summary>
    /// Возвращает список рекомендованных фильмов на основе указанных жанров.
    /// </summary>
    /// <param name="genres">Список жанров для рекомендаций.</param>
    /// <returns>Список рекомендованных фильмов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="genres"/> равен null.</exception>
    List<Film> GetRecommendedFilms(List<GenreOfFilm> genres);

    /// <summary>
    /// Обновляет данные о фильмах (например, загружает новые данные из внешнего источника).
    /// </summary>
    void UpdateData();

    /// <summary>
    /// Добавляет существующий фильм (например, из внешнего источника).
    /// </summary>
    /// <param name="film">Фильм для добавления.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="film"/> равен null.</exception>
    /// <exception cref="InvalidOperationException">Выбрасывается, если фильм с таким Id уже существует.</exception>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если фильм не найден.</exception>
    void AddExistingFilm(Film film);
}