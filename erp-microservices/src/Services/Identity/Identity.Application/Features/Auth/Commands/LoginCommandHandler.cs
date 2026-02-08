using MediatR;
using Identity.Application.DTOs.Auth;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using ERP.Shared.Contracts.Common;

namespace Identity.Application.Features.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Find user by username
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !user.IsActive)
                return Result<LoginResponseDto>.Failure("Invalid credentials");

            // Verify password
            if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
                return Result<LoginResponseDto>.Failure("Invalid credentials");

            // Generate tokens
            var accessToken = _jwtTokenService.GenerateToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // Save refresh token
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            var response = new LoginResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };

            return Result<LoginResponseDto>.Success(response);
        }
    }
}
