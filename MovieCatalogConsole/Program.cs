using System.Text.Json;
using MovieCatalogConsole.service;
using MovieCatalogLibrary.model;
using MovieCatalogLibrary.service.parsers;
using Spectre.Console;

/*
 * Беликов Владимир
 * БПИ249-2
 * Вариант: 8 (часть B)
 */
namespace MovieCatalogConsole;

/// <summary>
/// Класс с точкой входа в приложение.
/// </summary>
public static class Program
{
    private static string _path = string.Empty; // Путь к файлу с данными.
    private static Timer? _timer; // Таймер для автоматического обновления данных.

    // Словарь пунктов меню с описанием и действиями.
    private static readonly Dictionary<string, (string Description, Action Action)> MenuItems = new()
    {
        { "1", ("Просмотр всех фильмов.", FilmOperator.DisplayAllFilms) },
        { "2", ("Добавление собственного фильма.", FilmOperator.AddFilm) },
        { "3", ("Редактирование рейтинга фильма.", FilmOperator.EditRatingFilm) },
        { "4", ("Удаление фильма.", FilmOperator.DeleteFilm) },
        { "5", ("Представить в виде таблицы список фильмов.", FilmOperator.DisplayTable) },
        { "6", ("Диаграмма распределения фильмов по жанрам и рейтингам.", FilmOperator.DisplayСhart) },
        { "7", ("Визуализация обложек фильмов.", FilmOperator.DisplayCarousel) },
        { "8", ("Просмотр фильма.", FilmOperator.GetFilm) },
        { "9", ("Рекомендации.", FilmOperator.DisplayRecommendations) },
        { "10", ("Обновить данные.", () => FilmOperator.UpdateData(null)) },
        { "11", ("Сформировать график распределения рейтингов по жанрам в файл.", FilmOperator.DisplayRatingByGenre) },
        { "12", ("Сформировать график распределения рейтингов фильмов в файл.", FilmOperator.DisplayRatingHistogram) },
        { "13", ("Добавление фильма по названию.", FilmOperator.AddExistingFilm) },
        { "14", ("Выход.", Exit) }
    };

    /// <summary>
    /// Точка входа в приложение.
    /// </summary>
    public static void Main()
    {
        Load(); // Загрузка данных из файла.
        _timer = new Timer(FilmOperator.UpdateData, null, 0, 300000); // Запуск таймера для обновления данных.
        ShowMenu(); // Отображение главного меню.
    }

    /// <summary>
    /// Отображает главное меню и обрабатывает выбор пользователя.
    /// </summary>
    private static void ShowMenu()
    {
        while (true)
        {
            Console.Clear();

            // Создаем список пунктов меню для выбора.
            var menuOptions = new List<string>();
            foreach (var item in MenuItems)
            {
                menuOptions.Add($"{item.Key}. {item.Value.Description}");
            }

            // Отображаем интерактивное меню.
            string selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Выберите пункт меню:")
                    .PageSize(14)
                    .AddChoices(menuOptions)
            );

            // Извлекаем ключ выбранного пункта меню.
            string selectedKey = selectedOption.Split('.')[0];

            if (MenuItems.TryGetValue(selectedKey, out var menuItem))
            {
                Console.Clear();
                menuItem.Action.Invoke(); // Выполняем выбранное действие.
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Неверный выбор.[/]");
            }

            AnsiConsole.MarkupLine("\n[green]Нажмите любую клавишу, чтобы вернуться в меню...[/]");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Загружает данные о фильмах из файла.
    /// </summary>
    private static void Load()
    {
        while (true)
        {
            _path = FileDataProvider.GetPathToFile(); // Получаем путь к файлу.
            try
            {
                IEnumerable<Film> films = FileDataProvider.ReadFile(_path); // Чтение данных из файла.
                foreach (var film in films)
                {
                    FilmOperator.AddFilmToStorage(film); // Добавление фильмов в хранилище.
                }

                return; // Выход из цикла после успешной загрузки.
            }
            catch (FileNotFoundException)
            {
                AnsiConsole.MarkupLine("[red]Ошибка: Файл не найден.[/]");
            }
            catch (IOException)
            {
                AnsiConsole.MarkupLine("[red]Ошибка ввода-вывода.[/]");
            }
            catch (JsonException)
            {
                AnsiConsole.MarkupLine("[red]Ошибка при десериализации JSON.[/]");
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]{e.Message} Попробуйте загрузить в другой файл.[/]");
            }
        }
    }

    /// <summary>
    /// Завершает работу приложения с сохранением данных.
    /// </summary>
    private static void Exit()
    {
        var films = FilmOperator.GetAllFilmsFromStorage(); // Получаем все фильмы из хранилища.
        while (true)
        {
            try
            {
                string json = JsonParser.FilmsToJson(films); // Сериализуем фильмы в JSON.
                FileDataProvider.WriteDateInFile(_path, json); // Записываем данные в файл.
                Environment.Exit(0); // Завершаем работу приложения.
                return;
            }
            catch (NotSupportedException)
            {
                AnsiConsole.MarkupLine("[red]Невозможно десериализовать JSON: тип не поддерживается.[/]");
            }
            catch (ArgumentException)
            {
                AnsiConsole.MarkupLine("[red]Ошибка: Неверные символы.[/]");
            }
            catch (UnauthorizedAccessException)
            {
                AnsiConsole.MarkupLine("[red]Ошибка: Нет доступа к файлу.[/]");
            }
            catch (FileNotFoundException)
            {
                AnsiConsole.MarkupLine("[red]Ошибка: Файл не найден.[/]");
            }
            catch (IOException)
            {
                AnsiConsole.MarkupLine("[red]Ошибка ввода-вывода.[/]");
            }
            catch (JsonException)
            {
                AnsiConsole.MarkupLine("[red]Ошибка при десериализации JSON.[/]");
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]{e.Message} Попробуйте загрузить в другой файл.[/]");
            }
        }
    }
}