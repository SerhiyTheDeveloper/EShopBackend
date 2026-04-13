namespace MINT.EShop.API.Wrappers
{
    /// <summary>
    /// Обгортка для відповідей з API.
    /// </summary>
    public record APIResponse
    {
        protected APIResponse(bool success, string message, List<string>? errors = null) 
        {
            Success = success;
            Message = message;
            Errors = errors ?? [];
        }
        /// <summary>
        /// Статус операції: true — все ок, false — сталася помилка.
        /// </summary>
        /// <example>true</example>
        public bool Success { get; init; }
        /// <summary>
        /// Текстове повідомлення.
        /// </summary>
        /// <example>The operation was completed successfully.</example>
        public string Message { get; init; }
        /// <summary>
        /// Список помилок при виконанні операції.
        /// </summary>
        /// <example>["Invalid product ID", "Product not found"]</example>
        public List<string> Errors { get; init; }
        /// <summary>
        /// Точний час створення відповіді.
        /// </summary>
        /// <example>2024-06-01T14:30:00Z</example>
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
        /// <summary>
        /// Формує успішну відповідь.
        /// </summary>
        public static APIResponse SuccessResponse(string message = "The operation was completed successfully.")
        {
            return new APIResponse(true, message);
        }
        /// <summary>
        /// Формує відповідь з помилкою.
        /// </summary>
        public static APIResponse FailureResponse(string message = "The operation ended in error.", List<string>? errors = null)
        {
            return new APIResponse(false, message, errors);
        }
    }

    /// <summary>
    /// Універсальна обгортка для відповідей API.
    /// </summary>
    /// <typeparam name="T">Тип даних, які ми передаємо.</typeparam>
    public record APIResponse<T> : APIResponse
    {
        private APIResponse(bool success, T? data, string message, List<string>? errors = null) : base(success, message, errors) 
        {
            Data = data;
        }
        /// <summary>
        /// Самі дані. Якщо Success = false, тут зазвичай null.
        /// </summary>
        public T? Data { get; init; }
        /// <summary>
        /// Формує успішну відповідь з переданими даними.
        /// </summary>
        public static APIResponse<T> SuccessResponse(T? data, string message = "The operation was completed successfully.")
        {
            return new APIResponse<T>(true, data, message);
        }
        /// <summary>
        /// Формує відповідь з помилкою.
        /// </summary>
        public static new APIResponse<T> FailureResponse(string message = "The operation ended in error.", List<string>? errors = null) 
        {
            return new APIResponse<T>(false, default, message, errors);
        }
    }
}
