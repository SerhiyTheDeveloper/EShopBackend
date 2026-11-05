using Microsoft.AspNetCore.Components.Authorization;
using MimeKit.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Web.Services;

namespace Web
{
    public class CookieAuthenticationStateProvider(TokenStorageService tokenStorage) : AuthenticationStateProvider
    {
        private readonly TokenStorageService _tokenStorage = tokenStorage;
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var accessToken = _tokenStorage.GetAccessToken();

            if (accessToken == null)
            {
                var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
                return Task.FromResult(new AuthenticationState(anonymous));
            }

            var claims = GetClaimsFromJwt(accessToken);
            var identity = new ClaimsIdentity(claims, "cookie");
            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }

        public void NotifyUserLoggedIn(string accessToken)
        {
            var claims = GetClaimsFromJwt(accessToken);
            var identity = new ClaimsIdentity(claims, "cookie");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLoggedOut()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        private static List<Claim> GetClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims.ToList();
        }
    }
}
