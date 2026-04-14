using Microsoft.IdentityModel.Tokens;
using MINT.EShop.Business.DTOs.Identity;
using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Core.Interfaces;
using System.Security.Cryptography;

namespace MINT.EShop.Business.Services
{
    public class AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtProvider jwtProvider) : IAuthService
    {
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            // Отримуємо користувача і перевіряємо його на null
            var user = await unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user == null) return null;
            
            // Звіряємо переданий пароль користувача з паролем у базі даних
            var isMatched = passwordHasher.Verify(request.Password, user.Credential.PasswordHash);
            if (!isMatched) return null;

            // Генеруємо AccessToken, RefreshToken і ExpiresDate
            var accessToken = jwtProvider.GenerateToken(user);
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiresDate = DateTime.UtcNow.AddDays(7);

            // Формуємо сесію користувача
            var userSession = new UserSession
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                ExpiresDate = expiresDate,
            };

            // Додаємо сесію до бази даних
            unitOfWork.Users.AddSession(userSession);

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

            // Формуємо та повертаємо відповідь у форматі LoginResponse
            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = userSession.RefreshToken,
                ExpiresDate = userSession.ExpiresDate
            };
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshRequest request)
        {
            // Отримуємо ClaimsPrincipal з AccessToken
            var principal = jwtProvider.GetPrincipalFromExpiredToken(request.AccessToken);

            // Витягуємо userId з ClaimsPrincipal і перевіряємо його на валідність
            var userIdClaim = principal.FindFirst(p => p.Type == "id");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new SecurityTokenException("Invalid token claims");
            }

            // Отримуємо користувача з бази даних
            var user = await unitOfWork.Users.GetByIdAsync(userId) 
                ?? throw new SecurityTokenException("User does not exist");

            // Знаходимо сесію користувача, яка відповідає переданому RefreshToken
            var userSession = user.Sessions.FirstOrDefault(s => s.RefreshToken == request.RefreshToken)
                ?? throw new SecurityTokenException("Invalid refresh token");

            // Перевіряємо, чи сесія активна (не прострочена)
            if (!userSession.IsActive)
            {
                user.Sessions.Remove(userSession);
                await unitOfWork.CompleteAsync();
                throw new SecurityTokenException("Refresh token expired");
            }

            // Генеруємо нові AccessToken, RefreshToken і ExpiresDate
            var accessToken = jwtProvider.GenerateToken(user);
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            DateTime expiresDate = DateTime.UtcNow.AddDays(7);

            // Оновлюємо сесію користувача новими даними
            userSession.RefreshToken = refreshToken;
            userSession.ExpiresDate = expiresDate;

            // Завершуємо транзакцію, щоб зберегти зміни в базі даних
            await unitOfWork.CompleteAsync();

            // Повертаємо нові токени
            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = userSession.RefreshToken,
                ExpiresDate = userSession.ExpiresDate
            };
        }
    }
}
