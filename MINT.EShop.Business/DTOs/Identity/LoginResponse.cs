namespace MINT.EShop.Business.DTOs.Identity
{
    /// <summary>
    /// Відповідь після успішної автентифікації користувача
    /// </summary>
    public record LoginResponse
    {
        /// <summary>
        /// Токен для автентифікації запитів
        /// </summary>
        public required string AccessToken { get; init; }
        /// <summary>
        /// Токен для отримання нового AccessToken після закінчення терміну його дії
        /// </summary>
        public required string RefreshToken { get; init; }
        /// <summary>
        /// Дата та час закінчення дії RefreshToken (UTC)
        /// </summary>
        public required DateTime ExpiresDate { get; init; }
    }
}
