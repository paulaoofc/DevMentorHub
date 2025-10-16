using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DevMentorHub.Application.Interfaces;
using DevMentorHub.Domain.Entities;
using DevMentorHub.Application.DTOs;
using DevMentorHub.Application.Commands;
using System;

namespace DevMentorHub.Application.CommandHandlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IJwtService jwtService, IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken) != null)
            {
                return null!;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = hashedPassword,
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddUserAsync(newUser, cancellationToken);

            var token = _jwtService.GenerateToken(newUser);

            return new AuthResponse
            {
                Token = token,
                RefreshToken = string.Empty,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;

        public LoginUserCommandHandler(IJwtService jwtService, IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user == null) return null!;
            var ok = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!ok) return null!;

            var token = _jwtService.GenerateToken(user);
            return new AuthResponse
            {
                Token = token,
                RefreshToken = string.Empty,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}