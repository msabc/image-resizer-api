using System.IdentityModel.Tokens.Jwt;
using ImageResizer.Application.Constants.Auth;
using ImageResizer.Domain.Interfaces.Services;

namespace ImageResizer.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            IsAuthenticated = httpContextAccessor != null
                                         && httpContextAccessor.HttpContext != null
                                         && httpContextAccessor.HttpContext.User != null
                                         && httpContextAccessor.HttpContext.User.Identity != null
                                         && httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

            UserId = IsAuthenticated ? Guid.Parse(GetClaim(JwtCustomClaimNames.Sub)!) : throw ThrowUnauthorized(JwtCustomClaimNames.Sub);
            EmailAddress = IsAuthenticated ? GetClaim(JwtCustomClaimNames.Email)! : throw ThrowUnauthorized(JwtCustomClaimNames.Email);
        }

        public bool IsAuthenticated { get; private set; }

        public Guid UserId { get; private set; }

        public string EmailAddress { get; private set; }

        private string? GetClaim(string claimType)
        {
            return _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

        private static UnauthorizedAccessException ThrowUnauthorized(string missingClaim) => new($"Missing authorization claim {missingClaim}");
    }
}
