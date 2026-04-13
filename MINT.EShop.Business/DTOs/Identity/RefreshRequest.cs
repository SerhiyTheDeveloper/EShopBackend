namespace MINT.EShop.Business.DTOs.Identity
{
    public record RefreshRequest
    {
        /// <summary>
        /// Токен для автентифікації запитів
        /// </summary>
        public required string AccessToken { get; init; }
        /// <summary>
        /// Токен для отримання нового AccessToken після закінчення терміну його дії
        /// </summary>
        public required string RefreshToken { get; init; }
    }
}
