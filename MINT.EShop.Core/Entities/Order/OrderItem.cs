namespace MINT.EShop.Core.Entities.Order
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public required Guid OrderId { get; set; }
        public required Guid ProductId { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } = default;
        public Order Order { get; set; } = null!;
        public Product.Product Product { get; set; } = null!;
    }
}