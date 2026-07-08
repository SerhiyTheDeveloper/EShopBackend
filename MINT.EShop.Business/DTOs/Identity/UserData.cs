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
    /// Прізвище
    /// </summary>
    /// <example>Кузьма</example>
    public string? LastName { get; init; }
    /// <summary>
    /// Номер телефону
    /// </summary>
    /// <example>0938057198</example>
    public required string PhoneNumber { get; init; }
    /// <summary>
    /// Верифікаційний код (6 значень)
    /// </summary>
    /// <example>487426</example>
    public required string VerificationCode { get; init; }
}