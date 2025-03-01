using MovieCatalogLibrary.model;
using Spectre.Console;

namespace MovieCatalogLibrary.service.visualizations;

/// <summary>
/// Класс для отображения карусели с обложками фильмов.
/// </summary>
public static class FilmCarousel
{
    private static int _offset; // Текущий индекс карусели

    /// <summary>
    /// Отображает карусель с обложками фильмов.
    /// </summary>
    /// <param name="films">Список фильмов.</param>
    public static void Render(List<Film> films)
    {
        // Проверка на null.
        if (films == null)
            throw new ArgumentNullException(nameof(films), "Список фильмов не может быть null.");

        // Проверка на пустой список.
        if (films.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Список фильмов пуст.[/]");
            return;
        }

        // Основной цикл карусели.
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("Используйте ← → для прокрутки, Esc - выход.");

            // Отображаем текущий фильм.
            RenderCurrentFilm(films);

            // Обрабатываем ввод пользователя.
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.LeftArrow: // Переход к предыдущему фильму.
                    _offset = (_offset - 1 + films.Count) % films.Count;
                    break;
                case ConsoleKey.RightArrow: // Переход к следующему фильму.
                    _offset = (_offset + 1) % films.Count;
                    break;
                case ConsoleKey.Escape: // Выход из карусели.
                    return;
            }
        }
    }

    /// <summary>
    /// Отображает обложку и информацию о текущем фильме.
    /// </summary>
    /// <param name="films">Список фильмов.</param>
    private static void RenderCurrentFilm(List<Film> films)
    {
        var currentFilm = films[_offset];

        // Проверяем, есть ли путь к обложке и доступен ли файл.
        if (string.IsNullOrEmpty(currentFilm.Poster) || !File.Exists(currentFilm.Poster))
        {
            AnsiConsole.MarkupLine("[red]Обложка отсутствует.[/]");
        }
        else
        {
            // Загружаем и отображаем изображение.
            var image = new CanvasImage(currentFilm.Poster).MaxWidth(20); // Ограничиваем ширину изображения.
            AnsiConsole.Write(image);
        }

        // Отображаем информацию о фильме.
        AnsiConsole.MarkupLine($"[bold]Id:[/] {currentFilm.Id}");
        AnsiConsole.MarkupLine($"[bold]Название:[/] {currentFilm.Name}");
        AnsiConsole.MarkupLine($"[bold]Год выпуска:[/] {currentFilm.ReleaseYear}");
        AnsiConsole.MarkupLine($"[bold]Рейтинг:[/] {currentFilm.Rating}");
        AnsiConsole.MarkupLine($"[bold]Жанр:[/] {currentFilm.Genre}");
        AnsiConsole.MarkupLine($"[bold]Режиссер:[/] {currentFilm.Director}");
        AnsiConsole.MarkupLine($"[bold]Актеры:[/] {string.Join(", ", currentFilm.Actors)}");
        AnsiConsole.MarkupLine($"[bold]Описание сюжета:[/] {currentFilm.Plot}");
        AnsiConsole.MarkupLine($"[bold]Рейтинг IMDb:[/] {currentFilm.ImdbRating}");
        AnsiConsole.MarkupLine($"[bold]Рейтинг Metacritic:[/] {currentFilm.MetacriticRating}");
        AnsiConsole.MarkupLine($"[bold]Ссылка на трейлер:[/] {currentFilm.TrailerUrl}");
    }
}