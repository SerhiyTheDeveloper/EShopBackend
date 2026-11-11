using MINT.EShop.API.Wrappers;
using MINT.EShop.Business.DTOs.Identity;

namespace Web.Services
{
    /// <summary>
    /// Клієнт до ендпоінтів поточного користувача (api/v1/users/me).
    /// </summary>
    public class UserApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
    {
        public Task<APIResponse<UserResponse>> GetCurrentAsync(string accessToken, CancellationToken cancToken = default)
            => SendAsync<UserResponse>(HttpMethod.Get, "api/v1/users/me", accessToken, null, cancToken);

        public Task<APIResponse<UserResponse>> UpdateCurrentAsync(string accessToken, UpdateUserDataRequest request, CancellationToken cancToken = default)
            => SendAsync<UserResponse>(HttpMethod.Put, "api/v1/users/me", accessToken, request, cancToken);
    }
}
