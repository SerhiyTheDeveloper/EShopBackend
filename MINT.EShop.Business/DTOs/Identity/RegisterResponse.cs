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
    /// Номер телефону
    /// </summary>
    /// <example>0937054525</example>
    public required string PhoneNumber { get; init; }
}