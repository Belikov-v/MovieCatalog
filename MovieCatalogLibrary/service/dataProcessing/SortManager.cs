using MovieCatalogLibrary.model;
using Spectre.Console;

namespace MovieCatalogLibrary.service.dataProcessing;

/// <summary>
/// Класс для создания операций сортировки фильмов.
/// </summary>
public class SortManager
{
    /// <summary>
    /// Создает операцию сортировки фильмов.
    /// </summary>
    /// <returns>Операция сортировки.</returns>
    public Operation CreateSortingOperation()
    {
        // Определяем доступные варианты сортировки.
        var sortingOptions = new Dictionary<string, Func<List<Film>, List<Film>>>
        {
            { "Без сортировки.", films => films },
            { "По убыванию рейтинга.", films => films.OrderByDescending(f => f.Rating).ToList() },
            { "По возрастанию рейтинга.", films => films.OrderBy(f => f.Rating).ToList() }
        };

        // Запрашиваем у пользователя выбор способа сортировки.
        var selectedOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Выберите способ сортировки:")
                .AddChoices(sortingOptions.Keys)
        );

        // Возвращаем операцию сортировки.
        return new Operation
        {
            Name = selectedOption,
            Apply = sortingOptions[selectedOption]
        };
    }
}