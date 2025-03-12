using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ImageResizer.Application.Constants.Auth;
using ImageResizer.Configuration;
using ImageResizer.Configuration.Models;
using ImageResizer.Domain.Models.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ImageResizer.Application.Services.AccessToken
{
    public class AccessTokenService(
        UserManager<ApplicationUser> userManager,
        IOptions<ResizerSettings> resizerOptions) : IAccessTokenService
    {
        private readonly JWTSettingsElement _jwtSettings = resizerOptions.Value.JWTSettings;

        public async Task<string> CreateAccessTokenAsync(ApplicationUser user)
        {
            // Using custom claims here because ASP.NET Identity maps the 'sub' claim to http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier and
            // 'email' claim to http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress.
            List<Claim> claims =
            [
                new Claim(JwtCustomClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtCustomClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, _jwtSettings.Audience),
            ];

            await userManager.AddClaimsAsync(user, claims);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.IssuerSigningKey)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<(ClaimsPrincipal principal, SecurityToken validatedToken)> ValidateAccessTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.IssuerSigningKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return Task.FromResult((principal, validatedToken));
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token", ex);
            }
        }
    }
}