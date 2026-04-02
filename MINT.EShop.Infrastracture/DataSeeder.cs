using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Core.Enums;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture
{
    public class DataSeeder(AppDbContext dbContext, IOptions<AdminOptions> options) : IDataSeeder
    {
        private readonly AdminOptions _options = options.Value;
        public async Task SeedAsync()
        {
            if (!await dbContext.Users.AnyAsync())
            {
                var adminUser = new User
                {
                    Email = _options.Email,
                    FirstName = _options.FirstName,
                    Role = Role.Admin
                };

                var credential = new UserCredential
                {
                    UserId = adminUser.Id,
                    PasswordHash = _options.Password
                };

                var clientAccount = new ClientAccount
                {
                    UserId = adminUser.Id,
                };

                adminUser.Credential = credential;
                adminUser.ClientAccount = clientAccount;

                dbContext.Users.Add(adminUser);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
