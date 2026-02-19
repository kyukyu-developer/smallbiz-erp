using MediatR;
using ERP.Application.DTOs.Auth;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Auth.Commands
{
    public class RegisterCommand : IRequest<Result<LoginResponseDto>>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
