using MINT.EShop.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CheckoutFormModel
    {
        // Область обирається зі списку (SearchSelect працює з Guid), назву беремо за цим Id
        public Guid RegionId { get; set; }

        [Required(ErrorMessage = "Введіть місто")]
        [MaxLength(100, ErrorMessage = "Максимум 100 символів")]
        public string City { get; set; } = string.Empty;

        public DeliveryService DeliveryService { get; set; } = DeliveryService.NovaPoshta;

        [Required(ErrorMessage = "Вкажіть відділення або адресу")]
        [MaxLength(200, ErrorMessage = "Максимум 200 символів")]
        public string DeliveryPoint { get; set; } = string.Empty;

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CashOnDelivery;

        // Номер після +380 (його перевіряємо вручну, бо користувач може ввести пробіли чи 0 на початку)
        public string Phone { get; set; } = string.Empty;
    }
}
