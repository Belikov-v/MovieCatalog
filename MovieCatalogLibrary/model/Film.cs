namespace MovieCatalogLibrary.model;

/// <summary>
/// Класс, представляющий фильм.
/// </summary>
public class Film
{
    // Уникальный идентификатор фильма.
    public int Id { get; set; }

    // Название фильма. По умолчанию задано значение "default".
    public string Name { get; set; } = "default";

    // Жанр фильма.
    public GenreOfFilm Genre { get; set; }

    // Поле для хранения года выпуска фильма.
    private int _releaseYear;

    // Поле для хранения рейтинга фильма.
    private int _rating;

    /// <summary>
    /// Свойство для доступа к году выпуска фильма.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если год выпуска выходит за допустимые пределы (1800 - текущий год).
    /// </exception>
    public int ReleaseYear
    {
        get => _releaseYear; // Возвращает значение поля _releaseYear.
        set
        {
            // Проверка, что год выпуска находится в допустимом диапазоне.
            if (value < 1800 || value > DateTime.Now.Year)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Год выпуска должен быть между 1800 и текущим годом.");
            }

            _releaseYear = value; // Устанавливает значение поля _releaseYear.
        }
    }

    /// <summary>
    /// Свойство для доступа к рейтингу фильма.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если рейтинг выходит за допустимые пределы (1 - 10).
    /// </exception>
    public int Rating
    {
        get => _rating; // Возвращает значение поля _rating.
        set
        {
            // Проверка, что рейтинг находится в допустимом диапазоне.
            if (value is < 1 or > 10)
                throw new ArgumentOutOfRangeException(nameof(value), "Рейтинг должен быть от 1 до 10.");
            _rating = value; // Устанавливает значение поля _rating.
        }
    }

    /// <summary>
    /// Переопределение метода ToString для получения строкового представления объекта Film.
    /// </summary>
    /// <returns>Строка, содержащая информацию о фильме.</returns>
    public override string ToString()
    {
        return $"Id:{Id} \n" +
               $"Название:{Name} \n" +
               $"Жанр:{Genre} \n" +
               $"Год выпуска:{ReleaseYear} \n" +
               $"Рейтинг:{Rating}";
    }
}