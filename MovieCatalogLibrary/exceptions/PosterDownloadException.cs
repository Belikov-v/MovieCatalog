namespace MovieCatalogLibrary.exceptions
{
    /// <summary>
    /// Исключение, которое выбрасывается при ошибке скачивания постера.
    /// </summary>
    public class PosterDownloadException : Exception
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public PosterDownloadException()
        {
        }

        /// <summary>
        /// Конструктор с сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public PosterDownloadException(string? message) : base(message)
        {
        }
    }
}