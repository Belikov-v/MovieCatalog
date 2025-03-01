using MovieCatalogLibrary.model;
using Spectre.Console;
using System.Text;

namespace MovieCatalogConsole.service;

/// <summary>
/// Класс FilmBuilder предоставляет методы для пошагового создания объекта Film.
/// Он взаимодействует с пользователем через консоль, запрашивая данные для каждого свойства фильма.
/// </summary>
public static class FilmBuilder
{
    /// <summary>
    /// Основной метод для создания объекта Film.
    /// Запрашивает у пользователя данные для каждого свойства фильма и возвращает готовый объект.
    /// </summary>
    public static Film BuildFilm()
    {
        var film = new Film();
        SetName(film);
        SetGenre(film);
        SetReleaseYear(film);
        SetRating(film);
        SetDirector(film);
        SetPlot(film);
        SetActors(film); // Добавляем актеров.

        return film;
    }

    /// <summary>
    /// Метод для установки названия фильма.
    /// Запрашивает у пользователя ввод и проверяет, что название не пустое.
    /// </summary>
    public static void SetName(Film film)
    {
        while (true) // Бесконечный цикл для повторного запроса в случае ошибки.
        {
            Console.WriteLine("Введите название фильма.");
            var name = Console.ReadLine() ?? string.Empty; // Чтение ввода пользователя.
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Введенное имя некорректно.");
                film.Name = name;
                break; // Выход из цикла, если ввод корректен.
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
        }
    }

    /// <summary>
    /// Метод для установки имени режиссера фильма.
    /// Запрашивает у пользователя ввод и проверяет, что имя не пустое.
    /// </summary>
    private static void SetDirector(Film film)
    {
        while (true) // Бесконечный цикл для повторного запроса в случае ошибки.
        {
            Console.WriteLine("Введите имя режиссера фильма.");
            var name = Console.ReadLine() ?? string.Empty; // Чтение ввода пользователя.
            try
            {
                if (string.IsNullOrWhiteSpace(name) || int.TryParse(name, out _) || double.TryParse(name, out _))
                    throw new ArgumentException("Введенное имя некорректно.");
                film.Director = name;
                break; // Выход из цикла, если ввод корректен.
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
        }
    }

    /// <summary>
    /// Метод для установки сюжета фильма.
    /// Запрашивает у пользователя ввод и проверяет, что сюжет не пустой.
    /// </summary>
    private static void SetPlot(Film film)
    {
        while (true) // Бесконечный цикл для повторного запроса в случае ошибки.
        {
            Console.WriteLine("Введите сюжет фильма.");
            var name = Console.ReadLine() ?? string.Empty; // Чтение ввода пользователя.
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Введенные данные некорректны.");
                film.Plot = name;
                break; // Выход из цикла, если ввод корректен.
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
        }
    }

    /// <summary>
    /// Метод для установки года выпуска фильма.
    /// Запрашивает у пользователя ввод и проверяет, что дата корректна.
    /// </summary>
    public static void SetReleaseYear(Film film)
    {
        while (true) // Бесконечный цикл для повторного запроса в случае ошибки.
        {
            Console.WriteLine("Введите год выпуска фильма.");
            string input = Console.ReadLine() ?? string.Empty; // Чтение ввода пользователя.
            try
            {
                int date = int.Parse(input); // Преобразование строки в тип int.
                film.ReleaseYear = date;
                break; // Выход из цикла, если ввод корректен.
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine($"Ошибка: Недопустимый диапазон даты. (Детали: {e.Message})");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Ошибка: Входное значение не может быть null. (Детали: {e.Message})");
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Ошибка: Некорректный формат ввода. (Детали: {e.Message})");
            }
        }
    }

    /// <summary>
    /// Метод для установки рейтинга фильма.
    /// Запрашивает у пользователя ввод и проверяет, что рейтинг является целым числом.
    /// </summary>
    public static void SetRating(Film film)
    {
        while (true) // Бесконечный цикл для повторного запроса в случае ошибки.
        {
            Console.WriteLine("Введите рейтинг фильма.");
            string input = Console.ReadLine() ?? string.Empty; // Чтение ввода пользователя.
            try
            {
                int rating = int.Parse(input); // Преобразование строки в целое число.
                film.Rating = rating;
                break; // Выход из цикла, если ввод корректен.
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine($"Ошибка: Недопустимый диапазон значения. (Детали: {e.Message})");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Ошибка: Входное значение не может быть null. (Детали: {e.Message})");
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Ошибка: Некорректный формат ввода. (Детали: {e.Message})");
            }
            catch (OverflowException e)
            {
                Console.WriteLine($"Ошибка: Введенное значение выходит за допустимые пределы. (Детали: {e.Message})");
            }
        }
    }

    /// <summary>
    /// Метод для установки жанра фильма.
    /// Запрашивает у пользователя выбор жанра из списка и проверяет, что выбор корректен.
    /// </summary>
    private static void SetGenre(Film film)
    {
        // Получаем все значения перечисления GenreOfFilm
        var genres = Enum.GetValues(typeof(GenreOfFilm));

        // Создаем список жанров для выбора
        var genreOptions = genres.Cast<object>().Select((t, i) => $"{i + 1}. {genres.GetValue(i)}").ToList();

        // Отображаем интерактивное меню
        var selectedGenre = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Выберите жанр фильма:")
                .PageSize(10) // Указываем количество пунктов на экране
                .AddChoices(genreOptions)
        );

        // Извлекаем номер выбранного жанра
        var choice = int.Parse(selectedGenre.Split('.')[0]);

        // Устанавливаем выбранный жанр
        film.Genre = (GenreOfFilm)genres.GetValue(choice - 1)!;
    }

    /// <summary>
    /// Метод для добавления актеров в фильм.
    /// Запрашивает у пользователя список актеров и добавляет их в объект Film.
    /// </summary>
    private static void SetActors(Film film)
    {
        var actors = new List<string>();
        Console.WriteLine("Введите имена актеров (для завершения введите пустую строку):");

        while (true)
        {
            Console.Write("Актер: ");
            var actor = Console.ReadLine()?.Trim(); // Чтение ввода пользователя.

            // Если введена пустая строка, завершаем ввод.
            if (string.IsNullOrWhiteSpace(actor))
            {
                if (actors.Count == 0)
                {
                    Console.WriteLine("Должен быть указан хотя бы один актер.");
                    continue;
                }

                break;
            }

            // Добавляем актера в список.
            actors.Add(actor);
        }

        // Устанавливаем список актеров в объект Film.
        film.Actors = actors;
    }
}