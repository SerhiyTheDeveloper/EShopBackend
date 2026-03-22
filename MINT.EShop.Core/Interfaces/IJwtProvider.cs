using MINT.EShop.Core.Entities.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Core.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
