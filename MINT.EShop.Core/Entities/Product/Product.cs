namespace MINT.EShop.Core.Entities.Product
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public Guid? ManagerId { get; set; }
        public required Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public required Guid ProducerId { get; set; }
        public Producer Producer { get; set; } = null!;
    }
}
