namespace MINT.EShop.Core.Entities.UserData
{
    public class ClientAccount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string PhoneNumber { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public List<WishListItem> Wishlist { get; set; } = [];
        public List<Order.Order> Orders { get; set; } = [];
    }
}