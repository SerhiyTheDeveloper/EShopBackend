using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Identity;

namespace Web.Services
{
    /// <summary>
    /// Клієнт до ендпоінтів адміна: список користувачів та зміна ролей.
    /// </summary>
    public class AdminApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
    {
        public Task<APIResponse<List<UserResponse>>> GetUsersAsync(string accessToken, CancellationToken cancToken = default)
            => SendAsync<List<UserResponse>>(HttpMethod.Get, "api/v1/users", accessToken, null, cancToken);

        public Task<APIResponse<object>> PromoteToManagerAsync(string accessToken, Guid userId, CancellationToken cancToken = default)
            => SendAsync<object>(HttpMethod.Post, $"api/Admin/users/{userId}/promote", accessToken, null, cancToken);

        public Task<APIResponse<object>> DemoteToClientAsync(string accessToken, Guid userId, CancellationToken cancToken = default)
            => SendAsync<object>(HttpMethod.Post, $"api/Admin/users/{userId}/demote", accessToken, null, cancToken);

        public Task<APIResponse<object>> DeleteUserAsync(string accessToken, Guid userId, CancellationToken cancToken = default)
            => SendAsync<object>(HttpMethod.Delete, $"api/v1/users/{userId}", accessToken, null, cancToken);
    }
}
