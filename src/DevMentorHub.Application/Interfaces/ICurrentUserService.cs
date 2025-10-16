using System.Security.Claims;

namespace DevMentorHub.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        string Email { get; }
        ClaimsPrincipal User { get; }
    }

    public interface IJwtService
    {
        string GenerateToken(Domain.Entities.User user);
    }
}