using MINT.EShop.Core.Enums;

namespace Web.Models
{
    /// <summary>
    /// Дані доставки й оплати, які покупець вказує при оформленні замовлення.
    /// </summary>
    public record CheckoutDetails(
        string Region,
        string City,
        DeliveryService DeliveryService,
        string DeliveryPoint,
        PaymentMethod PaymentMethod,
        string RecipientPhone);
}
