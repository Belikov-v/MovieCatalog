using MovieCatalogLibrary.model;

namespace MovieCatalogLibrary.service.dataProcessing;

/// <summary>
/// Класс для управления операциями над списком фильмов.
/// </summary>
public class OperationManager
{
    private readonly List<Operation> _operations = new();

    /// <summary>
    /// Добавляет операцию в список операций.
    /// </summary>
    /// <param name="operation">Операция для добавления.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="operation"/> равен null.</exception>
    public void AddOperation(Operation operation)
    {
        // Проверка на null.
        if (operation == null)
            throw new ArgumentNullException(nameof(operation), "Операция не может быть null.");

        _operations.Add(operation);
    }

    /// <summary>
    /// Удаляет операцию из списка операций по указанному индексу.
    /// </summary>
    /// <param name="index">Индекс операции для удаления.</param>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если <paramref name="index"/> выходит за пределы диапазона.</exception>
    public void RemoveOperation(int index)
    {
        // Проверка на корректность индекса.
        if (index < 0 || index >= _operations.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс выходит за пределы диапазона.");

        _operations.RemoveAt(index);
    }

    /// <summary>
    /// Возвращает список всех операций.
    /// </summary>
    /// <returns>Список операций.</returns>
    public List<Operation> GetOperations()
    {
        return _operations;
    }

    /// <summary>
    /// Применяет все операции к списку фильмов.
    /// </summary>
    /// <param name="films">Список фильмов для применения операций.</param>
    /// <returns>Список фильмов после применения операций.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="films"/> равен null.</exception>
    public List<Film> ApplyOperations(List<Film> films)
    {
        // Проверка на null.
        if (films == null)
            throw new ArgumentNullException(nameof(films), "Список фильмов не может быть null.");

        return _operations.Aggregate(films, (current, operation) => operation.Apply(current));
    }
}