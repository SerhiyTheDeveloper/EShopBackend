namespace Web.Models
{
    /// <summary>
    /// Частина відповіді API про створене замовлення, потрібна Web (решту полів ігноруємо).
    /// </summary>
    public record OrderCreatedResponse(Guid Id, decimal TotalAmount);
}
