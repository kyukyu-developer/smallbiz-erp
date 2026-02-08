using MediatR;
using Identity.Application.DTOs.Auth;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using ERP.Shared.Contracts.Common;

namespace Identity.Application.Features.Auth.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<LoginResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<LoginResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if username already exists
            var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUser != null)
                return Result<LoginResponseDto>.Failure("Username already exists");

            // Check if email already exists
            var existingEmail = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingEmail != null)
                return Result<LoginResponseDto>.Failure("Email already exists");

            // Create user with hashed password
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password),
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

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
