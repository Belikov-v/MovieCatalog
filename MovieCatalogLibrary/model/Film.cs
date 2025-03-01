namespace MovieCatalogLibrary.model
{
    /// <summary>
    /// Класс, представляющий фильм.
    /// </summary>
    public class Film
    {
        /// <summary>
        /// Уникальный идентификатор фильма.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название фильма.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание сюжета фильма.
        /// </summary>
        public string Plot { get; set; } = string.Empty;

        /// <summary>
        /// Режиссер фильма.
        /// </summary>
        public string Director { get; set; } = string.Empty;

        /// <summary>
        /// Список актеров, участвующих в фильме.
        /// </summary>
        public List<string> Actors { get; set; } = [];

        /// <summary>
        /// Жанр фильма.
        /// </summary>
        public GenreOfFilm Genre { get; set; }

        /// <summary>
        /// Путь к постеру фильма.
        /// </summary>
        public string Poster { get; set; } = string.Empty;

        /// <summary>
        /// Рейтинг фильма на IMDb.
        /// </summary>
        public string ImdbRating { get; init; } = string.Empty;

        /// <summary>
        /// Рейтинг фильма на Metacritic.
        /// </summary>
        public string MetacriticRating { get; init; } = string.Empty;

        /// <summary>
        /// Ссылка на трейлер фильма.
        /// </summary>
        public string TrailerUrl { get; init; } = string.Empty;

        // Поле для хранения года выпуска фильма.
        private int _releaseYear;

        // Поле для хранения рейтинга фильма.
        private int _rating;

        /// <summary>
        /// Год выпуска фильма.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если год выпуска выходит за допустимые пределы (1800 - текущий год).
        /// </exception>
        public int ReleaseYear
        {
            get => _releaseYear;
            set
            {
                if (value < 1800 || value > DateTime.Now.Year)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "Год выпуска должен быть между 1800 и текущим годом.");
                _releaseYear = value;
            }
        }

        /// <summary>
        /// Рейтинг фильма.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если рейтинг выходит за допустимые пределы (1 - 10).
        /// </exception>
        public int Rating
        {
            get => _rating;
            set
            {
                if (value is < 1 or > 10)
                    throw new ArgumentOutOfRangeException(nameof(value), "Рейтинг должен быть от 1 до 10.");
                _rating = value;
            }
        }

        /// <summary>
        /// Возвращает строковое представление объекта Film.
        /// </summary>
        /// <returns>Строка, содержащая информацию о фильме.</returns>
        public override string ToString()
        {
            return $"Id: {Id}\n" +
                   $"Название: {Name}\n" +
                   $"Жанр: {Genre}\n" +
                   $"Год выпуска: {ReleaseYear}\n" +
                   $"Рейтинг: {Rating}\n" +
                   $"Режиссер: {Director}\n" +
                   $"Актеры: {string.Join(", ", Actors)}\n" +
                   $"Описание сюжета: {Plot}\n" +
                   $"Рейтинг IMDb: {ImdbRating}\n" +
                   $"Рейтинг Metacritic: {MetacriticRating}\n" +
                   $"Трейлер: {TrailerUrl}";
        }
    }
}