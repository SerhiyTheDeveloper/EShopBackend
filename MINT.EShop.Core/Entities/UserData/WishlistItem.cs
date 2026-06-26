namespace MINT.EShop.Core.Entities.UserData
{
    public class WishListItem
    {
        public required Guid ClientId { get; set; }
        public required Guid ProductId { get; set; }
        public ClientAccount ClientAccount { get; set; } = null!;
        public Product.Product Product { get; set; } = null!;
    }
}
