using MINT.EShop.Business.DTOs.Identity;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Core.Enums;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Business.Services
{
    public class UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IUserService
    {
        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            // Дістаємо всіх користувачів з бази даних
            var users = await unitOfWork.Users.GetAllAsync();
            
            // Повертаємо їх у вигляді колекції UserResponse
            return users.Select(user => new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        public async Task<UserResponse?> GetByIdAsync(Guid id)
        {
            // Дістаємо користувача за його ідентифікатором та перевіряємо його існування
            var user = await unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return null;

            // Повертаємо користувача у вигляді UserResponse
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<UserResponse> CreateAsync(RegisterRequest request)
        {
            // Створюємо нового користувача на основі даних з RegisterRequest
            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            // Створюємо об'єкт UserCredential для збереження хешу пароля
            var credential = new UserCredential
            {
                UserId = user.Id,
                PasswordHash = passwordHasher.Hash(request.Password)
            };

            // Створюємо об'єкт ClientAccount
            var clientAccount = new ClientAccount
            {
                UserId = user.Id,
            };

            // Прив'язуємо UserCredential до користувача
            user.Credential = credential;

            // Прив'язуємо ClientAccount до користувача
            user.ClientAccount = clientAccount;

            // Додаємо користувача до бази даних та зберігаємо зміни
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.CompleteAsync();

            // Повертаємо створеного користувача у вигляді UserResponse
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<UserResponse?> UpdateDataAsync(Guid id, UpdateUserDataRequest request)
        {
            // Дістаємо існуючого користувача за його ідентифікатором та перевіряємо його існування
            var existingUser = await unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null)
                return null;

            // Оновлюємо дані користувача на основі даних з UpdateUserDataRequest
            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;

            // Завершимо транзакцію та зберігаємо зміни в базі даних
            await unitOfWork.CompleteAsync();

            // Повертаємо оновленого користувача у вигляді UserResponse
            return new UserResponse
            {
                Id = existingUser.Id,
                Email = existingUser.Email,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            // Дістаємо існуючого користувача за його ідентифікатором та перевіряємо його існування
            var existingUser = await unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null)
                return false;

            // Видаляємо користувача з бази даних та зберігаємо зміни
            unitOfWork.Users.Delete(existingUser);
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> PromoteToManagerAsync(Guid id)
        {
            // Дістаємо існуючого користувача за його ідентифікатором та перевіряємо його існування
            var existingUser = await unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null)
                return false;

            // Перевіряємо, чи користувач вже має роль "Manager" або "Admin"
            if (existingUser.Role == Role.Manager)
                throw new InvalidOperationException("User is already a manager.");
            if (existingUser.Role == Role.Admin)
                throw new InvalidOperationException("Cannot promote an admin to manager.");

            // Оновлюємо роль користувача на "Manager"
            existingUser.Role = Role.Manager;

            // Завершимо транзакцію та зберігаємо зміни в базі даних
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DemoteToClientAsync(Guid id)
        {
            // Дістаємо існуючого користувача за його ідентифікатором та перевіряємо його існування
            var existingUser = await unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null)
                return false;

            // Перевіряємо, чи користувач має роль "Client" або "Admin"
            if (existingUser.Role == Role.Client)
                throw new InvalidOperationException("User is already a client.");
            if (existingUser.Role == Role.Admin)
                throw new InvalidOperationException("Cannot demote an admin to client.");

            // Оновлюємо роль користувача на "Client"
            existingUser.Role = Role.Client;
            // Завершимо транзакцію та зберігаємо зміни в базі даних
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
