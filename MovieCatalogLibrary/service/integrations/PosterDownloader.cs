using MovieCatalogLibrary.exceptions;

namespace MovieCatalogLibrary.service.integrations;

/// <summary>
/// Класс для скачивания и сохранения обложек фильмов.
/// </summary>
public static class PosterDownloader
{
    private static readonly HttpClient HttpClient = new();
    private static readonly string PostersFolder = Path.Combine(Directory.GetCurrentDirectory(), "posters");

    /// <summary>
    /// Скачивает обложку фильма и сохраняет её в папку.
    /// </summary>
    /// <param name="imageUrl">URL обложки.</param>
    /// <returns>Путь к сохранённой обложке.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="imageUrl"/> равен null или пустой строке.</exception>
    /// <exception cref="PosterDownloadException">Выбрасывается, если не удалось скачать обложку.</exception>
    /// <exception cref="IOException">Выбрасывается, если произошла ошибка ввода-вывода при создании папки или сохранении файла.</exception>
    /// <exception cref="UnauthorizedAccessException">Выбрасывается, если нет доступа для создания папки или файла.</exception>
    /// <exception cref="PathTooLongException">Выбрасывается, если путь к файлу или папке слишком длинный.</exception>
    /// <exception cref="DirectoryNotFoundException">Выбрасывается, если указанная папка не существует и не может быть создана.</exception>
    /// <exception cref="HttpRequestException">Выбрасывается, если произошла ошибка при выполнении HTTP-запроса.</exception>
    /// <exception cref="TaskCanceledException">Выбрасывается, если операция скачивания была отменена.</exception>
    public static async Task<string> DownloadPosterAsync(string imageUrl)
    {
        // Проверяем входные данные.
        if (string.IsNullOrEmpty(imageUrl))
            throw new ArgumentNullException(nameof(imageUrl), "URL обложки не может быть пустым.");

        // Создаём папку для обложек, если её нет.
        if (!Directory.Exists(PostersFolder))
        {
            Directory.CreateDirectory(PostersFolder);
        }

        // Формируем путь для сохранения обложки.
        var filePath = Path.Combine(PostersFolder, $"{GetJpgFileCount(PostersFolder) + 1}.jpg");

        // Скачиваем изображение.
        var response = await HttpClient.GetAsync(imageUrl);
        if (response.IsSuccessStatusCode)
        {
            // Сохраняем изображение на диск.
            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await response.Content.CopyToAsync(fileStream);
        }
        else
        {
            // Если HTTP-запрос неуспешен, выбрасываем исключение.
            throw new PosterDownloadException("Не удалось скачать обложку.");
        }

        return filePath;
    }

    /// <summary>
    /// Возвращает количество JPG-файлов в указанной папке.
    /// </summary>
    /// <param name="folderPath">Путь к папке.</param>
    /// <returns>Количество JPG-файлов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="folderPath"/> равен null или пустой строке.</exception>
    /// <exception cref="DirectoryNotFoundException">Выбрасывается, если указанная папка не существует.</exception>
    /// <exception cref="UnauthorizedAccessException">Выбрасывается, если нет доступа к папке.</exception>
    /// <exception cref="IOException">Выбрасывается, если произошла ошибка ввода-вывода при доступе к папке.</exception>
    /// <exception cref="PathTooLongException">Выбрасывается, если путь к папке слишком длинный.</exception>
    private static int GetJpgFileCount(string folderPath)
    {
        // Проверяем входные данные.
        if (string.IsNullOrEmpty(folderPath))
            throw new ArgumentNullException(nameof(folderPath), "Путь к папке не может быть пустым.");

        if (!Directory.Exists(folderPath))
            throw new DirectoryNotFoundException($"Папка не найдена: {folderPath}");

        // Получаем все файлы с расширением .jpg.
        var jpgFiles = Directory.GetFiles(folderPath, "*.jpg");

        // Возвращаем количество файлов.
        return jpgFiles.Length;
    }
}