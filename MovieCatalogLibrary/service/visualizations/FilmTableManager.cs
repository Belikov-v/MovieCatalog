using MovieCatalogLibrary.model;
using MovieCatalogLibrary.service.dataProcessing;
using Spectre.Console;

namespace MovieCatalogLibrary.service.visualizations;

/// <summary>
/// Класс для управления операциями фильтрации, сортировки и отображения таблицы фильмов.
/// </summary>
public class FilmTableManager
{
    private readonly OperationManager _operationManager = new();
    private readonly SortManager _sortManager = new();

    /// <summary>
    /// Применяет все операции к списку фильмов.
    /// </summary>
    /// <param name="films">Список фильмов.</param>
    /// <returns>Список фильмов после применения операций.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="films"/> равен null.</exception>
    public List<Film> ApplyOperations(List<Film> films)
    {
        // Проверка на null.
        if (films == null)
            throw new ArgumentNullException(nameof(films), "Список фильмов не может быть null.");

        return _operationManager.ApplyOperations(films);
    }

    /// <summary>
    /// Добавляет операцию фильтрации в список операций.
    /// </summary>
    /// <param name="films">Список фильмов для фильтрации.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="films"/> равен null.</exception>
    public void AddFilterOperation(List<Film> films)
    {
        // Проверка на null.
        if (films == null)
            throw new ArgumentNullException(nameof(films), "Список фильмов не может быть null.");

        // Определяем доступные варианты фильтрации.
        var filterOptions = new Dictionary<string, Func<Operation>>
        {
            { "По жанру", () => FilterManager.CreateGenreFilter(films) },
            { "По рейтингу", () => FilterManager.CreateRatingFilter(films) }
        };

        // Запрашиваем у пользователя выбор типа фильтра.
        string selectedFilter = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Выберите тип фильтра:")
                .AddChoices(filterOptions.Keys)
        );

        // Создаем и добавляем операцию фильтрации.
        var operation = filterOptions[selectedFilter]();
        _operationManager.AddOperation(operation);
    }

    /// <summary>
    /// Добавляет операцию сортировки в список операций.
    /// </summary>
    public void AddSortingOperation()
    {
        // Создаем и добавляем операцию сортировки.
        var operation = _sortManager.CreateSortingOperation();
        _operationManager.AddOperation(operation);
    }

    /// <summary>
    /// Удаляет операцию из списка операций.
    /// </summary>
    public void RemoveOperation()
    {
        var operations = _operationManager.GetOperations();

        // Проверка на пустой список операций.
        if (operations.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Нет операций для удаления.[/]");
            Console.ReadKey();
            return;
        }

        // Формируем список названий операций для выбора.
        var operationNames = operations.Select((op, index) => $"{index + 1}. {op.Name}").ToList();
        operationNames.Add("Отмена");

        // Запрашиваем у пользователя выбор операции для удаления.
        var selectedOperation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Выберите операцию для удаления:")
                .AddChoices(operationNames)
        );

        // Если выбрана "Отмена", завершаем метод.
        if (selectedOperation == "Отмена") return;

        // Удаляем выбранную операцию.
        var index = operationNames.IndexOf(selectedOperation);
        _operationManager.RemoveOperation(index);
    }

    /// <summary>
    /// Возвращает список всех операций.
    /// </summary>
    /// <returns>Список операций.</returns>
    public List<Operation> GetOperations()
    {
        return _operationManager.GetOperations();
    }
}