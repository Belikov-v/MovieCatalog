namespace MovieCatalogConsole.service;

/// <summary>
/// Предоставляет методы для валидации, нормализации и проверки существования путей к файлам.
/// </summary>
public static class PathValidator
{
    /// <summary>
    /// Проверяет, что путь не является пустым, null и не содержит недопустимых символов.
    /// </summary>
    /// <param name="path">Путь к файлу, который необходимо проверить.</param>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если путь пустой, равен null или содержит недопустимые символы.
    /// </exception>
    public static void ValidatePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Путь к файлу не может быть пустым или null.", nameof(path));
        }

        if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
        {
            throw new ArgumentException("Путь содержит недопустимые символы.", nameof(path));
        }
    }

    /// <summary>
    /// Нормализует путь, преобразуя его в абсолютный и убирая лишние элементы (например, ".", "..").
    /// </summary>
    /// <param name="path">Путь к файлу, который необходимо нормализовать.</param>
    /// <returns>Абсолютный путь к файлу.</returns>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если путь имеет некорректный формат.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Выбрасывается, если путь содержит недопустимые символы или формат.
    /// </exception>
    /// <exception cref="PathTooLongException">
    /// Выбрасывается, если путь превышает максимально допустимую длину.
    /// </exception>
    public static string NormalizePath(string path)
    {
        try
        {
            return Path.GetFullPath(path);
        }
        catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
        {
            throw new ArgumentException("Некорректный путь.", nameof(path), ex);
        }
    }

    /// <summary>
    /// Проверяет, существует ли файл по указанному пути.
    /// </summary>
    /// <param name="path">Путь к файлу, который необходимо проверить.</param>
    /// <exception cref="FileNotFoundException">
    /// Выбрасывается, если файл по указанному пути не существует.
    /// </exception>
    public static void CheckFileExists(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Файл не найден.", path);
        }
    }
}