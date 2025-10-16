using MediatR;
using DevMentorHub.Application.DTOs;

namespace DevMentorHub.Application.Commands
{
    public class RegisterUserCommand : IRequest<AuthResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }

    public class LoginUserCommand : IRequest<AuthResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}