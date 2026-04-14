namespace MINT.EShop.Business.DTOs.Identity;

public record UserData
{
    /// <summary>
    /// Електронна пошта користувача.
    /// </summary>
    /// <example>user@example.com</example>
    public required string Email { get; init; }
    /// <summary>
    /// Захешований пароль
    /// </summary>
    public required string PasswordHash { get; init; }
    /// <summary>
    /// Ім'я
    /// </summary>
    /// <example>Марія</example>
    public required string FirstName { get; init; }
    /// <summary>
    /// Прізвище (може бути відсутнім)
    /// </summary>
    /// <example>Кузьма</example>
    public required string? LastName { get; init; }
    /// <summary>
    /// Верифікаційний код (6 значень)
    /// </summary>
    /// <example>487426</example>
    public required string VerificationCode { get; init; }
}