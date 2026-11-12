using MINT.EShop.Business.DTOs.OrderItems;
using MINT.EShop.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MINT.EShop.Business.DTOs.Orders
{
    public record CreateOrderRequest
    {
        /// <summary>
        /// Список товарів у замовленні.
        /// </summary>
        public required IReadOnlyList<CreateOrderItemRequest> OrderItems { get; init; }

        /// <summary>
        /// Область доставки.
        /// </summary>
        /// <example>Київська область</example>
        [Required(ErrorMessage = "Region is required.")]
        [MaxLength(100)]
        public required string Region { get; init; }

        /// <summary>
        /// Місто доставки.
        /// </summary>
        /// <example>Київ</example>
        [Required(ErrorMessage = "City is required.")]
        [MaxLength(100)]
        public required string City { get; init; }

        /// <summary>
        /// Служба доставки (NovaPoshta або UkrPoshta).
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required DeliveryService DeliveryService { get; init; }

        /// <summary>
        /// Номер відділення або адреса доставки.
        /// </summary>
        /// <example>Відділення №12</example>
        [Required(ErrorMessage = "Delivery point is required.")]
        [MaxLength(200)]
        public required string DeliveryPoint { get; init; }

        /// <summary>
        /// Спосіб оплати (Prepaid, CashOnDelivery).
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required PaymentMethod PaymentMethod { get; init; }

        /// <summary>
        /// Номер телефону одержувача.
        /// </summary>
        /// <example>+380932080545</example>
        [Required(ErrorMessage = "Recipient phone is required.")]
        [Phone]
        [MaxLength(20)]
        public required string RecipientPhone { get; init; }
    }
}
