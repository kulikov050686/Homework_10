namespace Models
{
    public class MessageLog
    {
        /// <summary>
        /// Время
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Имя пользователя отправившего сообщение
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>        
        public MessageLog(string time, string message, string firstName, long id)
        {
            Time = time;
            Id = id;
            Message = message;
            FirstName = firstName;
        }            
    }
}
