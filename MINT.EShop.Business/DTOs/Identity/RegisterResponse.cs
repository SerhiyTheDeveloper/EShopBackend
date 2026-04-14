namespace MINT.EShop.Business.DTOs.Identity;

public record RegisterResponse
{
    /// <summary>
    /// Електронна пошта користувача.
    /// </summary>
    /// <example>user@example.com</example>
    public required string Email { get; init; }
    /// <summary>
    /// Ім'я
    /// </summary>
    /// <example>Марія</example>
    public required string FirstName { get; init; }
    /// <summary>
    /// Прізвище (може бути відсутнім)
    /// </summary>
    /// <example>Кузьма</example>
    public string? LastName { get; init; }
}