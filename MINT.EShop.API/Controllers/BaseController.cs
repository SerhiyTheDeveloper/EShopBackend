using Microsoft.AspNetCore.Mvc;
using MINT.EShop.Core.Enums;
using System.Security.Claims;

namespace MINT.EShop.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected Guid CurrentUserId
        {
            get
            {
                var claimValue = User.Claims.FirstOrDefault(c => c.Type == "id")!.Value;
                return Guid.Parse(claimValue);
            }
        }

        protected Guid CurrentClientAccountId
        {
            get
            {
                var claimValue = User.Claims.FirstOrDefault(c => c.Type == "ClientAccountId")!.Value;
                return Guid.Parse(claimValue);
            }
        }

        protected Role CurrentUserRole
        {
            get
            {
                var claimValue = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;
                return Enum.Parse<Role>(claimValue, true);
            }
        }
    }
}