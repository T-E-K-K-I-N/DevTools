namespace DevTools.Kafka.Exceptions
{
    /// <summary>
    /// Невалидное тело сообщения
    /// </summary>
    public sealed class InvalidMessagePayloadException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InvalidMessagePayloadException"/>
        /// </summary>
        public InvalidMessagePayloadException() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InvalidMessagePayloadException"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        public InvalidMessagePayloadException(string? message) { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InvalidMessagePayloadException"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="innerException">Внутреннее исключение</param>
        public InvalidMessagePayloadException(string? message, Exception? innerException) { }
    }
}
