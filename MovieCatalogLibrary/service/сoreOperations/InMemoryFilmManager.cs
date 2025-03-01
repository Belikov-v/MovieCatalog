using MovieCatalogLibrary.model;
using MovieCatalogLibrary.service.integrations;
using MovieCatalogLibrary.service.parsers;

namespace MovieCatalogLibrary.service.сoreOperations;

/// <summary>
/// Реализация интерфейса IFilmManager для управления фильмами в памяти.
/// </summary>
public class InMemoryFilmManager : IFilmManager
{
    // Словарь для хранения фильмов, где ключ — Id фильма, а значение — объект Film.
    private readonly Dictionary<int, Film> _films = new();

    // Множество для хранения Id пользовательских фильмов.
    private readonly HashSet<int> _customFilmIds = [];

    // Счетчик для генерации уникальных Id.
    private int _index;

    /// <summary>
    /// Инициализирует новый экземпляр класса InMemoryFilmManager.
    /// </summary>
    public InMemoryFilmManager()
    {
        _index = _films.Count;
    }

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
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="film"/> равен null.</exception>
    private void AddFilm(Film film)
    {
        // Проверка на null.
        if (film == null)
            throw new ArgumentNullException(nameof(film), "Фильм не может быть null.");

        // Присваиваем фильму уникальный Id.
        film.Id = _index;
        // Добавляем фильм в словарь.
        _films.Add(_index++, film);
    }

    /// <summary>
    /// Добавляет новый пользовательский фильм.
    /// </summary>
    /// <param name="film">Фильм для добавления.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="film"/> равен null.</exception>
    public void AddCustomFilm(Film film)
    {
        // Добавляем фильм в список пользовательских фильмов.
        _customFilmIds.Add(_index);
        AddFilm(film);
    }

    /// <summary>
    /// Добавляет существующий фильм (например, из внешнего источника).
    /// </summary>
    /// <param name="film">Фильм для добавления.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="film"/> равен null.</exception>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если фильм не найден.</exception>
    public void AddExistingFilm(Film film)
    {
        // Получаем данные о фильме из OMDb API.
        FilmDto filmDto = OmdbIntegration.GetFilm(film).GetAwaiter().GetResult();
        // Преобразуем FilmDto в Film и добавляем его.
        AddFilm(FilmConverter.ConvertFilmDtoToFilm(filmDto));
    }

    /// <summary>
    /// Редактирует существующий фильм.
    /// </summary>
    /// <param name="film">Фильм с обновленными данными.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="film"/> равен null.</exception>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если фильм с указанным Id не найден.</exception>
    public void EditFilm(Film film)
    {
        // Проверка на null.
        if (film == null)
            throw new ArgumentNullException(nameof(film), "Фильм не может быть null.");

        // Проверяем, существует ли фильм с указанным Id.
        if (!IsIdExist(film.Id))
            throw new KeyNotFoundException("Фильм с указанным Id не найден.");

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
        if (!IsIdExist(idFilm))
            throw new KeyNotFoundException("Фильм с указанным Id не найден.");

        // Удаляем фильм из словаря.
        _films.Remove(idFilm);
        _customFilmIds.Remove(idFilm);
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
        if (!IsIdExist(id))
            throw new KeyNotFoundException("Фильм с указанным Id не найден.");

        // Возвращаем фильм из словаря.
        return _films[id];
    }

    /// <summary>
    /// Возвращает список рекомендованных фильмов на основе указанных жанров.
    /// </summary>
    /// <param name="genres">Список жанров для рекомендаций.</param>
    /// <returns>Список рекомендованных фильмов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="genres"/> равен null.</exception>
    public List<Film> GetRecommendedFilms(List<GenreOfFilm> genres)
    {
        // Проверка на null.
        if (genres == null)
            throw new ArgumentNullException(nameof(genres), "Список жанров не может быть null.");

        List<Film> films = new();

        // Фильтруем фильмы по жанрам и рейтингу.
        foreach (var idFilm in _films.Keys)
        {
            if (genres.Contains(_films[idFilm].Genre) && _films[idFilm].Rating > 5)
            {
                films.Add(_films[idFilm]);
            }

            // Ограничиваем количество рекомендаций до 10.
            if (films.Count == 10)
            {
                return films;
            }
        }

        return films;
    }

    /// <summary>
    /// Обновляет данные о фильмах (например, загружает новые данные из внешнего источника).
    /// </summary>
    public void UpdateData()
    {
        // Собираем фильмы для обновления.
        List<Film> filmsForUpdating = new();
        foreach (var idFilm in _films.Keys)
        {
            if (_customFilmIds.Contains(idFilm)) continue; // Пропускаем пользовательские фильмы.
            Film film = _films[idFilm];
            _films.Remove(idFilm);
            filmsForUpdating.Add(film);
        }

        // Если нет фильмов для обновления, завершаем метод.
        if (filmsForUpdating.Count == 0)
        {
            return;
        }

        // Получаем обновленные данные из OMDb API.
        var updatedFilms = OmdbIntegration.GetFilms(filmsForUpdating).GetAwaiter().GetResult();

        // Очищаем словарь и обновляем данные.
        _index = _films.Count;
        _films.Clear();
        foreach (var film in updatedFilms.Select(updatedFilm => FilmConverter.ConvertFilmDtoToFilm(updatedFilm)))
        {
            AddFilm(film);
        }
    }
}