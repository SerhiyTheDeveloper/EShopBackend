namespace MINT.EShop.Business.DTOs.Identity;

public record VerifyRequest
{
    /// <summary>
    /// Електронна пошта користувача.
    /// </summary>
    /// <example>user@example.com</example>
    public required string Email { get; init; }
    /// <summary>
    /// Верифікаційний код (6 значень)
    /// </summary>
    /// <example>487426</example>
    public required string VerificationCode { get; init; }
}