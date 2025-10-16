using DevMentorHub.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DevMentorHub.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor accessor) { _httpContextAccessor = accessor; }
        public Guid UserId
        {
            get
            {
                var idValue = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Guid.TryParse(idValue, out var id) ? id : Guid.Empty;
            }
        }
        public string Email => User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
    }
}