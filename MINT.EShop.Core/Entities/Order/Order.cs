using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Core.Enums;

namespace MINT.EShop.Core.Entities.Order
{
    public class Order
    {
        public Guid Id { get; set; }
        public required Guid ClientAccountId { get; set; }
        public required DateTime OrderDate { get; set; }
        public required decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public required string Region { get; set; }
        public required string City { get; set; }
        public required DeliveryService DeliveryService { get; set; }
        public required string DeliveryPoint { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
        public required string RecipientPhone { get; set; }
        public ClientAccount ClientAccount { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = [];
    }
}