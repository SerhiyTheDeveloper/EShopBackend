namespace MINT.EShop.Core.Options
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
    }
}
