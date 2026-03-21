using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
    }
}
