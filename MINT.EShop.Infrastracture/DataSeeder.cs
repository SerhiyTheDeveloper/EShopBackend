using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Core.Enums;
using MINT.EShop.Core.Interfaces;
using MINT.EShop.Core.Options;


namespace MINT.EShop.Infrastracture
{
    public class DataSeeder(AppDbContext dbContext, IOptions<AdminOptions> adminOptions, IOptions<ManagerOptions> managerOptions) : IDataSeeder
    {
        private readonly AdminOptions _adminOptions = adminOptions.Value;
        private readonly ManagerOptions _managerOptions = managerOptions.Value;
        public async Task SeedAsync()
        {
            if (!await dbContext.Users.AnyAsync())
            {
                var admin = new User
                {
                    Email = _adminOptions.Email,
                    FirstName = _adminOptions.FirstName,
                    Role = Role.Admin
                };
                var credential = new UserCredential
                {
                    UserId = admin.Id,
                    PasswordHash = _adminOptions.Password
                };
                var clientAccount = new ClientAccount
                {
                    UserId = admin.Id,
                    PhoneNumber = _adminOptions.PhoneNumber
                };
                admin.Credential = credential;
                admin.ClientAccount = clientAccount;

                var manager = new User
                {
                    Email = _managerOptions.Email,
                    FirstName = _managerOptions.FirstName,
                    Role = Role.Manager
                };
                var managerCredential = new UserCredential
                {
                    UserId = manager.Id,
                    PasswordHash = _managerOptions.Password
                };
                var managerClientAccount = new ClientAccount
                {
                    UserId = manager.Id,
                    PhoneNumber = _managerOptions.PhoneNumber
                };
                manager.Credential = managerCredential;
                manager.ClientAccount = managerClientAccount;

                dbContext.Users.Add(admin);
                dbContext.Users.Add(manager);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
