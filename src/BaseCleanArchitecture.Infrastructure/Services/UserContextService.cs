using BaseCleanArchitecture.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BaseCleanArchitecture.Infrastructure.Services
{
    public class UserContextService : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
        }

        public string? GetCurrentUserEmail()
        {
            var emailClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email);
            return emailClaim?.Value;
        }

        public int? GetCurrentUserEmpresaId()
        {
            var empresaClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("IdEmpresa");
            return empresaClaim != null ? int.Parse(empresaClaim.Value) : null;
        }
    }
} 