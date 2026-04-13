using MINT.EShop.Core.Entities.UserData;
using System.Security.Claims;

namespace MINT.EShop.Core.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
