namespace MovieCatalogLibrary.model;

/// <summary>
/// Класс, представляющий операцию, которая может быть применена к списку фильмов.
/// Используется для фильтрации, сортировки или других операций над списком фильмов.
/// </summary>
public class Operation
{
    private readonly string _name = string.Empty;
    private readonly Func<List<Film>, List<Film>> _apply = films => films;

    /// <summary>
    /// Название операции.
    /// </summary>
    public string Name
    {
        get => _name;
        init => _name = value ??
                        throw new ArgumentNullException(nameof(value), "Название операции не может быть null.");
    }

    /// <summary>
    /// Функция, которая применяет операцию к списку фильмов.
    /// Принимает список фильмов и возвращает новый список после применения операции.
    /// </summary>
    public Func<List<Film>, List<Film>> Apply
    {
        get => _apply;
        init => _apply = value ??
                         throw new ArgumentNullException(nameof(value), "Функция операции не может быть null.");
    }
}