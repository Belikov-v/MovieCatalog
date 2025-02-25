using MovieCatalogLibrary.model;

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
        Film film = new Film();
        SetName(film);
        SetGenre(film);
        SetReleaseYear(film);
        SetRating(film);
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
            string name = Console.ReadLine() ?? string.Empty; // Чтение ввода пользователя.
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Введенное имя некорректно.");
                film.Name = name;
                break; // Выход из цикла, если ввод корректен.
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Ошибка: {e.Message} (Детали: {e.Message})");
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
    public static void SetGenre(Film film)
    {
        Console.WriteLine("Выберите жанр фильма:");
        var genres = Enum.GetValues(typeof(GenreOfFilm)); // Получаем все значения перечисления GenreOfFilm.

        // Выводим список жанров с номерами для выбора.
        for (int i = 0; i < genres.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {genres.GetValue(i)}");
        }

        while (true) // Бесконечный цикл для повторного запроса в случае ошибки.
        {
            Console.Write("Введите номер жанра: ");
            string input = Console.ReadLine() ?? string.Empty; // Чтение ввода пользователя.

            try
            {
                int choice = int.Parse(input); // Преобразование строки в целое число.
                Object genre = genres.GetValue(choice - 1) ?? throw new ArgumentException(); // Получаем выбранный жанр.
                film.Genre = (GenreOfFilm)genre;
                break; // Выход из цикла, если ввод корректен.
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Ошибка: Некорректный аргумент. (Детали: {e.Message})");
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine($"Ошибка: Введен недопустимый индекс. (Детали: {e.Message})");
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
}