using MovieCatalogLibrary.model;
using Spectre.Console;

namespace MovieCatalogLibrary.service.visualizations;

/// <summary>
/// Класс для отображения таблицы фильмов с возможностью прокрутки, фильтрации, сортировки и удаления операций.
/// </summary>
public class FilmTableView
{
    private int _currentOffset; // Текущая позиция прокрутки.
    private const int PageSize = 5; // Количество фильмов на странице.

    private readonly FilmTableManager _filmTableManager = new();

    /// <summary>
    /// Отображает таблицу фильмов и обрабатывает ввод пользователя.
    /// </summary>
    /// <param name="films">Список фильмов для отображения.</param>
    public void Render(List<Film> films)
    {
        // Проверка на null.
        if (films == null)
            throw new ArgumentNullException(nameof(films), "Список фильмов не может быть null.");

        while (true)
        {
            Console.Clear();
            DisplayInstructions();

            // Применяем все операции к списку фильмов.
            List<Film> filteredFilms = _filmTableManager.ApplyOperations(films);

            // Отображаем таблицу с фильмами.
            DisplayFilmTable(filteredFilms);

            // Обрабатываем ввод пользователя.
            if (!HandleUserInput(filteredFilms))
            {
                break; // Выход, если пользователь нажал Esc.
            }
        }
    }

    /// <summary>
    /// Отображает таблицу с фильмами.
    /// </summary>
    /// <param name="films">Список фильмов для отображения.</param>
    private void DisplayFilmTable(List<Film> films)
    {
        // Отображаем применённые операции.
        DisplayAppliedOperations();

        // Создаём таблицу и добавляем колонки.
        var table = CreateTable();

        // Получаем фильмы для текущей страницы.
        var visibleFilms = GetVisibleFilms(films);

        // Добавляем строки в таблицу.
        AddFilmsToTable(table, visibleFilms);

        // Отображаем таблицу.
        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Отображает применённые операции.
    /// </summary>
    private void DisplayAppliedOperations()
    {
        var operations = _filmTableManager.GetOperations();
        if (operations.Count == 0) return;
        var operationsString = string.Join(" ", operations.Select(op => $"|{op.Name}|"));
        AnsiConsole.MarkupLine($"Применённые операции: {operationsString}");
    }

    /// <summary>
    /// Создаёт таблицу для отображения фильмов.
    /// </summary>
    /// <returns>Таблица с колонками.</returns>
    private Table CreateTable()
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Название");
        table.AddColumn("Жанр");
        table.AddColumn("Рейтинг");
        return table;
    }

    /// <summary>
    /// Получает фильмы для текущей страницы.
    /// </summary>
    /// <param name="films">Список фильмов.</param>
    /// <returns>Список фильмов для отображения.</returns>
    private List<Film> GetVisibleFilms(List<Film> films)
    {
        return films.Skip(_currentOffset).Take(PageSize).ToList();
    }

    /// <summary>
    /// Добавляет фильмы в таблицу.
    /// </summary>
    /// <param name="table">Таблица для добавления строк.</param>
    /// <param name="films">Список фильмов.</param>
    private void AddFilmsToTable(Table table, List<Film> films)
    {
        foreach (var film in films)
        {
            table.AddRow(film.Id.ToString(), film.Name, film.Genre.ToString(), film.Rating.ToString());
        }
    }

    /// <summary>
    /// Обрабатывает ввод пользователя.
    /// </summary>
    /// <param name="films">Список фильмов.</param>
    /// <returns>Возвращает false, если пользователь нажал Esc, иначе true.</returns>
    private bool HandleUserInput(List<Film> films)
    {
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                ScrollUp();
                break;
            case ConsoleKey.DownArrow:
                ScrollDown(films.Count);
                break;
            case ConsoleKey.F:
                ApplyFilterOperation(films);
                break;
            case ConsoleKey.S:
                ApplySortingOperation();
                break;
            case ConsoleKey.D:
                RemoveOperation();
                break;
            case ConsoleKey.Escape:
                return false;
        }

        return true;
    }

    /// <summary>
    /// Прокручивает таблицу вверх.
    /// </summary>
    private void ScrollUp()
    {
        if (_currentOffset > 0)
        {
            _currentOffset--;
        }
    }

    /// <summary>
    /// Прокручивает таблицу вниз.
    /// </summary>
    /// <param name="totalFilms">Общее количество фильмов.</param>
    private void ScrollDown(int totalFilms)
    {
        if (_currentOffset < totalFilms - PageSize)
        {
            _currentOffset++;
        }
    }

    /// <summary>
    /// Применяет операцию фильтрации.
    /// </summary>
    /// <param name="films">Список фильмов.</param>
    private void ApplyFilterOperation(List<Film> films)
    {
        try
        {
            _filmTableManager.AddFilterOperation(films);
            ResetOffset();
        }
        catch (Exception e)
        {
            DisplayErrorMessage(e.Message);
        }
    }

    /// <summary>
    /// Применяет операцию сортировки.
    /// </summary>
    private void ApplySortingOperation()
    {
        _filmTableManager.AddSortingOperation();
        ResetOffset();
    }

    /// <summary>
    /// Удаляет операцию.
    /// </summary>
    private void RemoveOperation()
    {
        _filmTableManager.RemoveOperation();
        ResetOffset();
    }

    /// <summary>
    /// Сбрасывает позицию прокрутки.
    /// </summary>
    private void ResetOffset()
    {
        _currentOffset = 0;
    }

    /// <summary>
    /// Отображает сообщение об ошибке.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    private void DisplayErrorMessage(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message}[/]");
        AnsiConsole.MarkupLine("Нажмите любую клавишу чтобы продолжить.");
        Console.ReadKey();
    }

    /// <summary>
    /// Отображает инструкции для пользователя.
    /// </summary>
    private static void DisplayInstructions()
    {
        AnsiConsole.MarkupLine(
            "Используйте ↑ ↓ для прокрутки, F - фильтр, S - сортировка, D - удалить операцию, Esc - выход.");
    }
}