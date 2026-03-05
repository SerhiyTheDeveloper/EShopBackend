using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.API.Wrappers
{
    /// <summary>
    /// Універсальна обгортка для всіх відповідей API.
    /// </summary>
    /// <typeparam name="T">Тип даних, які ми передаємо.</typeparam>
    public record APIResponse<T>
    {
        /// <summary>
        /// Статус операції: true — все ок, false — сталася помилка.
        /// </summary>
        /// <example>true</example>
        public required bool Success { get; init; }
        /// <summary>
        /// Текстове повідомлення для фронтенду або користувача.
        /// </summary>
        /// <example>The operation was completed successfully.</example>
        public string Message { get; init; } = string.Empty;
        /// <summary>
        /// Самі дані. Якщо Success = false, тут зазвичай null.
        /// </summary>
        public T? Data { get; init; }
        /// <summary>
        /// Список помилок при виконанні операції.
        /// </summary>
        /// <example>["Invalid product ID", "Product not found"]</example>
        public List<string> Errors { get; init; } = [];
        /// <summary>
        /// Точний час створення відповіді.
        /// </summary>
        /// <example>2024-06-01T14:30:00Z</example>
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
        /// <summary>
        /// Формує успішну відповідь.
        /// </summary>
        public static APIResponse<T> SuccessResponse(T? data, string message = "The operation was completed successfully.") => new()
        {
            Success = true,
            Data = data,
            Message = message
        };
        /// <summary>
        /// Формує відповідь з помилкою.
        /// </summary>
        public static APIResponse<T> FailureResponse(string message = "The operation ended in error.", List<string>? errors = null) => new()
        {
            Success = false,
            Message = message,
            Errors = errors ?? []
        };
    }
}
