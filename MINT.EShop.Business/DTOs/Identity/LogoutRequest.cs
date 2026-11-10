namespace MINT.EShop.Business.DTOs.Identity
{
    public record LogoutRequest
    {
        public required string RefreshToken { get; init; }
    }
}
