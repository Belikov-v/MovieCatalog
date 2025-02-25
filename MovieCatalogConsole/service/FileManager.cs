using System.Text.Json;
using MovieCatalogLibrary.model;
using MovieCatalogLibrary.service;

namespace MovieCatalogConsole.service;

/// <summary>
/// Управляет чтением и записью данных в файл.
/// </summary>
public static class FileManager
{
    /// <summary>
    /// Читает данные из файла и возвращает коллекцию фильмов.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <returns>Коллекция фильмов.</returns>
    /// <exception cref="FileNotFoundException">Выбрасывается, если файл не найден.</exception>
    /// <exception cref="IOException">Выбрасывается при ошибках ввода-вывода.</exception>
    /// <exception cref="JsonException">Выбрасывается при ошибках парсинга JSON.</exception>
    /// <exception cref="Exception">Выбрасывается при других неожиданных ошибках.</exception>
    public static IEnumerable<Film> ReadFile(string path)
    {
        using StreamReader reader = new StreamReader(path);
        string data = reader.ReadToEnd();
        if (string.IsNullOrWhiteSpace(data))
        {
            return new List<Film>();
        }

        IEnumerable<Film> films = JsonParser.GetFilmsFromJson(data);
        Console.WriteLine("Данные загружены.");
        return films;
    }

    /// <summary>
    /// Записывает данные в файл.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="data">Данные для записи.</param>
    /// <exception cref="FileNotFoundException">Выбрасывается, если файл не найден.</exception>
    /// <exception cref="IOException">Выбрасывается при ошибках ввода-вывода.</exception>
    /// <exception cref="UnauthorizedAccessException">Выбрасывается, если нет доступа к файлу.</exception>
    /// <exception cref="Exception">Выбрасывается при других неожиданных ошибках.</exception>
    public static void WriteDateInFile(string path, string data)
    {
        using StreamWriter writer = new StreamWriter(path);
        writer.Write(data);
    }

    /// <summary>
    /// Запрашивает у пользователя путь к файлу и проверяет его корректность.
    /// </summary>
    /// <returns>Корректный путь к файлу.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если путь содержит недопустимые символы.</exception>
    /// <exception cref="NotSupportedException">Выбрасывается, если путь не поддерживается.</exception>
    /// <exception cref="PathTooLongException">Выбрасывается, если путь слишком длинный.</exception>
    /// <exception cref="FileNotFoundException">Выбрасывается, если файл не найден.</exception>
    public static string GetPathToFile()
    {
        while (true)
        {
            Console.WriteLine("Введите полный путь к файлу.");
            string input = Console.ReadLine() ?? String.Empty;

            try
            {
                PathValidator.ValidatePath(input);
                string path = PathValidator.NormalizePath(input);
                PathValidator.CheckFileExists(path);
                return path;
            }
            catch (Exception e) when (e is ArgumentException or NotSupportedException or PathTooLongException or
                                          FileNotFoundException)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}