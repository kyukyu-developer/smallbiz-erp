using MediatR;
using Identity.Application.DTOs.Auth;
using ERP.Shared.Contracts.Common;

namespace Identity.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<Result<LoginResponseDto>>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
